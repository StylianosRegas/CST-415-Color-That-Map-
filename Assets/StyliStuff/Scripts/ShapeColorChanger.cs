using NUnit.Framework;
using UnityEngine;

public class ShapeColorChanger : MonoBehaviour
{
    private SpriteRenderer sprite;
    private GameManager gm;

    public enum colorValue { white, red, blue, yellow, green };
    public Color color = Color.white;
    [SerializeField] colorValue cValue = colorValue.white;


    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        RandomColor();
    }

    void OnMouseDown()
    {
        Debug.Log("I HAVE BEEN CLICKED");
        changeColor(gm.colors[gm.colorChoice], gm.colorChoice);
        
    }

    void changeColor(Color tileColor, int value)
    {
        sprite.color = tileColor;
        color = tileColor;
        cValue = FindColor(value);
    }

    colorValue FindColor(int val)
    {
        switch (val)
        {
            case 1:
                return colorValue.red;
            case 2:
                return colorValue.blue;
            case 3:
                return colorValue.yellow;
            case 4:
                return colorValue.green;
            default:
                return colorValue.white;
        }
    }

    private void RandomColor()
    {
        int rand = Random.Range(1, 100);
        if(rand >= 90)
        {
            rand = Random.Range(1, gm.colors.Length);
            changeColor(gm.colors[rand], rand);
        }
    }

    public colorValue GetColorValue()
    {
        return cValue;
    }
}
