using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class GenerateBoard : MonoBehaviour
{
    public GameManager gm;
    public GameObject tilePrefab;
    public List<ShapeColorChanger> tiles;
    public int width = 9;
    public int height = 9;

    public event Action BoardGenerated;

    void Start()
    {
        GenerateNewBoard();
    }

    public void GenerateNewBoard()
    {
        // Clear existing tiles if any
        foreach (ShapeColorChanger tile in tiles)
        {
            if (tile != null)
            {
                Destroy(tile.gameObject);
            }
        }
        tiles.Clear();

        Generate();
        StartCoroutine(WaitForSeconds());
    }

    private void Generate()
    {
        float endX = -(1 * Mathf.Floor(width / 2));

        float currX = endX;
        float currY = (1 * Mathf.Floor(height / 2));

        int currIndex = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                ShapeColorChanger newTile = Instantiate(tilePrefab, new Vector3(currX, currY, 0), Quaternion.identity, transform).GetComponentInChildren<ShapeColorChanger>();
                newTile.SetPosition(currIndex);
                tiles.Add(newTile);

                currX += 1;
                currIndex++;
            }
            currX = endX;
            currY -= 1;
        }

        gm.SetTilesLeft(tiles.Count);
    }

    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(0.75f);
        BoardGenerated?.Invoke();
    }
}