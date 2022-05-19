using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BlackJack.Manager;

namespace BlackJack.Editor
{
    [CustomEditor(typeof(CreditDataManager))]
    public class CreditManagerEditor : UnityEditor.Editor
    {
        int _credit;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var manager = target as CreditDataManager;

            GUILayout.Space(5f);

                EditorGUILayout.HelpBox("下の入力ボックスに値をいれてボタンを押すと所持金を調整できる", MessageType.Info);

            GUILayout.Space(5f);

            _credit = EditorGUILayout.IntField("所持金", _credit);

            GUILayout.Space(5f);

            if (GUILayout.Button("設定した所持金を保存する"))
            {
                manager.UpdateCreditData(new Data.CreditData(_credit));
                manager.CreateCreditData();
            }
        }
    }
}