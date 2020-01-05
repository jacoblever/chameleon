using System;
using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using GameLogic;

namespace ChameleonFunction
{
    public class Responder
    {
        public delegate void Handler(APIGatewayProxyResponse response);
        
        public APIGatewayProxyResponse Respond(Handler handler)
        {
            var response = new APIGatewayProxyResponse
            {
                Headers = new Dictionary<string, string>
                {
                    { "Access-Control-Allow-Origin", "*" },
                },
            };

            try
            {
                handler(response);
            }
            catch (RoomDoesNotExistException e)
            {
                response.StatusCode = 404;
                response.Body = e.Message;
            }
            catch (PersonNotInRoomException e)
            {
                response.StatusCode = 403;
                response.Body = e.Message;
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                response.Body = $"{e.Message}\n{e.StackTrace}";
            }

            return response;
        }
    }
}
