using UnityEngine.SceneManagement;

public class ShopState : GameState
{
    public override void EnterState(GameStateManager state)
    {
        base.EnterState(state);
        SceneManager.LoadScene("Shop");
    }
}