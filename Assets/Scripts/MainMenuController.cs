using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject StartButton;

    [SerializeField]
    private GameObject QuitButton;

    public void OnStartClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameboard");
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
