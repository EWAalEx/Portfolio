namespace AlexPortfolio.Services;

public interface IAuthenticationService
{
    /// <summary>
	/// Creates an assessment to analyse the risk of a UI action through Google ReCAPTCHA.
	/// </summary>
	/// <param name="recaptchaKey">The reCAPTCHA key associated with the site.</param>
	/// <param name="projectID">Your Google Cloud project ID.</param>
	/// <param name="token">The generated token obtained from the client.</param>
	/// <param name="recaptchaAction">Action name corresponding to the token.</param>
	/// <returns>A boolean result where true indicates a successful reCAPTCHA request and false indicates a failure</returns>
	public bool CreateGoogleCaptchaAssessment(string recaptchaKey, string projectID, string token, string recaptchaAction);

    /// <summary>
    /// Creates an assessment to analyse the risk of a UI action through Google ReCAPTCHA.
    /// </summary>
    /// <param name="token">The generated token obtained from the client.</param>
    /// <param name="recaptchaAction">Action name corresponding to the token.</param>
    /// <returns>A boolean result where true indicates a successful reCAPTCHA request and false indicates a failure</returns>
    public bool CreateGoogleCaptchaAssessment(string token = "action-token", string recaptchaAction = "action-name");
}
