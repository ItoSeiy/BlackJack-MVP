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
        /// �J�[�h�̃f�[�^�Ɖ摜���Z�b�g�A�b�v����֐�
        /// 
        /// ���\�b�h���`�F�[���o����悤�ɂ��Ă���
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
            print($"�X�[�g��{_data.Suit} �G����{_data.Rank}" +
                  $"\n�摜��{_image.name} ������{_data.Num}");
            return this;
        }
    }
}