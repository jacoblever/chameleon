using System;
using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using DataStore;
using GameLogic;
using Newtonsoft.Json;

namespace ChameleonFunction.JoinRoom
{
    public class JoinRoomHandler
    {
        public APIGatewayProxyResponse Handle(APIGatewayProxyRequest request, ILambdaContext context)
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
            catch (PersonNameNotUniqueException e)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 409,
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
