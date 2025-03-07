using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void OnStartClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
