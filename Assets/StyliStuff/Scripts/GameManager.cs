using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Color[] colors;
    public int colorChoice;

    public int tilesLeft = 0;


    public void SetTilesLeft(int count)
    {
        tilesLeft = count;
    }

    public void Increment()
    {
        tilesLeft++;
    }

    public void Decrement()
    {
        tilesLeft--;
        if (tilesLeft == 0)
        {
            Debug.Log("You win!");
        }
    }
}
