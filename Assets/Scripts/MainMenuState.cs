using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuState : GameState
{
    public override void EnterState(GameStateManager state)
    {
        base.EnterState(state);
        SceneManager.LoadScene("Main Menu");
    }
}
