using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Color[] colors;
    public int colorChoice;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            colorChoice++;
            if (colorChoice == colors.Length)
            {
                colorChoice = 0;
            }
            Debug.Log(colors[colorChoice]);
        }
    }
}
