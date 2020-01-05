using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using GameLogic;

namespace ChameleonFunction.StartGame
{
    public class StartGameHandler
    {
        public APIGatewayProxyResponse Handle(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return new Responder().Respond(response =>
            {
                ChameleonGame.Create().StartGame(
                    request.GetChameleonRoomCode(),
                    request.GetChameleonPersonIdHeader());
                response.StatusCode = 204;
                response.Body = "";
            });
        }
    }
}
