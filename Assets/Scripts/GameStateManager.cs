using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    #region Variables
    private GameState currentState;
    public GameState CurrentState { get { return currentState; } }

    [Header("States")]
    public MainMenuState s_MainMenu = new MainMenuState();
    public PlayingState s_Playing = new PlayingState();
    public ShopState s_Shop = new ShopState();
    public GameOverState s_GameOver = new GameOverState();
    #endregion


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Start()
    {
    }

    public void MoveToState(GameState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void OnStartGame()
    {
        MoveToState(s_Playing);
    }

    public void OnRoundEnd()
    {
        MoveToState(s_Shop);
    }

    public void OnNextRound()
    {
        MoveToState(s_Playing);
    }
}
