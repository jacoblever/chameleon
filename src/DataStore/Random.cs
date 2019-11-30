using System.Text;

namespace DataStore
{
    public class Random : IRandom
    {
        private const string Letters = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public string GenerateRoomCode()
        {
            var random = new System.Random();
            var code = new StringBuilder();
            for (var i = 0; i < 4; i++)
            {
                var index = random.Next(Letters.Length);
                code.Append(Letters[index]);
            }

            return code.ToString();
        }
    }
}
