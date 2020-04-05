using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStore
{
    public class Scorer
    {
        private readonly Room _room;

        public Scorer(Room room)
        {
            _room = room;
        }

        public Dictionary<string, int> CalculateScore()
        {
            var scoreByPersonId = new Dictionary<string, int>();
            int numberOfPeople = _room.PersonIds.Count;
            int numberOfChameleons = _room.PersonIds.Count(x => _room.GetCharacterFor(x) == "chameleon");
            
            Dictionary<string, int> personByVoteCount = new Dictionary<string, int>();
            foreach (var id in _room.PersonIds)
            {
                personByVoteCount[id] = _room.PersonIds.Count(x => _room.GetVotedFor(x) == id);
            }
            
            int votesForChameleons = _room.PersonIds
                .Where(x => _room.GetCharacterFor(x) == "chameleon")
                .Sum(x => personByVoteCount[x]);
            
            foreach (var id in _room.PersonIds)
            {
                int score;
                if (_room.GetCharacterFor(id) == "chameleon")
                {
                    score = numberOfPeople - numberOfChameleons - votesForChameleons;
                    score += (int)Math.Round(((float)votesForChameleons / numberOfChameleons) - personByVoteCount[id]);
                }
                else
                {
                    score = votesForChameleons - personByVoteCount[id];
                    if (_room.GetCharacterFor(_room.GetVotedFor(id)) == "chameleon")
                    {   
                        score += 1;
                    }
                }

                scoreByPersonId[id] = score;
            }
            return scoreByPersonId;
        }
    }
}