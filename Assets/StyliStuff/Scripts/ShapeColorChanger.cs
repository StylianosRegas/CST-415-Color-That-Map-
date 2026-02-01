using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShapeColorChanger : MonoBehaviour
{
    public Color color = Color.white;
    private SpriteRenderer sprite;
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
        colliders = GetComponents<Collider2D>();
        touchingObjects = new List<GameObject>();





    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnMouseDown()
    {
        Debug.Log("I HAVE BEEN CLICKED");
        changeColor(gm.colors[gm.colorChoice]);
        
    }

    void changeColor(Color c)
    {
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
