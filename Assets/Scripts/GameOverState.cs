using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverState : GameState
{
    public override void EnterState(GameStateManager state)
    {
        base.EnterState(state);
        Debug.Log("Gameover triggered");
    }
}