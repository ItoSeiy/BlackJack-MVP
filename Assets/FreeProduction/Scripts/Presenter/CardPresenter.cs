using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackJack.Model;
using BlackJack.View;
using UniRx;
using BlackJack.Data;

namespace BlackJack.Presenter
{
    /// <summary>
    /// �g�����v�̕`��Ɋւ��钇���(Presenter)
    /// </summary>
    public class CardPresenter : MonoBehaviour
    {
        [SerializeField]
        CardView _cardViewPrefab;

        [SerializeField]
        private Transform _playerCardViewParent;

        private void Awake()
        {
            BoardModel.Instance.ObservablePlayerCurrentCard
                .Where(x => x.Sprite != null)
                .Subscribe(OnDrawPlayerCard);
        }

        private void OnDrawPlayerCard(CardData cardData)
        {
        }
    }
}

