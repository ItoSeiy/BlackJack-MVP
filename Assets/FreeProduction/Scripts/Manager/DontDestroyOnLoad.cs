using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackJack.Manager
{
    /// <summary>
    /// �Q�[���I�u�W�F�N�g��DontDestroy�ɂ���N���X
    /// </summary>
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}