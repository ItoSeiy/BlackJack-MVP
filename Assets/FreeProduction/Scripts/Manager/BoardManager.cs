using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackJack.Manager
{
    /// <summary>
    /// �v���C���[��f�B�[���̎�D���Ǘ�����N���X
    /// </summary>
    public class BoardManager : SingletonMonoBehaviour<BoardManager>
    {
        private int _dealerCardNum;
        private int _playerCardNum;

        public void DrawCard(Person person)
        {
            switch (person)
            {
                case Person.Player:

                    
                    
                    break;

                case Person.Dealer:



                    break;
            }
        }

        public enum Person
        {
            Player,
            Dealer
        }
    }
}