using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class State : ScriptableObject
{

    [System.NonSerialized] public bool ForceQuit = false;
    [System.NonSerialized] public bool isInit = false;

    public abstract bool ExecuteState(FSM fsm);
    protected abstract bool OnStartState();
    protected abstract bool OnExecuteState();
    protected abstract bool OnEndState();

}
