using System;
using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using GameLogic;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ChameleonStartGameFunction
{
    public class Function
    {
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
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
