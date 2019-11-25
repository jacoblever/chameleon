using System;
using System.IO;
using System.Linq;

namespace GameLogic
{
    public class Words
    {
        public string GetRandomWord()
        {
            const string wordsFile = "./words.txt";
            var wordCount = File.ReadLines(wordsFile).Count();
            var wordToPick = new Random().Next(0, wordCount);
            var i = 0;
            foreach (var word in File.ReadLines(wordsFile))
            {
                if (i == wordToPick)
                {
                    return word;
                }
                i++;
            }

            throw new IndexOutOfRangeException("Failed to find a word");
        }
    }
}
