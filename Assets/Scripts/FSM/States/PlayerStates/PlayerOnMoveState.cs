using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerOnMoveState", menuName = "Gameplay/Player/States/PlayerOnMoveState", order = 1)]
public class PlayerOnMoveState : State
{
    private Unit currentUnit = null;
    private int index = 0;

    public override bool ExecuteState(FSM fsm)
    {
        if (!isInit)
        {
            currentUnit = ((PlayerFSM)fsm).GetOwner().CurrentSelectedUnit;
            isInit = OnStartState();
        }

        if (OnExecuteState() || ForceQuit)
        {
            ((PlayerFSM)fsm).ChangeState(PlayerFSM.PlayerStates.SELECT_UNIT);
            ((UnitFSM)currentUnit.fSM).ForceChangeState(UnitFSM.UnitStates.IDLE);
            return OnEndState();
        }
           
        return false;
    }

    protected override bool OnStartState()
    {
        UnitFSM fSM = currentUnit.fSM as UnitFSM;
        fSM.ForceChangeState(UnitFSM.UnitStates.MOVE);
        return true;
    }

    protected override bool OnExecuteState()
    {
        if (currentUnit.HasMoved)
            return true;

        return false;
    }

    protected override bool OnEndState()
    {
        isInit = false;
        currentUnit = null;
        ForceQuit = false;
        index = 0;

        return true;
    }
}
