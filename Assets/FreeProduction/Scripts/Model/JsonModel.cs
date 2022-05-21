using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System;

namespace BlackJack.Model
{
    public static class JsonModel
    {
        /// <summary>
        /// Resouces�t�@�C������Json�t�@�C��"����"��ǂݍ��ރW�F�l���b�N�֐�
        /// </summary>
        /// <typeparam name="T">�ǂݍ��݂����N���X</typeparam>
        /// <param name="path">�ǂݍ��݂����f�[�^�̃p�X</param>
        /// <returns>���X�g�^�̃f�[�^</returns>
        public static T[] LoadJsonsFromResources<T>(string path)
        {
            var objects = Resources.LoadAll(path);
            List<string> jsonStrs = new List<string>();
            List<T> datas = new List<T>();

            for (int i = 0; i < objects.Length; i++)
            {
                jsonStrs.Add(objects[i].ToString());
                datas.Add(JsonUtility.FromJson<T>(jsonStrs[i]));
            }

            return datas.ToArray();
        }

        /// <summary>
        /// Resouces�t�@�C������Json�t�@�C����ǂݍ��ފ֐�
        /// </summary>
        /// <typeparam name="T">�ǂݍ��݂����N���X</typeparam>
        /// <param name="path">�ǂݍ��݂����f�[�^�̃p�X</param>
        /// <returns>�ǂݍ��񂾃f�[�^</returns>
        public static T LoadJsonFromResources<T>(string path)
        {
            var jsonStr = Resources.Load(path).ToString();

            Debug.Log($"���\�[�X����ǂݍ���Json�t�@�C���̓��e{jsonStr}");

            return JsonUtility.FromJson<T>(jsonStr);
        }

        /// <summary>
        /// �w�肳�ꂽ�p�X����Json�t�@�C����ǂݍ��ފ֐�
        /// </summary>
        /// <typeparam name="T">�ǂݍ��݂����N���X</typeparam>
        /// <param name="path">�ǂݍ��݂����f�[�^�̃p�X</param>
        /// <returns>�ǂݍ��񂾃f�[�^</returns>
        public static T LoadJson<T>(string path)
        {
            using var reader = new StreamReader(path, System.Text.Encoding.GetEncoding("UTF-8"));

            var jsonStr = reader.ReadToEnd();
            reader.Close();

            Debug.Log($"�ʏ�t�@�C���ǂݍ���Json�t�@�C���̓��e{jsonStr}");

            return JsonUtility.FromJson<T>(jsonStr);
        }

        /// <summary>
        /// Json�t�@�C�������֐�
        /// </summary>
        /// <param name="data">��肽���f�[�^</param>
        /// <param name="path">�ǂݍ��݂����f�[�^�̃p�X</param>
        public static void CreateJson<T>(T data, string path)
        {
            using var writer = new StreamWriter(path);

            var jsonStr = JsonUtility.ToJson(data);

            Debug.Log($"�쐬����Json�t�@�C���̓��e{jsonStr}\n�p�X{path}");

            writer.Write(jsonStr);
            writer.Flush();
            writer.Close();

            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
        }
    }

    public static class FileUtils
    {
        /// <summary>
        /// �������݉\�ȃf�B���N�g���̃p�X��Ԃ�
        /// �t�@�C���̕ۑ��͂��̃f�B���N�g���̒����ł͂Ȃ��A�T�u�f�B���N�g�����쐬���ĕۑ����鎖�𐄏����܂�
        /// </summary>
        /// <returns>�v���b�g�t�H�[�����Ƃ̏������݉\�ȃf�B���N�g���̃p�X</returns>
        public static string GetWritableDirectoryPath()
        {
            // Android�̏ꍇ�AApplication.persistentDataPath�ł͊O������ǂݏo����ꏊ�ɕۑ�����Ă��܂�����
            // �A�v�����A���C���X�g�[�����Ă��t�@�C�����c���Ă��܂�
            // �����ł̓A�v����p�̈�ɕۑ�����悤�ɂ���
            #if !UNITY_EDITOR && UNITY_ANDROID
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var getFilesDir = currentActivity.Call<AndroidJavaObject>("getFilesDir"))
            {
                return getFilesDir.Call<string>("getCanonicalPath");
            }
            #else
            return Application.persistentDataPath;
            #endif
        }
    }
}