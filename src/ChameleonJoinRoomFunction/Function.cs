using System;
using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using GameLogic;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ChameleonJoinRoomFunction
{
    public class Function
    {
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var headers = new Dictionary<string, string>
            {
                { "Access-Control-Allow-Origin", "*" },
            };
            try
            {
                var chameleonGame = ChameleonGame.Create();
                var person = new RoomJoiner(chameleonGame)
                    .Join(request.Body);
                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = JsonConvert.SerializeObject(person),
                    Headers = headers,
                };
            }
            catch (PersonNameMustBeSpecifiedException e)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 400,
                    Body = e.Message,
                    Headers = headers,
                };
            }
            catch (RoomCodeMustBeValidException e)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 400,
                    Body = e.Message,
                    Headers = headers,
                };
            }
            catch (RoomDoesNotExistException e)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 404,
                    Body = e.Message,
                    Headers = headers,
                };
            }
            catch (Exception e)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = $"{e.Message}\n{e.StackTrace}",
                    Headers = headers,
                };
            }
        }
    }
}
