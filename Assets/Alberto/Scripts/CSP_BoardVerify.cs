using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CSPSolver : MonoBehaviour
{
    public GenerateBoard boardGenerator;
    public Dictionary<int, ShapeColorChanger.colorValue> boardState;

    private void Start()
    {
        boardGenerator.BoardGenerated += VerifyBoard;
    }

    private void VerifyBoard()
    {
        if (IsBoardSolvable())
        {
            Debug.Log("Board is solvable!");
            LogBoardState();
        }
        else
        {
            Debug.Log("Board is NOT solvable! Regenerating...");
            boardGenerator.GenerateNewBoard();
        }
    }

    public bool IsBoardSolvable()
    {
        // Initialize board state from tiles
        boardState = new Dictionary<int, ShapeColorChanger.colorValue>();
        for (int i = 0; i < boardGenerator.tiles.Count; i++)
        {
            boardState[i] = boardGenerator.tiles[i].GetColorValue();
        }

        // Check for immediate conflicts
        if (!ValidateCurrentState())
        {
            return false;
        }

        // Create a proper copy for backtracking
        Dictionary<int, ShapeColorChanger.colorValue> workingState =
            new Dictionary<int, ShapeColorChanger.colorValue>(boardState);

        // Solve using CSP with backtracking
        if (Backtrack(workingState))
        {
            // Update boardState with the solution
            boardState = workingState;
            return true;
        }

        return false;
    }

    private bool ValidateCurrentState()
    {
        foreach (int index in boardState.Keys)
        {
            if (boardState[index] != ShapeColorChanger.colorValue.white)
            {
                if (HasColorConflict(boardState, index))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool Backtrack(Dictionary<int, ShapeColorChanger.colorValue> state)
    {
        int unassignedIndex = SelectUnassignedVariableMRV(state);

        // Base case: all variables assigned
        if (unassignedIndex == -1)
        {
            return true;
        }

        // Try colors in optimal order
        List<ShapeColorChanger.colorValue> orderedColors = OrderDomainValuesLCV(state, unassignedIndex);

        foreach (ShapeColorChanger.colorValue color in orderedColors)
        {
            if (IsConsistent(state, unassignedIndex, color))
            {
                // Assign color
                state[unassignedIndex] = color;

                // Recurse
                if (Backtrack(state))
                {
                    return true;
                }

                // Backtrack
                state[unassignedIndex] = ShapeColorChanger.colorValue.white;
            }
        }

        return false;
    }

    private int SelectUnassignedVariableMRV(Dictionary<int, ShapeColorChanger.colorValue> state)
    {
        int selectedIndex = -1;
        int minRemainingValues = int.MaxValue;

        for (int i = 0; i < state.Count; i++)
        {
            if (state[i] != ShapeColorChanger.colorValue.white)
            {
                continue;
            }

            int validColorCount = CountValidColors(state, i);

            // Early exit for variables with no valid colors
            if (validColorCount == 0)
            {
                return i;
            }

            if (validColorCount < minRemainingValues)
            {
                minRemainingValues = validColorCount;
                selectedIndex = i;
            }
        }

        return selectedIndex;
    }

    private int CountValidColors(Dictionary<int, ShapeColorChanger.colorValue> state, int index)
    {
        ShapeColorChanger.colorValue[] colors = {
            ShapeColorChanger.colorValue.red,
            ShapeColorChanger.colorValue.green,
            ShapeColorChanger.colorValue.blue
        };

        return colors.Count(color => IsConsistent(state, index, color));
    }

    private bool IsConsistent(Dictionary<int, ShapeColorChanger.colorValue> state, int index,
        ShapeColorChanger.colorValue color)
    {
        List<int> neighbors = GetNeighborIndices(index);

        foreach (int neighborIndex in neighbors)
        {
            ShapeColorChanger.colorValue neighborColor = state[neighborIndex];

            if (neighborColor != ShapeColorChanger.colorValue.white && neighborColor == color)
            {
                return false;
            }
        }

        return true;
    }

    private List<ShapeColorChanger.colorValue> OrderDomainValuesLCV(
        Dictionary<int, ShapeColorChanger.colorValue> state, int index)
    {
        ShapeColorChanger.colorValue[] colors = {
            ShapeColorChanger.colorValue.red,
            ShapeColorChanger.colorValue.green,
            ShapeColorChanger.colorValue.blue
        };

        var colorConstraints = colors
            .Where(color => IsConsistent(state, index, color))
            .Select(color => new { Color = color, Constraints = CountConstraints(state, index, color) })
            .OrderBy(x => x.Constraints)
            .Select(x => x.Color)
            .ToList();

        return colorConstraints;
    }

    private int CountConstraints(Dictionary<int, ShapeColorChanger.colorValue> state, int index,
        ShapeColorChanger.colorValue color)
    {
        List<int> neighbors = GetNeighborIndices(index);
        int count = 0;

        foreach (int neighborIndex in neighbors)
        {
            // Only count unassigned neighbors
            if (state[neighborIndex] != ShapeColorChanger.colorValue.white)
            {
                continue;
            }

            // Count if this color would remove an option from the neighbor
            if (IsConsistent(state, neighborIndex, color))
            {
                count++;
            }
        }

        return count;
    }

    private bool HasColorConflict(Dictionary<int, ShapeColorChanger.colorValue> state, int index)
    {
        List<int> neighbors = GetNeighborIndices(index);

        foreach (int neighbor in neighbors)
        {
            if (state[neighbor] == state[index])
            {
                return true;
            }
        }

        return false;
    }

    public List<int> GetNeighborIndices(int index)
    {
        List<int> neighbors = new List<int>();
        int width = boardGenerator.width;
        int height = boardGenerator.height;

        int x = index % width;
        int y = index / width;

        if (x > 0) neighbors.Add(index - 1);       // Left
        if (x < width - 1) neighbors.Add(index + 1);       // Right
        if (y > 0) neighbors.Add(index - width);   // Up
        if (y < height - 1) neighbors.Add(index + width);   // Down

        return neighbors;
    }

    private void LogBoardState()
    {
        for (int i = 0; i < boardGenerator.tiles.Count; i++)
        {
            Debug.Log($"Tile {i}: {boardState[i]}");
        }
    }

    public void CSPView()
    {
        for (int i = 0; i < boardGenerator.tiles.Count; i++)
        {
            Color incomingColor;

            if(boardState[i] == ShapeColorChanger.colorValue.red)
            {
                incomingColor = Color.red;
            }
            else if (boardState[i] == ShapeColorChanger.colorValue.green)
            {
                incomingColor = Color.green;
            }
            else if (boardState[i] == ShapeColorChanger.colorValue.blue)
            {
                incomingColor = Color.blue;
            }
            else
            {
                incomingColor = Color.white;
            }

            boardGenerator.tiles[i].ChangeColor(incomingColor, boardState[i]);
            ShapeColorChanger.colorValue color = boardState[i];

        }
    }
}