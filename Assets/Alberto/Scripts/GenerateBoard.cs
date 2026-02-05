using UnityEngine;
using System;
using System.Collections.Generic;

public class GenerateBoard : MonoBehaviour
{
    public GameObject tilePrefab;
    public List<GameObject> tiles;
    public int width = 9;
    public int height = 9;

    public event Action BoardGenerated;

    void Start()
    {
        Generate();
        BoardGenerated?.Invoke();
        ActivateCollisions();
    }

    private void Generate()
    {
        float endX = -(0.95f * Mathf.Floor(width / 2));

        float currX = endX;
        float currY = (0.95f * Mathf.Floor(height / 2));

        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                tiles.Add(Instantiate(tilePrefab, new Vector3(currX, currY, 0), Quaternion.identity, transform));
                currX += 0.95f;
            }
            currX = endX;
            currY -= 0.95f;
        }
    }

    private void ActivateCollisions()
    {
        foreach(GameObject node in tiles)
        {
            node.GetComponent<NodeRecognition>().ActivateCollision();
        }
    }
}
