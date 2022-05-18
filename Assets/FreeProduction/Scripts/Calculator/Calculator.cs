using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackJack.Calculation
{
    /// <summary>
    /// 計算を行うクラス
    /// </summary>
    public static class Calculator
    {
        /// <summary>
        /// ブラックジャックで買った際の配当を返す
        /// </summary>
        /// <param name="betValue">掛け金</param>
        /// <returns>2.5倍の配当(少数切り捨て)</returns>
        public static int BlackJack(int betValue)
        {
            float betValueF = (float)betValue;
            float returnValueF = betValueF * 2.5f;
            return (int)returnValueF;
        }

        /// <summary>
        /// 通常に勝利した際の配当を返す
        /// </summary>
        /// <param name="betValue">掛け金</param>
        /// <returns>2倍の配当</returns>
        public static int NormalWin(int betValue)
        {
            return betValue * 2;
        }

    }
}