namespace AlexPortfolio.Services;
using Google.Cloud.RecaptchaEnterprise.V1;

public interface IRecaptchaEnterpriseClient
{
    RecaptchaEnterpriseServiceClient CreateClient();
    RecaptchaEnterpriseServiceClient CreateClient(RecaptchaEnterpriseServiceClientBuilder clientBuilder);

    Assessment CreateAssessment(RecaptchaEnterpriseServiceClient client, CreateAssessmentRequest request);

    Task<Assessment> CreateAssessmentAsync(RecaptchaEnterpriseServiceClient client, CreateAssessmentRequest request, CancellationToken cancellationToken = default);
}