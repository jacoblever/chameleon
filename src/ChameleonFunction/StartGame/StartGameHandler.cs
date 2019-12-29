using System;
using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using GameLogic;

namespace ChameleonFunction.StartGame
{
    public class StartGameHandler
    {
        public APIGatewayProxyResponse Handle(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var roomCode = request.QueryStringParameters["RoomCode"];
            var personId = GetPersonId(request);

            try
            {
                ChameleonGame.Create().StartGame(roomCode, personId);
                return new APIGatewayProxyResponse
                {
                    StatusCode = 204,
                    Body = "",
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
            catch (Exception e)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = $"{e.Message}\n{e.StackTrace}",
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
