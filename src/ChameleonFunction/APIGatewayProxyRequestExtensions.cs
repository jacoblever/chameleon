using Amazon.Lambda.APIGatewayEvents;

namespace ChameleonFunction
{
    public static class APIGatewayProxyRequestExtensions
    {
        public static string GetChameleonPersonIdHeader(this APIGatewayProxyRequest request)
        {
            if (request.Headers.TryGetValue("x-chameleon-personid", out var personId))
            {
                return personId;
            }
            if (request.Headers.TryGetValue("X-Chameleon-Personid", out personId))
            {
                return personId;
            }

            return "";
        }
    }
}
