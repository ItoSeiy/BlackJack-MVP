using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sample : MonoBehaviour
{

    Sprite[] _cardSprites;

    void Start()
    {
        LoadSprite();

        for(int i = 0; i < _cardSprites.Length; i++)
        {
            print($"Index{i} –¼‘O‚Í{_cardSprites[i].name}");
        }
    }

    private void LoadSprite()
    {
        _cardSprites = Resources.LoadAll<Sprite>("Images/Cards");
    }
}
