using BlackJack.Data;   
using UnityEngine;
using UnityEngine.UI;
using System;

namespace BlackJack.View
{
    /// <summary>
    /// �J�[�h��View
    /// </summary>
    public class CardView : MonoBehaviour
    {
        [SerializeField]
        [Header("�g�����v�̉摜")]
        Image _cardImage;
        
        public void SetSprite(Sprite sprite)
        {
            _cardImage.sprite = sprite;
            gameObject.SetActive(true);
            // �A�j���[�V�������𗬂�
        }
    }
}