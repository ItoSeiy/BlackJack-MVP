using BlackJack.Data;
using UnityEngine;
using UnityEngine.UI;

namespace BlackJack
{
    public class Card : MonoBehaviour
    {
        public CardData Data => _data;

        public Image Image => _image;

        private CardData _data;

        private Image _image;

        /// <summary>
        /// カードのデータと画像をセットアップする関数
        /// 
        /// メソッドをチェーン出来るようにしている
        /// </summary>
        public Card SetUp(CardData data, Image image, Sprite sprite)
        {
            _data = data;
            _image = image;
            _image.sprite = sprite;
            return this;
        }

        public Card Show()
        {
            print($"スートは{_data.Suit} 絵柄は{_data.Rank}" +
                  $"\n画像は{_image.name} 数字は{_data.Num}");
            return this;
        }
    }
}