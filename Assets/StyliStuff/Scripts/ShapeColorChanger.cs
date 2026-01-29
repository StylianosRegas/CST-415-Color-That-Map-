using NUnit.Framework;
using UnityEngine;

public class ShapeColorChanger : MonoBehaviour
{
    public Color color = Color.white;
    private SpriteRenderer sprite;
    public GameManager gm;
    public GameObject[] touchingObjects;
    private int numObjects;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        
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
        sprite.color = c;
        color = c;
        
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        touchingObjects[numObjects] = obj;
        numObjects++;
        Debug.Log("Object added");
    }
}
