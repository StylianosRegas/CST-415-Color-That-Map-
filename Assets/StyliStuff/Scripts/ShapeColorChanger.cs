using System.Collections.Generic;
using UnityEngine;

public class ShapeColorChanger : MonoBehaviour
{
    public enum colorValue { white, red, green, blue }

    [SerializeField] private colorValue cValue = colorValue.white;
    public int position = 0;

    private SpriteRenderer sprite;
    private GameManager gm;
    public CSPSolver solver;
    private bool isLocked = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        solver = GameObject.Find("GameManager").GetComponent<CSPSolver>();

        RandomizeLockedTile();
    }

    private void OnMouseDown()
    {
        if (isLocked)
        {
            return;
        }

        colorValue newColor = IntToColorValue(gm.colorChoice);

        if (IsValidColorChoice(newColor))
        {
            ChangeColor(gm.colors[gm.colorChoice], newColor);
        }
        else
        {
            Debug.Log($"Invalid move! Color {newColor} conflicts with a neighbor at position {position}.");
        }
    }

    private bool IsValidColorChoice(colorValue newColor)
    {
        // White is always valid (erasing)
        if (newColor == colorValue.white)
        {
            return true;
        }

        List<int> neighbors = solver.GetNeighborIndices(position);

        foreach (int neighborIndex in neighbors)
        {
            // Get the CURRENT color of the neighbor tile, not from solver's solution
            ShapeColorChanger neighborTile = solver.boardGenerator.tiles[neighborIndex];
            colorValue neighborColor = neighborTile.GetColorValue();

            // If neighbor has the same color, it's invalid
            if (neighborColor == newColor)
            {
                return false;
            }
        }

        return true;
    }

    public void ChangeColor(Color tileColor, colorValue newColorValue)
    {
        // Update tile count in game manager
        bool wasEmpty = (cValue == colorValue.white);
        bool willBeEmpty = (newColorValue == colorValue.white);

        if (wasEmpty && !willBeEmpty)
        {
            gm.Decrement(); // Placed a color on empty tile
        }
        else if (!wasEmpty && willBeEmpty)
        {
            gm.Increment(); // Removed a color from filled tile
        }

        // Update visual and state
        cValue = newColorValue;
        sprite.color = tileColor;

        if (solver.boardState != null)
        {
            solver.boardState[position] = cValue;
        }

        // Darken locked tiles
        if (isLocked)
        {
            DarkenSprite();
        }
    }

    private void DarkenSprite()
    {
        Color current = sprite.color;
        sprite.color = new Color(
            Mathf.Max(0, current.r - 0.25f),
            Mathf.Max(0, current.g - 0.25f),
            Mathf.Max(0, current.b - 0.25f),
            1f
        );
    }

    private void RandomizeLockedTile()
    {
        int rand = Random.Range(1, 100);

        // 25% chance to be a locked tile with a random color
        if (rand >= 75)
        {
            isLocked = true;
            int colorIndex = Random.Range(1, gm.colors.Length);
            colorValue lockedColor = IntToColorValue(colorIndex);
            ChangeColor(gm.colors[colorIndex], lockedColor);
        }
    }

    private colorValue IntToColorValue(int value)
    {
        switch (value)
        {
            case 1: return colorValue.red;
            case 2: return colorValue.green;
            case 3: return colorValue.blue;
            default: return colorValue.white;
        }
    }

    public colorValue GetColorValue()
    {
        return cValue;
    }

    public void SetPosition(int index)
    {
        position = index;
    }

    public bool IsLocked()
    {
        return isLocked;
    }
}