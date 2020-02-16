using UnityEngine;
using System.Collections;

public class MoveUnit : StateActions
{
    bool isInit = false;
    float time;
    float speed;

    GridNode start;
    GridNode end;

    public override void Execute(FSM fsm, Turn turn)
    {
        if (!isInit)
        {
            isInit = true;
            start = fsm.CurrentNode;
            end = 
        }
    }
}
