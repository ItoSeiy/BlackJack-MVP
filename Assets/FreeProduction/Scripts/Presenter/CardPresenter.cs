using BlackJack.Data;
using BlackJack.Model;
using BlackJack.View;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Threading.Tasks;

namespace BlackJack.Presenter
{
    /// <summary>
    /// トランプの描画に関する仲介役(Presenter)
    /// </summary>
    public class CardPresenter : MonoBehaviour
    {
        [SerializeField]
        [Header("ゲーム終了後にカードを破棄するまでの時間(ミリ秒)")]
        private int _timeToDoDispose = 3000;

        [SerializeField]
        CardView _cardViewPrefab;

        [SerializeField]
        private Transform _playerCardViewParent;


        private List<CardView> _drewCards = new List<CardView>();

        private void Awake()
        {
            SubscribePlayerCard();
        }
        
        private void SubscribePlayerCard()
        {
            BoardModel.Instance.ObservableLatestPlayerCard
                .Where(x => x.Sprite != null)
                .Subscribe(onNext: GeneratePlayerCard,
                      onCompleted: DestroyAllPlayerHand);
        }

        private void GeneratePlayerCard(CardData cardData)
        {
            print("Next");
            _drewCards.Add(Instantiate(_cardViewPrefab, _playerCardViewParent)
                .SetSprite(cardData.Sprite));
        }

        private async void DestroyAllPlayerHand()
        {
            print("Comp");
            await Task.Delay(_timeToDoDispose);
            _drewCards.ForEach(x => Destroy(x.gameObject));
            _drewCards.Clear();
        }
    }
}

