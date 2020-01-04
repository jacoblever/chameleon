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
            return new Responder().Respond(response =>
            {
                try
                {
                    var chameleonGame = ChameleonGame.Create();
                    var person = new RoomJoiner(chameleonGame)
                        .Join(request.Body);

                    response.StatusCode = 200;
                    response.Body = JsonConvert.SerializeObject(person);
                }
                catch (PersonNameMustBeSpecifiedException e)
                {
                    response.StatusCode = 400;
                    response.Body = e.Message;
                }
                catch (RoomCodeMustBeValidException e)
                {
                    response.StatusCode = 400;
                    response.Body = e.Message;
                }
                catch (PersonNameNotUniqueException e)
                {
                    response.StatusCode = 409;
                    response.Body = e.Message;
                }
            });
        }
    }
}
