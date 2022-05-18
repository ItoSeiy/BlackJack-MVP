using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace BlackJack.Manager
{
    /// <summary>
    /// �V�[���̓ǂݍ��݂��s���N���X
    /// DontDestroy�ł���
    /// </summary>
    public class SceneLoder : SingletonMonoBehaviour<SceneLoder>
    {
        #region Properties

        public bool IsLoading { get; private set; } = false;

        public string ActiveSceneName => SceneManager.GetActiveScene().name;

        #endregion

        #region Inspector Variables

        [SerializeField]
        private float _fadeDuaration = 0.5f;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        #endregion

        #region Events

        /// <summary>�V�[���̓ǂݍ��ݑO</summary>
        public event Action OnLoadStart;

        /// <summary>�V�[���̓ǂݍ��݌�</summary>
        public event Action OnLoadEnd;

        #endregion

        #region Public Methods

        /// <summary>
        /// �V�[�������[�h����֐�
        /// �t�F�[�h���s��
        /// </summary>
        /// <param name="sceneName">�V�[����</param>
        /// <returns></returns>
        public void LoadScene(string sceneName)
        {
            // ���Ƀ��[�h���ł���΃��[�h���s��Ȃ�
            if (IsLoading == true) return;

            StartCoroutine(Fade(sceneName));
        }

        /// <summary>
        /// ���݂̃V�[�����ēx�ǂݍ��ރI�[�o�[���[�h
        /// </summary>
        /// <returns></returns>
        public void LoadScene()
        {
            if (IsLoading == true) return;

            StartCoroutine(Fade(ActiveSceneName));
        }

        #endregion

        #region Privete Methods

        private IEnumerator Fade(string sceneName)
        {
            IsLoading = true;
            OnLoadStart?.Invoke();

            yield return _canvasGroup.DOFade(1f, _fadeDuaration)
                .WaitForCompletion();

            yield return SceneManager.LoadSceneAsync(sceneName);

            OnLoadEnd?.Invoke();
            IsLoading = false;

            _canvasGroup.DOFade(0f, _fadeDuaration);
        }

        #endregion
    }
}