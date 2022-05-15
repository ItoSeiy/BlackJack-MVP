# 規則

フォルダやクラス, 変数などの命名は必ず英語で行う

変数名は[キャメルケース](https://e-words.jp/w/%E3%82%AD%E3%83%A3%E3%83%A1%E3%83%AB%E3%82%B1%E3%83%BC%E3%82%B9.html) (先頭小文字)

メンバー変数の接頭辞には「＿」(アンダースコア)を付ける

関数名　クラス名　プロパティの名前は[パスカルケース](https://wa3.i-3-i.info/word13955.html) (先頭大文字)

### region 規則

```shell

public class <ANY NAME>:...
{
    #region Properties
        // プロパティを入れる。
    #region Inspector Variables
        // unity inpectorに表示したいものを記述。
    #region Member Variables
        // プライベートなメンバー変数。
    #region Constant
        // 定数をいれる。
    #region Events
        //  System.Action, System.Func などのデリゲートやコールバック関数をいれるところ。
    #region Unity Methods
        //  Start, UpdateなどのUnityのイベント関数。
    #region Enums
        // 列挙型を入れる。
    #region Public Methods
        //　自身で作成したPublicな関数を入れる。
    #region Private Methods
        // 自身で作成したPrivateな関数を入れる。
}
```

~~ブランチの名前は[スネークケース](https://e-words.jp/w/%E3%82%B9%E3%83%8D%E3%83%BC%E3%82%AF%E3%82%B1%E3%83%BC%E3%82%B9.html#:~:text=%E3%82%B9%E3%83%8D%E3%83%BC%E3%82%AF%E3%82%B1%E3%83%BC%E3%82%B9%E3%81%A8%E3%81%AF%E3%80%81%E3%83%97%E3%83%AD%E3%82%B0%E3%83%A9%E3%83%9F%E3%83%B3%E3%82%B0,%E3%81%AA%E8%A1%A8%E8%A8%98%E3%81%8C%E3%81%93%E3%82%8C%E3%81%AB%E5%BD%93%E3%81%9F%E3%82%8B%E3%80%82)
(すべて小文字単語間は「＿」(アンダースコア))
機能を作成するブランチであれば接頭辞に「feature/」を付けてる
機能の修正等は接頭辞に「fix/」を付ける~~

1人開発のためブランチは必要が無いと判断
実装に迷いがあったときにテスト用としての利用はあり

# 概要

ブラックジャック

[MVP](https://virtualcast.jp/blog/2019/11/mvp_pattern_on_unity/)を意識した設計

## 制作形式　

| エンジン | バージョン  |
| ---------- | ----------- |
| Unity      | 2020.3.20f1 |

Github,
Sourcetree,

上記の3点を用いて制作しました
