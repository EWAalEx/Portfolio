using Google.Cloud.RecaptchaEnterprise.V1;
using Google.Protobuf.Collections;

namespace AlexPortfolio.Models;

/// <summary>
/// The lowest requirements for the Recaptcha Response to analyse google recaptcha requests and manually cache them for 3 mins.
/// </summary>
public class RecaptchaCachedResponse
{
    public Properties TokenProperties { get; set; } = new Properties();
    public Analysis RiskAnalysis { get; set; } = new Analysis();

    public class Properties
    {
        public bool Valid { get; set; }
        public TokenProperties.Types.InvalidReason InvalidReason { get; set; }
        public string Action { get; set; } = "";
    }

    public class Analysis
    {
        public float Score { get; set; }
        public RepeatedField<RiskAnalysis.Types.ClassificationReason>? Reasons { get; set; }
    }
}