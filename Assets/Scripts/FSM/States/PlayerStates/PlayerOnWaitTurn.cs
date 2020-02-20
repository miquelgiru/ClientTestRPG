using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "WaitForTurnState", menuName = "Gameplay/Player/States/WaitForTurnState", order = 1)]
public class PlayerOnWaitTurn : State
{
    public override bool ExecuteState(FSM fsm)
    {
        if (!isInit)
        {
            foreach (Unit u in fsm.GetOwner().Units)
            {
                var temp = u.fSM as UnitFSM;
                temp.ForceChangeState(UnitFSM.UnitStates.IDLE);
                u.HasAttacked = false;
                u.HasBeenAttacked = false;
                u.HasMoved = false;
            }

            isInit = OnStartState();
        }

        if (OnExecuteState() || ForceQuit)
        {

            return OnEndState();
        }

        return false;
    }

    protected override bool OnEndState()
    {
        return true;
    }

    protected override bool OnExecuteState()
    {


        return false;
    }

    protected override bool OnStartState()
    {
        return true;
    }
}
