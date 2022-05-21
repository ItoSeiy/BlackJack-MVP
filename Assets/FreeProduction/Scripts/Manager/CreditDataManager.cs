using BlackJack.Data;
using System;
using UniRx;
using UnityEngine;
using static BlackJack.Model.JsonModel;
using static BlackJack.Model.FileUtils;

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

        /// <summary>初回読み込みのパス</summary>
        private const string SAVE_DATA_PATH_FOR_INITIAL_LOAD = "Data/CreditData";

        private const string DATA_FILE_NAME = "CreditData.json";

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
            CreateJson(_creditData, GetWritableDirectoryPath() + "/" + DATA_FILE_NAME);
        }

        private void LoadCredit()
        {
            try
            {
                // 読み書き可能なファイルからデータを読み込んでみる
                _creditData = LoadJson<CreditData>(GetWritableDirectoryPath() + "/" + DATA_FILE_NAME);
            }
            catch (Exception)
            {
                // 初回読み込み時はデータがリソースファイルにしかないためリソースファイルから読み込む
                _creditData = LoadJsonFromResources<CreditData>(SAVE_DATA_PATH_FOR_INITIAL_LOAD);
            }
        }
    }
}