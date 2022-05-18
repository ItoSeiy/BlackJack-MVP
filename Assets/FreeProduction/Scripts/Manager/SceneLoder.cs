using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace BlackJack.Manager
{
    /// <summary>
    /// シーンの読み込みを行うクラス
    /// DontDestroyである
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

        /// <summary>シーンの読み込み前</summary>
        public event Action OnLoadStart;

        /// <summary>シーンの読み込み後</summary>
        public event Action OnLoadEnd;

        #endregion

        #region Public Methods

        /// <summary>
        /// シーンをロードする関数
        /// フェードも行う
        /// </summary>
        /// <param name="sceneName">シーン名</param>
        /// <returns></returns>
        public void LoadScene(string sceneName)
        {
            // 既にロード中であればロードを行わない
            if (IsLoading == true) return;

            StartCoroutine(Fade(sceneName));
        }

        /// <summary>
        /// 現在のシーンを再度読み込むオーバーロード
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