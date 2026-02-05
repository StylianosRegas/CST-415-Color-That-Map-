using UnityEngine;
using System.Collections.Generic;

public class NodeRecognition : MonoBehaviour
{
    public ShapeColorChanger shapeScript;
    public BoxCollider2D collider1, collider2;
    public List<GameObject> collidedNodes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateCollision()
    {
        collider1.enabled = true;
        collider2.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Has collided");
        collidedNodes.Add(collision.gameObject);
    }
}
