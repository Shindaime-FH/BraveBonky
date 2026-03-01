using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Game is closing...");
        Application.Quit();
    }
}
