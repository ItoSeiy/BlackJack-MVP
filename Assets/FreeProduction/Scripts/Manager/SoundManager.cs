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
        [Header("BGM�𗬂�AudioSource")]
        AudioSource _bgmAudioSource;

        [SerializeField]
        [Header("Audio�̃I�u�W�F�N�g�̐e�I�u�W�F�N�g")]
        GameObject _audioObjParent;

        List<Pool> _pool = new List<Pool>();

        int _poolCountIndex = 0;

        protected override void Awake()
        {
            base.Awake();
            CreatePool();
            //�f�o�b�O�p
            //_pool.ForEach(x => Debug.Log($"�I�u�W�F�N�g��:{x.Object.name}���: {x.Type}"));
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
        /// �w�肵���I�[�f�B�I�\�[�X�̃t�F�[�h���s��
        /// </summary>
        /// <param name="targetAudioSouece">�t�F�[�h�������I�[�f�B�I�\�[�X</param>
        /// <param name="targetVol">�ݒ肵��������</param>
        /// <param name="fadeTime">�ǂ̂��炢���Ԃ������邩</param>
        public void FadeAudioSource(AudioSource targetAudioSouece, float targetVol, float fadeTime)
        {
            targetAudioSouece.DOFade(targetVol, fadeTime);
        }

        /// <summary>
        /// ���ʉ����g�������Ƃ��ɌĂяo���֐�
        /// </summary>
        /// <param name="name">���������T�E���h�̖��O</param>
        /// <returns></returns>
        public AudioSource UseSFX(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Debug.LogWarning("���ʉ����w�肳��Ă��܂���");
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
                Debug.LogError($"{name}�Ƃ����T�E���h��������܂���ł��� �뎚���͐ݒ�̂��Y�ꂪ����܂��B");
                return null;
            }

            var newSound = Instantiate(_soundObjParam.Params.Find(x => x.Name == name).Audio, _audioObjParent.transform);
            _pool.Add(new Pool { Audio = newSound, Name = name });
            newSound.gameObject.SetActive(true);
            return newSound;
        }

        /// <summary>
        /// �ݒ肵���I�u�W�F�N�g�̎��,�������v�[���ɃI�u�W�F�N�g�𐶐����Ēǉ�����
        /// �ċA�Ăяo����p���Ă���
        /// </summary>
        private void CreatePool()
        {
            if (_poolCountIndex >= _soundObjParam.Params.Count)
            {
                //�f�o�b�O�p
                //Debug.Log("���ׂẴI�[�f�B�I�𐶐����܂����B");
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