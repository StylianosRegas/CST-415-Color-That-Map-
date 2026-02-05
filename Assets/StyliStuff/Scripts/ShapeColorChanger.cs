using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShapeColorChanger : MonoBehaviour
{
    private SpriteRenderer sprite;
    private GameManager gm;

    public enum colorValue { white, red, blue, yellow, green };
    public Color color = Color.white;
    [SerializeField] colorValue cValue = colorValue.white;


    public Collider2D[] colliders;
    public GameManager gm;
    public List<GameObject> touchingObjects;
    private GameObject parent;
    private int numObjects = 0;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = transform.parent.gameObject;
        sprite = GetComponent<SpriteRenderer>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        RandomColor();
        colliders = GetComponents<Collider2D>();
        touchingObjects = new List<GameObject>();





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
        bool canChange = true;
        for (int i = 0; i < numObjects; i++)
        {
            ShapeColorChanger scc = touchingObjects[i].GetComponent<ShapeColorChanger>();
            if (scc.color == c)
            {
                canChange = false;
                break;
            }
        }

        if (canChange)
        {
            sprite.color = c;
            color = c;
        }

        else
        {
            Debug.Log("color can't change");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        GameObject ob = collision.gameObject;
        if (ob.name == "SquareFill")
        {
            touchingObjects.Add(ob);
            numObjects++;
        }
    }

    void DisableColliders()
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }

    void EnableColliders()
    {
        for (int i = 0;i < colliders.Length; i++)
        {
            colliders[i].enabled=true;
        }
    }
    
}
