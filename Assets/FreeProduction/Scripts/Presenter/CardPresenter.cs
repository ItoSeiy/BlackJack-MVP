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
        #region Inspector Variables

        [SerializeField]
        [Header("ゲーム終了後にカードを破棄するまでの時間(ミリ秒)")]
        private int _timeToDoDispose = 3000;

        [SerializeField]
        CardView _cardViewPrefab;

        [SerializeField]
        private Transform _playerCardViewParent;

        [SerializeField]
        private Transform _dealerCardViewParent;

        #endregion

        #region Member Variables

        private List<CardView> _drewPlayerCards = new List<CardView>();

        private List<CardView> _drewDealerCards = new List<CardView>();

        private int _dealerDrawCounter = 0;

        #endregion

        #region Constant

        private const int DEALER_INITIAL_UP_CARD_NUM_INDEX = 0;

        private const int DEALER_HOLE_CARD_NUM_INDEX = 1;


        #endregion

        #region Unity Methods

        private void Awake()
        {
            SubscribeCard();
            BoardModel.Instance.OnInitialize += SubscribeCard;
        }

        #endregion

        #region Privete Methods

        private void SubscribeCard()
        {
            BoardModel.Instance.ObservableLatestPlayerCard
                .Where(x => x.Sprite != null)
                .Subscribe(onNext: GeneratePlayerCard,
                      onCompleted: Init);

            BoardModel.Instance.ObservableLatestDealerCard
                .Where(x => x.Sprite != null)
                .Subscribe(x =>
                {
                    _dealerDrawCounter++;
                    if(_dealerDrawCounter >= 3)
                    {
                        // 3回目以降のディーラーのドローはトランプを常に公開する
                        GenerateDealerCard(x, true);
                    }
                    else
                    {
                        GenerateDealerCard(x, false);
                    }
                });

            BoardModel.Instance.OnOpenInitialUpCard += OpenInitialUpCard;
            BoardModel.Instance.OnOpenHoleCard += OpenHoleCard;
        }

        private void GeneratePlayerCard(CardData cardData)
        {
            _drewPlayerCards.Add(Instantiate(_cardViewPrefab, _playerCardViewParent)
                .SetSprite(cardData.Sprite));
        }

        private void GenerateDealerCard(CardData cardData, bool doOpen)
        {
            _drewDealerCards.Add(Instantiate(_cardViewPrefab, _dealerCardViewParent)
                .SetSprite(cardData.Sprite, doOpen));
        }

        private async void Init()
        {
            await Task.Delay(_timeToDoDispose);

            _drewPlayerCards.ForEach(x => Destroy(x.gameObject));
            _drewDealerCards.ForEach(x => Destroy(x.gameObject));

            _drewPlayerCards.Clear();
            _drewDealerCards.Clear();

            _dealerDrawCounter = 0;
        }

        private void OpenInitialUpCard()
        {
            _drewDealerCards[DEALER_INITIAL_UP_CARD_NUM_INDEX].OpenCard();
        }

        private void OpenHoleCard()
        {
            _drewDealerCards[DEALER_HOLE_CARD_NUM_INDEX].OpenCard();
        }

        #endregion
    }
}

