using System;

namespace GameLogic
{
    public class ChameleonCannotVoteException : Exception
    {
        public ChameleonCannotVoteException() : base("Chameleons can't vote (because they know who the chameleon is!)")
        {
        }
    }
}
