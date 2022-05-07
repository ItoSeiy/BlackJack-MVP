using System;
using System.Collections.Generic;

namespace BlackJack.Extension
{
    /// <summary>
    /// IEnumerable<T>��List��ForEach�̂悤��ForEach�o����悤�ɂ���g�����\�b�h
    /// 
    /// IEnumerable<T>��ForEach�o����悤�ɂ������R
    /// 
    /// Linq��Enumerble�N���X�ŃN�G�����������List<T>.ForEach�����悤�Ƃ����ToList���Ȃ��Ƃ����Ȃ�
    /// (Enumerble�N���X�̃��\�b�h�߂�l��IEnmuerble�ł��邽��)
    /// 
    /// ������ToList�̏����͂��Ȃ�d���p�t�H�[�}���X��������
    /// 
    /// ���̂���IEnumerble�Ɋg�����\�b�h�����IEnumerble�̏�Ԃ�ForEach�ł���g�����\�b�h��p�ӂ���
    /// 
    /// �����ɒʏ�̔z���string�ł�ForEach���g�p�ł���悤�ɂȂ�V���v����foreach���ł���悤�ɂȂ�
    /// �l�X�g���󂭂Ȃ��ĉǐ����ǂ��Ȃ�
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