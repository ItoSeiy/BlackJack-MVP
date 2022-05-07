using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackJack.Data
{
    /// <summary>
    /// 値型<T>の範囲(開始値, 終了値)を格納する構造体
    /// </summary>
    /// <typeparam name="T">System.Int32, System.Single, System.Double等の構造体</typeparam>
    public struct RangeValue<T> where T : struct
    {
        public T Start => _start;

        public T End => _end;

        private T _start;
        private T _end;

        public RangeValue(T start, T end)
        {
            _start = start;
            _end = end;
        }
    }
}
