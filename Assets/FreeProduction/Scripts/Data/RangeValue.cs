using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackJack.Data
{
    /// <summary>
    /// �l�^<T>�͈̔�(�J�n�l, �I���l)���i�[����\����
    /// </summary>
    /// <typeparam name="T">System.Int32, System.Single, System.Double���̍\����</typeparam>
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
