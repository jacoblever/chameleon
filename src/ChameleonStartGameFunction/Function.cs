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
            var personId = request.Headers["x-chameleon-personid"];

            try
            {
                ChameleonGame.Create().StartGame(roomCode, personId);
                return new APIGatewayProxyResponse
                {
                    StatusCode = 204,
                    Body = "",
                    Headers = new Dictionary<string, string>(),
                };
            }
            catch (PersonNotInRoomException e)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 403,
                    Body = e.Message,
                    Headers = new Dictionary<string, string>(),
                };
            }
            catch (Exception e)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = $"{e.Message}\n{e.StackTrace}",
                    Headers = new Dictionary<string, string>(),
                };
            }
        }
    }
}
