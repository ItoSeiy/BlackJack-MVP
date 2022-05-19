using BlackJack.Data;
using System;
using UniRx;
using UnityEngine;
using static BlackJack.Model.JsonModel;

namespace BlackJack.Manager
{
    /// <summary>
    /// セーブデータ(所持金)を管理するManagerクラス
    /// DontDestroyである
    /// </summary>
    public class CreditDataManager : SingletonMonoBehaviour<CreditDataManager>
    {
        public CreditData Data => _creditData;

        public IObservable<int> ObservableCreditDataChange => _onCreditDataChange;

        [SerializeField]
        private string _inGameSceneName = "Main";

        private CreditData _creditData;

        private Subject<int> _onCreditDataChange = new Subject<int>();

        private const string SAVE_DATA_PATH_FOR_LOAD = "Data/CreditData";

        private string SAVE_DATA_PATH_FOR_CREATE;

        protected override void Awake()
        {
            base.Awake();

            LoadCredit();

            SceneLoder.Instance.LoadScene(_inGameSceneName);
        }

        private void OnApplicationQuit()
        {
            CreateCreditData();
        }

        public void UpdateCreditData(CreditData creditData)
        {
            _creditData = creditData;
            _onCreditDataChange.OnNext(_creditData.Credit);
        }

        public void CreateCreditData()
        {
            SAVE_DATA_PATH_FOR_CREATE = Application.dataPath + "/Resources/Data/CreditData.json";
            CreateJson(_creditData, SAVE_DATA_PATH_FOR_CREATE);
        }

        private void LoadCredit()
        {
            _creditData = LoadFromJson<CreditData>(SAVE_DATA_PATH_FOR_LOAD);
        }
    }
}