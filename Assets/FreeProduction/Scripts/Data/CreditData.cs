using System;

namespace BlackJack.Data
{
    [Serializable]
    public struct CreditData
    {
        public int Credit;

        public CreditData(int credit)
        {
            Credit = credit;
        }
    }
}