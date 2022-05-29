using BlackJack.Data;   
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
using Photon.Pun;

namespace BlackJack.View
{
    /// <summary>
    /// カードのView
    /// </summary>
    public class CardView : MonoBehaviour
    {
        [SerializeField]
        [Header("トランプの表のImage")]
        Image _cardFrontImage;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        [Header("カードを公開するときのアニメーションのステート名")]
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