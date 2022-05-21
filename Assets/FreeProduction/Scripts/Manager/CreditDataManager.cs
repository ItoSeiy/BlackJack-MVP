using BlackJack.Data;
using System;
using UniRx;
using UnityEngine;
using static BlackJack.Model.JsonModel;
using static BlackJack.Model.FileUtils;

namespace BlackJack.Manager
{
    /// <summary>
    /// �Z�[�u�f�[�^(������)���Ǘ�����Manager�N���X
    /// DontDestroy�ł���
    /// </summary>
    public class CreditDataManager : SingletonMonoBehaviour<CreditDataManager>
    {
        public CreditData Data => _creditData;

        public IObservable<int> ObservableCreditDataChange => _onCreditDataChange;

        [SerializeField]
        private string _inGameSceneName = "Main";

        private CreditData _creditData;

        private Subject<int> _onCreditDataChange = new Subject<int>();

        /// <summary>����ǂݍ��݂̃p�X</summary>
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
                // �ǂݏ����\�ȃt�@�C������f�[�^��ǂݍ���ł݂�
                _creditData = LoadJson<CreditData>(GetWritableDirectoryPath() + "/" + DATA_FILE_NAME);
            }
            catch (Exception)
            {
                // ����ǂݍ��ݎ��̓f�[�^�����\�[�X�t�@�C���ɂ����Ȃ����߃��\�[�X�t�@�C������ǂݍ���
                _creditData = LoadJsonFromResources<CreditData>(SAVE_DATA_PATH_FOR_INITIAL_LOAD);
            }
        }
    }
}