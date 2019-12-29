using System.Collections.Generic;
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
            var roomCode = request.QueryStringParameters["RoomCode"];
            var personId = GetPersonId(request);

            try
            {
                var status = ChameleonGame.Create().GetRoomStatus(roomCode, personId);
                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = JsonConvert.SerializeObject(status),
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/json" },
                        { "Access-Control-Allow-Origin", "*" },
                    },
                };
            }
            catch (RoomDoesNotExistException e)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 404,
                    Body = e.Message,
                    Headers = new Dictionary<string, string>
                    {
                        { "Access-Control-Allow-Origin", "*" },
                    },
                };
            }
            catch (PersonNotInRoomException e)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 403,
                    Body = e.Message,
                    Headers = new Dictionary<string, string>
                    {
                        { "Access-Control-Allow-Origin", "*" },
                    },
                };
            }
        }

        private static string GetPersonId(APIGatewayProxyRequest request)
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
