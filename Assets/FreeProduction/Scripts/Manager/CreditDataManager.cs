using BlackJack.Data;
using UnityEngine;
using System.Collections;
using static BlackJack.Model.JsonModel;

namespace BlackJack.Manager
{
    /// <summary>
    /// セーブデータ(所持金)を管理するManagerクラス
    /// DontDestroyである
    /// </summary>
    public class CreditDataManager : SingletonMonoBehaviour<CreditDataManager>
    {
        public CreditData CreditData => _creditData;

        [SerializeField]
        private string _inGameSceneName = "Main";

        private CreditData _creditData;

        private const string SAVE_DATA_PATH_FOR_LOAD = "Data/CreditData";

        private string SAVE_DATA_PATH_FOR_CREATE;

        protected override void Awake()
        {
            base.Awake();

            SAVE_DATA_PATH_FOR_CREATE = Application.dataPath + "/Resources/Data/CreditData.json";

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
        }

        private void LoadCredit()
        {
            _creditData = LoadFromJson<CreditData>(SAVE_DATA_PATH_FOR_LOAD);
        }

        private void CreateCreditData()
        {
            CreateJson(_creditData, SAVE_DATA_PATH_FOR_CREATE);
        }
    }
}