using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

namespace BlackJack.Manager
{
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        public AudioSource BGMAudioSource => _bgmAudioSource;

        [SerializeField]
        SoundPoolParams _soundObjParam;

        [SerializeField]
        [Header("BGMを流すAudioSource")]
        AudioSource _bgmAudioSource;

        [SerializeField]
        [Header("Audioのオブジェクトの親オブジェクト")]
        GameObject _audioObjParent;

        List<Pool> _pool = new List<Pool>();

        int _poolCountIndex = 0;

        protected override void Awake()
        {
            base.Awake();
            CreatePool();
            //デバッグ用
            //_pool.ForEach(x => Debug.Log($"オブジェクト名:{x.Object.name}種類: {x.Type}"));
        }

        public void PlayeBGM(AudioClip bgmClip)
        {
            _bgmAudioSource.Pause();
            _bgmAudioSource.clip = bgmClip;
            _bgmAudioSource.volume = 1f;
            _bgmAudioSource.Play();
        }

        public void PlayeBGM(AudioClip bgmClip, float volumeScale)
        {
            _bgmAudioSource.Pause();
            _bgmAudioSource.clip = bgmClip;
            _bgmAudioSource.volume = volumeScale;
            _bgmAudioSource.Play();
        }

        /// <summary>
        /// 指定したオーディオソースのフェードを行う
        /// </summary>
        /// <param name="targetAudioSouece">フェードしたいオーディオソース</param>
        /// <param name="targetVol">設定したい音量</param>
        /// <param name="fadeTime">どのくらい時間をかけるか</param>
        public void FadeAudioSource(AudioSource targetAudioSouece, float targetVol, float fadeTime)
        {
            targetAudioSouece.DOFade(targetVol, fadeTime);
        }

        /// <summary>
        /// 効果音を使いたいときに呼び出す関数
        /// </summary>
        /// <param name="name">流したいサウンドの名前</param>
        /// <returns></returns>
        public AudioSource UseSFX(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Debug.LogWarning("効果音が指定されていません");
                return null;
            }

            foreach (var pool in _pool)
            {
                if (pool.Audio.gameObject.activeSelf == false && pool.Name == name)
                {
                    pool.Audio.gameObject.SetActive(true);
                    return pool.Audio;
                }
            }

            if (_soundObjParam.Params.Find(x => x.Name == name) == null)
            {
                Debug.LogError($"{name}というサウンドが見つかりませんでした 誤字又は設定のし忘れがあります。");
                return null;
            }

            var newSound = Instantiate(_soundObjParam.Params.Find(x => x.Name == name).Audio, _audioObjParent.transform);
            _pool.Add(new Pool { Audio = newSound, Name = name });
            newSound.gameObject.SetActive(true);
            return newSound;
        }

        /// <summary>
        /// 設定したオブジェクトの種類,数だけプールにオブジェクトを生成して追加する
        /// 再帰呼び出しを用いている
        /// </summary>
        private void CreatePool()
        {
            if (_poolCountIndex >= _soundObjParam.Params.Count)
            {
                //デバッグ用
                //Debug.Log("すべてのオーディオを生成しました。");
                return;
            }

            for (int i = 0; i < _soundObjParam.Params[_poolCountIndex].MaxCount; i++)
            {
                var audio = Instantiate(_soundObjParam.Params[_poolCountIndex].Audio, _audioObjParent.transform);

                var audioVolume = audio.volume;
                audio.volume = 0f;
                audio.Play();
                audio.volume = audioVolume;

                audio.gameObject.SetActive(false);
                _pool.Add(new Pool { Audio = audio, Name = _soundObjParam.Params[_poolCountIndex].Name });
            }

            _poolCountIndex++;
            CreatePool();
        }

        private class Pool
        {
            public AudioSource Audio { get; set; }
            public string Name { get; set; }
        }

        [Serializable]
        public class SoundPoolParams
        {
            public List<SoundPoolParam> Params => soundPoolParams;

            [SerializeField]
            private List<SoundPoolParam> soundPoolParams;

            [Serializable]
            public class SoundPoolParam
            {
                public string Name => name;
                public AudioSource Audio => audio;
                public int MaxCount => maxCount;

                [SerializeField]
                private string name;

                [SerializeField]
                private AudioSource audio;

                [SerializeField]
                private int maxCount;
            }
        }
    }
}