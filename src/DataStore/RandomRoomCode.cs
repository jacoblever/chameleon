using System;
using System.Text;

namespace DataStore
{
    public class RandomRoomCode : IRandomRoomCode
    {
        private const string Letters = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public string Generate()
        {
            var random = new Random();
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
