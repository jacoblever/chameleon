using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using GameLogic;

namespace ChameleonFunction.LeaveRoom
{
    public class LeaveRoomHandler
    {
        public APIGatewayProxyResponse Handle(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return new Responder().Respond(response =>
            {
                ChameleonGame.Create().LeaveRoom(
                    request.GetChameleonRoomCode(),
                    request.GetChameleonPersonIdHeader());
                response.StatusCode = 204;
                response.Body = "";
            });
        }
    }
}
