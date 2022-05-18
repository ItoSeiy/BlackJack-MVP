using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackJack.Calculation
{
    /// <summary>
    /// �v�Z���s���N���X
    /// </summary>
    public static class Calculator
    {
        /// <summary>
        /// �u���b�N�W���b�N�Ŕ������ۂ̔z����Ԃ�
        /// </summary>
        /// <param name="betValue">�|����</param>
        /// <returns>2.5�{�̔z��(�����؂�̂�)</returns>
        public static int BlackJack(int betValue)
        {
            float betValueF = (float)betValue;
            float returnValueF = betValueF * 2.5f;
            return (int)returnValueF;
        }

        /// <summary>
        /// �ʏ�ɏ��������ۂ̔z����Ԃ�
        /// </summary>
        /// <param name="betValue">�|����</param>
        /// <returns>2�{�̔z��</returns>
        public static int NormalWin(int betValue)
        {
            return betValue * 2;
        }

    }
}