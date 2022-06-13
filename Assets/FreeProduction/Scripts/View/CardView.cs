using BlackJack.Data;   
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
using Photon.Pun;

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
        private string _openCardAnimName = "Open";
        
        public CardView SetSprite(Sprite sprite, bool doOpen = true)
        {
            _cardFrontImage.sprite = sprite;
            gameObject.SetActive(true);

            if(doOpen == true)
            {
                OpenCard();
            }

            return this;
        }

        public void OpenCard()
        {
            _animator.Play(_openCardAnimName);
        }
    }
}