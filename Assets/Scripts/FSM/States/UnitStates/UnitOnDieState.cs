using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "UnitDieState", menuName = "Gameplay/Unit/States/UnitDieState", order = 1)]
public class UnitOnDieState : State
{
    private UnitFSM FSM;

    public override bool ExecuteState(FSM fsm)
    {
        FSM = fsm as UnitFSM;

        if (!isInit)
        {
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
        ForceQuit = false;
        isInit = false;
        FSM.GetUnitOwner().Die();
        FSM = null;
        return true;
    }

    protected override bool OnExecuteState()
    {
        //NEXT STEPS: Play die animation and return true when finished
        return true;
    }

    protected override bool OnStartState()
    {
        return true;
    }
}
