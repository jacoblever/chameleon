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
                var roomCode = request.QueryStringParameters["RoomCode"];
                var personId = request.GetChameleonPersonIdHeader();

                ChameleonGame.Create().LeaveRoom(roomCode, personId);
                response.StatusCode = 204;
                response.Body = "";
            });
        }
    }
}
