using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState
{
    protected GameStateManager manager;

    public virtual void EnterState(GameStateManager state)
    {
        manager = state;
    }
}
