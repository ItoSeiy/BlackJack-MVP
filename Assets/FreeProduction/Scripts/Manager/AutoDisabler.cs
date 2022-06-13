using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace BlackJack
{
    public class AutoDisabler : MonoBehaviour
    {
        [SerializeField]
        private int _disableTimeMilliSecond = 2000;

        private async void OnEnable()
        {
            await Task.Delay(_disableTimeMilliSecond);
        }
    }
}