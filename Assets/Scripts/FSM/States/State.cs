using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class State : ScriptableObject
{
    public bool ForceQuit = false;
    public bool isInit = false;

    public abstract bool ExecuteState(PlayerFSM fsm);
    protected abstract bool OnStartState();
    protected abstract bool OnExecuteState();
    protected abstract bool OnEndState();

}
