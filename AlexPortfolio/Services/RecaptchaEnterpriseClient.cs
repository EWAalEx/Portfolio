using Google.Cloud.RecaptchaEnterprise.V1;

namespace AlexPortfolio.Services;

public class RecaptchaEnterpriseClient : IRecaptchaEnterpriseClient
{
    public RecaptchaEnterpriseClient()
    {
    }

    public RecaptchaEnterpriseServiceClient CreateClient()
    {
        return RecaptchaEnterpriseServiceClient.Create();
    }

    public RecaptchaEnterpriseServiceClient CreateClient(RecaptchaEnterpriseServiceClientBuilder clientBuilder)
    {
        return clientBuilder.Build();
    }

    public Assessment CreateAssessment(RecaptchaEnterpriseServiceClient client, CreateAssessmentRequest request)
    {
        return client.CreateAssessment(request);
    }

    public async Task<Assessment> CreateAssessmentAsync(RecaptchaEnterpriseServiceClient client, CreateAssessmentRequest request, CancellationToken cancellationToken = default)
    {
        return await client.CreateAssessmentAsync(request, cancellationToken);
    }
}
