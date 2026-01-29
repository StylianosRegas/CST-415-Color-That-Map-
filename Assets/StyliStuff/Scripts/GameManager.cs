using NUnit.Framework;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Color[] colors;
    public int colorChoice;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

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
