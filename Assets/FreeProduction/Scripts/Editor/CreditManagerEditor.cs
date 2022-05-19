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

                EditorGUILayout.HelpBox("���̓��̓{�b�N�X�ɒl������ă{�^���������Ə������𒲐��ł���", MessageType.Info);

            GUILayout.Space(5f);

            _credit = EditorGUILayout.IntField("������", _credit);

            GUILayout.Space(5f);

            if (GUILayout.Button("�ݒ肵����������ۑ�����"))
            {
                manager.UpdateCreditData(new Data.CreditData(_credit));
                manager.CreateCreditData();
            }
        }
    }
}