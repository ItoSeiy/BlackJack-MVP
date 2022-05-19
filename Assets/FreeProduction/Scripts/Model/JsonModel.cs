using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BlackJack.Model
{
    public static class JsonModel
    {
        /// <summary>
        /// ResoucesファイルからJsonファイル"複数"を読み込むジェネリック関数
        /// </summary>
        /// <typeparam name="T">読み込みたいクラス</typeparam>
        /// <param name="path">読み込みたいデータのパス</param>
        /// <returns>リスト型のデータ</returns>
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
        /// ResoucesファイルからJsonファイルを読み込む関数
        /// </summary>
        /// <typeparam name="T">読み込みたいクラス</typeparam>
        /// <param name="path">読み込みたいデータのパス</param>
        /// <returns>読み込んだデータ</returns>
        public static T LoadFromJson<T>(string path)
        {
            var jsonStr = Resources.Load(path).ToString();

            Debug.Log($"読み込んだJsonファイルの内容{jsonStr}");

            return JsonUtility.FromJson<T>(jsonStr);
        }

        /// <summary>
        /// Jsonファイルを作る関数
        /// </summary>
        /// <param name="data">作りたいデータ</param>
        /// <param name="path">読み込みたいデータのパス</param>
        public static void CreateJson<T>(T data, string path)
        {
            var writer = new StreamWriter(path);

            var jsonStr = JsonUtility.ToJson(data);

            Debug.Log($"作成したJsonファイルの内容\n{jsonStr}");

            writer.Write(jsonStr);
            writer.Flush();
            writer.Close();

            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
        }
    }
}