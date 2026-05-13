using AlexPortfolio.Models;
using Google.Api.Gax.Grpc;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.RecaptchaEnterprise.V1;
using Google.Protobuf.Collections;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace AlexPortfolio.Services;

public class AuthenticationService : IAuthenticationService
{
    private const double RecaptchaLimit = 0.5;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IMemoryCache _cache;
    private readonly IRecaptchaEnterpriseClient _recpatchaEnterpriseClient;

    public AuthenticationService(IConfiguration configuration, ILogger<AuthenticationService> logger, IMemoryCache cache, IRecaptchaEnterpriseClient recpatchaEnterpriseClient)
    {
        _configuration = configuration;
        _logger = logger;
        _cache = cache;
        _recpatchaEnterpriseClient = recpatchaEnterpriseClient;
    }

    /// <summary>
	/// Creates an assessment to analyse the risk of a UI action through Google ReCAPTCHA.
	/// </summary>
	/// <param name="recaptchaKey">The reCAPTCHA key associated with the site.</param>
	/// <param name="projectID">Your Google Cloud project ID.</param>
	/// <param name="token">The generated token obtained from the client.</param>
	/// <param name="recaptchaAction">Action name corresponding to the token.</param>
	/// <returns>A boolean result where true indicates a successful reCAPTCHA request and false indicates a failure</returns>
	public bool CreateGoogleCaptchaAssessment(string recaptchaKey, string projectID, string token, string recaptchaAction)
    {
        try
        {
            // Start getting Service Account Secrets
            string? serviceAccountSecret = _configuration["Recaptcha:ServiceAccountSecretLocation"];

            // If service account secret isn't set log an error and fail the request
            if (string.IsNullOrWhiteSpace(serviceAccountSecret))
            {
                _logger.LogError("serviceAccountSecret not set in Configuration");
                return false;
            }

            // If service account secret is missing from file system log an error and fail the request
            if (!File.Exists($"{serviceAccountSecret}.json"))
            {
                _logger.LogError("serviceAccountSecret not found in file system");
                return false;
            }

            // Get Service Account json
            string json = File.ReadAllText($"{serviceAccountSecret}.json");

            GrpcNetClientAdapter grpcAdapter = GrpcNetClientAdapter.Default.WithAdditionalOptions(options => options.HttpHandler = new HttpClientHandler());

            RecaptchaEnterpriseServiceClientBuilder clientBuilder = new RecaptchaEnterpriseServiceClientBuilder() { GrpcAdapter = grpcAdapter };
            // Set Service Account Credentials
            clientBuilder.JsonCredentials = json;

            // Create the reCAPTCHA client
            RecaptchaEnterpriseServiceClient client = _recpatchaEnterpriseClient.CreateClient(clientBuilder);

            ProjectName projectName = new ProjectName(projectID);

            // Build the assessment request
            CreateAssessmentRequest createAssessmentRequest = new CreateAssessmentRequest()
            {
                Assessment = new Assessment()
                {
                    Event = new Event()
                    {
                        SiteKey = recaptchaKey,
                        Token = token,
                        ExpectedAction = recaptchaAction
                    },
                },
                ParentAsProjectName = projectName
            };

            RecaptchaCachedResponse? response = null;

            // Check if the assessment request is cached
            string cacheKey = token;
            if (_cache.TryGetValue(cacheKey, out string? validationResult))
            {
                // If the request exists in the cache and isn't null, pull it out
                if (validationResult != null)
                {
                    response = JsonSerializer.Deserialize<RecaptchaCachedResponse>(validationResult);
                }
            }

            // If the repsonse result wasn't in the cache, make an assessment request and store it
            if (response == null)
            {
                Assessment captchaResponse = _recpatchaEnterpriseClient.CreateAssessment(client, createAssessmentRequest);

                response = new RecaptchaCachedResponse()
                {
                    TokenProperties = new()
                    {
                        Valid = captchaResponse.TokenProperties.Valid,
                        InvalidReason = captchaResponse.TokenProperties.InvalidReason,
                        Action = captchaResponse.TokenProperties.Action
                    },
                    RiskAnalysis = new()
                    {
                        Score = captchaResponse.RiskAnalysis.Score,
                        Reasons = captchaResponse.RiskAnalysis.Reasons
                    }
                };
            }

            // Check if the token is valid
            if (!response.TokenProperties.Valid)
            {
                _logger.LogWarning("The ReCAPTCHA CreateAssessment call failed because the token was: {response.TokenProperties.InvalidReason.ToString()}", response.TokenProperties.InvalidReason.ToString());
                return false;
            }

            // Check if the expected action was executed
            if (response.TokenProperties.Action != recaptchaAction)
            {
                _logger.LogWarning("The ReCAPTCHA CreateAssessment call failed because the token was: {response.TokenProperties.Action.ToString()} which does not match the expected action {recaptchaAction}", response.TokenProperties.Action.ToString(), recaptchaAction);
                return false;
            }

            // Get the risk score and the reason(s)
            // For more information on interpreting the assessment, see:
            // https://cloud.google.com/recaptcha-enterprise/docs/interpret-assessment
            double riskScore = (double)response.RiskAnalysis.Score;

            // If the Risk Score is lower than our allowed bound then consider the rquest a fail and log the reasons
            if (riskScore < RecaptchaLimit)
            {
                string reasons = "";
                
                response.RiskAnalysis.Reasons ??= new RepeatedField<RiskAnalysis.Types.ClassificationReason>();

                foreach (RiskAnalysis.Types.ClassificationReason classificationReason in response.RiskAnalysis.Reasons)
                {
                    reasons += $"{classificationReason.ToString()},\n";
                }

                _logger.LogWarning("ReCAPTCHA verification failed with a score of: {riskScore}/{RecaptchaLimit}\n\nReasons:\n{reasons}", riskScore, RecaptchaLimit, reasons);
                return false;
            }

            //Captcha was successful, store the successful request for later use rather than re-checking the assesment
            // If the request exists in the cache and isn't null, pull it out
            if (string.IsNullOrWhiteSpace(_cache.Get<string>(cacheKey)))
            {
                _logger.LogInformation("ReCAPTCHA verification succeeded with a score of: {riskScore}/{RecaptchaLimit}", riskScore, RecaptchaLimit);
                // Store the response in the cache for 3 mins, 1 mins longer than google captcha holds it.
                _cache.Set(cacheKey, JsonSerializer.Serialize(response), DateTime.Now.AddMinutes(3));
            }

            // ReCAPTCHA request successful
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Something went wrong when checking the reCAPTCHA request, error: {e}", e);
            return false;
        }

    }

    /// <summary>
    /// Creates an assessment to analyse the risk of a UI action through Google ReCAPTCHA.
    /// </summary>
    /// <param name="token">The generated token obtained from the client.</param>
    /// <param name="recaptchaAction">Action name corresponding to the token.</param>
    /// <returns>A boolean result where true indicates a successful reCAPTCHA request and false indicates a failure</returns>
    public bool CreateGoogleCaptchaAssessment(string token = "action-token", string recaptchaAction = "action-name")
    {
        // Get the Project ID and the Recaptcha Site Key from the appsettings
        string? projectID = _configuration["Recaptcha:ProjectID"];
        string? recaptchaKey = _configuration["Recaptcha:SiteKey"];

        // if projectID or recaptchaKey are invalid we cannot make a request
        if (string.IsNullOrWhiteSpace(projectID) || string.IsNullOrWhiteSpace(recaptchaKey))
        {
            if (string.IsNullOrWhiteSpace(projectID) && string.IsNullOrWhiteSpace(recaptchaKey))
            {
                _logger.LogError("Recaptcha:ProjectID and Recaptcha:SiteKey were null or whitespace and unable to be used in ReCAPTCHA verification");
            }
            else
            {
                _logger.LogError(projectID == null ? "Recaptcha:ProjectID was null or whitespace and unable to be used in ReCAPTCHA verification" : "Recaptcha:SiteKey was null or whitespace and unable to be used in ReCAPTCHA verification");
            }

            return false;
        }

        // Send appsettings values to complete CreateGoogleCaptchaAssessment() function to process the request
        return CreateGoogleCaptchaAssessment(recaptchaKey, projectID, token, recaptchaAction);
    }
}
