using BlackJack.Data;   
using UnityEngine;
using UnityEngine.UI;
using System;

namespace BlackJack.View
{
    /// <summary>
    /// カードのView
    /// </summary>
    public class CardView : MonoBehaviour
    {
        [SerializeField]
        [Header("トランプの画像")]
        Image _cardImage;
        
        public void SetSprite(Sprite sprite)
        {
            _cardImage.sprite = sprite;
            gameObject.SetActive(true);
            // アニメーション等を流す
        }
    }
}