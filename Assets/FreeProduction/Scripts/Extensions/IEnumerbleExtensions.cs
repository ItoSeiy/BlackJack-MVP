using System;
using System.Collections.Generic;

namespace BlackJack.Extension
{
    /// <summary>
    /// IEnumerable<T>をListのForEachのようにForEach出来るようにする拡張メソッド
    /// 注意点: 構造体を使用した際にメソッドの中身で書き換えを行うと参照を渡せていないので書き換えられないことです
    ///
    /// IEnumerable<T>をForEach出来るようにした理由
    /// 
    /// LinqのEnumerbleクラスでクエリをした後にList<T>.ForEachをしようとするとToListしないといけない
    /// (Enumerbleクラスのメソッド戻り値はIEnmuerbleであるため)
    /// 
    /// しかしToListの処理はかなり重くパフォーマンスが下がる
    /// 
    /// そのためIEnumerbleに拡張メソッドを作りIEnumerbleの状態でForEachできる拡張メソッドを用意した
    /// 
    /// 同時に通常の配列やstringでもForEachを使用できるようになりシンプルにforeachをできるようになり
    /// ネストが浅くなって可読性が良くなる
    /// </summary>
    public static class IEnumerbleExtensions
    {
        public static IEnumerable<T> ForEachExt<T>(this IEnumerable<T> sourceT, Action<T> action)
        {
            foreach (var st in sourceT)
            {
                action(st);
            }

            return sourceT;
        }
    }
}
