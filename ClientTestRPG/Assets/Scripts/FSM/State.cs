using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class State
{
    public List<StateActions> actions = new List<StateActions>();

    public void Tick(FSM stateManager, Turn turn)
    {
        if (stateManager.forceExit)
            return;

        for (int i = 0; i < actions.Count; ++i)
        {
            actions[i].Execute(stateManager, turn);
        }
    }
   
}
