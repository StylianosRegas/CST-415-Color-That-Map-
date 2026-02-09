using UnityEngine;
using System;

public class ColorSelect : MonoBehaviour
{
    public SpriteRenderer sr;
    public GameManager gm;
    public int storedColor;

    void Update()
    {
        if (gm.colorChoice != storedColor)
        {
            sr.color = Color.black;
        }
        else
        {
            sr.color = Color.gold;
        }
    }

    void OnMouseDown()
    {
        gm.colorChoice = storedColor;
    }
}
