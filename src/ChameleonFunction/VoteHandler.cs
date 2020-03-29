using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using GameLogic;
using Newtonsoft.Json.Linq;

namespace ChameleonFunction
{
    public class VoteHandler
    {
        public APIGatewayProxyResponse Handle(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return new Responder().Respond(response =>
            {
                var body = JObject.Parse(request.Body);
                var voteToken = body["Vote"];

                ChameleonGame.Create().Vote(
                    request.GetChameleonRoomCode(),
                    request.GetChameleonPersonIdHeader(),
                    voteToken.ToString());
                
                response.StatusCode = 204;
                response.Body = "";
            });
        }
    }
}
