using UnityEngine.SceneManagement;

public class PlayingState : GameState
{
    public override void EnterState(GameStateManager state)
    {
        base.EnterState(state);
        SceneManager.LoadScene("Game Board");
    }
}