using BlackJack.Data;   
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

namespace BlackJack.View
{
    /// <summary>
    /// �J�[�h��View
    /// </summary>
    public class CardView : MonoBehaviour
    {
        [SerializeField]
        [Header("�g�����v�̕\��Image")]
        Image _cardFrontImage;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        [Header("�J�[�h�����J����Ƃ��̃A�j���[�V�����̃X�e�[�g��")]
        private string _cardOpenAnimName = "Open";
        
        public CardView SetSprite(Sprite sprite)
        {
            _cardFrontImage.sprite = sprite;
            gameObject.SetActive(true);
            _animator.Play(_cardOpenAnimName);
            return this;
        }
    }
}