using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UnitMoveState", menuName = "Gameplay/Unit/States/UnitMoveState", order = 1)]
public class UnitOnMoveState : State
{
    Unit unit;
    private int index = 0;
    private List<GridNode> path = null;


    public override bool ExecuteState(FSM fsm)
    {
        unit = ((UnitFSM)fsm).GetUnitOwner();

        if (!isInit)
        {
            isInit = OnStartState();
        }

        path = unit.CurrentPath;

        if(path != null)
        {
            if (OnExecuteState() || ForceQuit)
            {
                if (ForceQuit)
                {
                    ((UnitFSM)fsm).ChangeState(((UnitFSM)fsm).ForcedState);
                }

                return OnEndState();
            }
        }
        
        return false;
    }

    protected override bool OnEndState()
    {
        //NEXT STEPS: End walk/run animation
        index = 0;
        path = null;
        unit.HasMoved = true;
        return true;
    }

    protected override bool OnExecuteState()
    {
        //NEXT STEPS: Play walk/run animation

        Vector3 from = path[index].WorldPosition;
        Vector3 to = path[index + 1].WorldPosition;
        Vector3 move = Vector3.MoveTowards(unit.transform.position, to, Time.deltaTime * 2);
        unit.transform.position = move;

        float dis = Vector3.Distance(unit.transform.position, to);
        if (dis <= 0.05)
        {
            unit.transform.position = to;
            if (index + 2 == path.Count)
            {
                unit.SetCurrentNode(path[index + 1]);
                return true;
            }

            index++;

        }

        return false;
    }

    protected override bool OnStartState()
    {
        //NEXT STEPS: Start walk/run animation
        //NEXT STEPS: Play audio "GO GO GO"

        index = 0;
        path = null;

        return true;
    }
}
