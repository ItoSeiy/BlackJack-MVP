using BlackJack.Data;   
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

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