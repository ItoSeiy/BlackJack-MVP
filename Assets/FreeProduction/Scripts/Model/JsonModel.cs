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
        /// ResoucesファイルからJsonファイル"複数"を読み込むジェネリック関数
        /// </summary>
        /// <typeparam name="T">読み込みたいクラス</typeparam>
        /// <param name="path">読み込みたいデータのパス</param>
        /// <returns>リスト型のデータ</returns>
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
        /// ResoucesファイルからJsonファイルを読み込む関数
        /// </summary>
        /// <typeparam name="T">読み込みたいクラス</typeparam>
        /// <param name="path">読み込みたいデータのパス</param>
        /// <returns>読み込んだデータ</returns>
        public static T LoadJsonFromResources<T>(string path)
        {
            var jsonStr = Resources.Load(path).ToString();

            Debug.Log($"リソースから読み込んだJsonファイルの内容{jsonStr}");

            return JsonUtility.FromJson<T>(jsonStr);
        }

        /// <summary>
        /// 指定されたパスからJsonファイルを読み込む関数
        /// </summary>
        /// <typeparam name="T">読み込みたいクラス</typeparam>
        /// <param name="path">読み込みたいデータのパス</param>
        /// <returns>読み込んだデータ</returns>
        public static T LoadJson<T>(string path)
        {
            using var reader = new StreamReader(path, System.Text.Encoding.GetEncoding("UTF-8"));

            var jsonStr = reader.ReadToEnd();
            reader.Close();

            Debug.Log($"通常ファイル読み込んだJsonファイルの内容{jsonStr}");

            return JsonUtility.FromJson<T>(jsonStr);
        }

        /// <summary>
        /// Jsonファイルを作る関数
        /// </summary>
        /// <param name="data">作りたいデータ</param>
        /// <param name="path">読み込みたいデータのパス</param>
        public static void CreateJson<T>(T data, string path)
        {
            using var writer = new StreamWriter(path);

            var jsonStr = JsonUtility.ToJson(data);

            Debug.Log($"作成したJsonファイルの内容{jsonStr}\nパス{path}");

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
        /// 書き込み可能なディレクトリのパスを返す
        /// ファイルの保存はこのディレクトリの直下ではなく、サブディレクトリを作成して保存する事を推奨します
        /// </summary>
        /// <returns>プラットフォームごとの書き込み可能なディレクトリのパス</returns>
        public static string GetWritableDirectoryPath()
        {
            // Androidの場合、Application.persistentDataPathでは外部から読み出せる場所に保存されてしまうため
            // アプリをアンインストールしてもファイルが残ってしまう
            // ここではアプリ専用領域に保存するようにする
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