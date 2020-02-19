using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "UnitAttackedState", menuName = "Gameplay/Unit/States/UnitAttackedState", order = 1)]
public class UnitOnAttackedState : State
{
    UnitFSM FSM;

    public override bool ExecuteState(FSM fsm)
    {
        if (!isInit)
        {
            FSM = fsm as UnitFSM;
            isInit = OnStartState();
        }

        if (OnExecuteState() || ForceQuit)
        {
            if (ForceQuit)
            {
                ((UnitFSM)fsm).ChangeState(((UnitFSM)fsm).ForcedState);
            }

            return OnEndState();
        }

        return false;
    }

    protected override bool OnEndState()
    {
        FSM.GetUnitOwner().RecalculateHealth();
        isInit = false;
        ForceQuit = false;
        FSM.GetUnitOwner().RecalculateHealth();
        return true;
    }

    protected override bool OnExecuteState()
    {
        //TODO: feedback on attack
        //NEXT STEPS: Play animation on being attacked

        if (FSM.GetUnitOwner().HasBeenAttacked)
            return true;

        return false;
    }

    protected override bool OnStartState()
    {
        return true;
    }
}
