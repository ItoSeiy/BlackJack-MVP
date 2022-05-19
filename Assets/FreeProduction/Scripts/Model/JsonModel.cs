using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

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
        public static T[] LoadFromJsons<T>(string path)
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
        public static T LoadFromJson<T>(string path)
        {
            var jsonStr = Resources.Load(path).ToString();

            Debug.Log($"�ǂݍ���Json�t�@�C���̓��e{jsonStr}");

            return JsonUtility.FromJson<T>(jsonStr);
        }

        /// <summary>
        /// Json�t�@�C�������֐�
        /// </summary>
        /// <param name="data">��肽���f�[�^</param>
        /// <param name="path">�ǂݍ��݂����f�[�^�̃p�X</param>
        public static void CreateJson<T>(T data, string path)
        {
            var writer = new StreamWriter(path);

            var jsonStr = JsonUtility.ToJson(data);

            Debug.Log($"�쐬����Json�t�@�C���̓��e\n{jsonStr}");

            writer.Write(jsonStr);
            writer.Flush();
            writer.Close();

            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
        }
    }
}