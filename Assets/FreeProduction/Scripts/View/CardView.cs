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
        Image _cardImage;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        [Header("カードを公開するときのアニメーションのステート名")]
        private string _cardOpenAnimName = "Open";
        
        public void SetSprite(Sprite sprite)
        {
            _cardImage.sprite = sprite;
            gameObject.SetActive(true);
            _animator.Play(_cardOpenAnimName);
        }
    }
}