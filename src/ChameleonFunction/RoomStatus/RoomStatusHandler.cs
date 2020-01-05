using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using GameLogic;
using Newtonsoft.Json;

namespace ChameleonFunction.RoomStatus
{
    public class RoomStatusHandler
    {
        public APIGatewayProxyResponse Handle(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return new Responder().Respond(response =>
            {
                var status = ChameleonGame.Create().GetRoomStatus(
                    request.GetChameleonRoomCode(),
                    request.GetChameleonPersonIdHeader());
                response.StatusCode = 200;
                response.Body = JsonConvert.SerializeObject(status);
                response.Headers.Add("Content-Type", "application/json");
            });
        }
    }
}
