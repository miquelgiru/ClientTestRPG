using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class FSM : MonoBehaviour
{

    public State CurrentState;
    public bool forceExit = false;
    public GridNode CurrentNode = null;
    public GridNode PrevNode = null;
    protected Dictionary<string, State> allStates = new Dictionary<string, State>();

    private void Start()
    {
        Init();
    }

    public abstract void Init();

    public void Tick(Turn turn)
    {
        if(CurrentState != null)
        {
            CurrentState.Tick(this, turn);
        }

        forceExit = false;
    }
}
