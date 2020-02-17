using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "UnitIdleState", menuName = "Gameplay/Unit/States/UnitIdleState", order = 1)]
public class UnitOnIdleState : State
{
    public override bool ExecuteState(FSM fsm)
    {
        if (!isInit)
        {
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
        //NEXT STEPS: Idle animation should end here

        isInit = false;
        ForceQuit = false;
        return true;
    }

    protected override bool OnExecuteState()
    {
        return false;
    }

    protected override bool OnStartState()
    {
        //NEXT STEPS: Idle animation should start here
        return true;
    }
}
