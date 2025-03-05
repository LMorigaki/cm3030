using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameState
{
    protected GameStateManager manager;

    public virtual void EnterState(GameStateManager state)
    {
        manager = state;
    }
}

public class MainMenuState : GameState
{
    public override void EnterState(GameStateManager state)
    {
        base.EnterState(state);
        SceneManager.LoadScene("Main Menu");
    }
}

public class PlayingState : GameState
{
    public override void EnterState(GameStateManager state)
    {
        base.EnterState(state);
        SceneManager.LoadScene("Game Board");
    }
}

public class ShopState : GameState
{
    public override void EnterState(GameStateManager state)
    {
        base.EnterState(state);
        SceneManager.LoadScene("Shop");
    }
}

public class GameOverState : GameState
{
    public override void EnterState(GameStateManager state)
    {
        base.EnterState(state);
        Debug.Log("Gameover triggered");
    }
}