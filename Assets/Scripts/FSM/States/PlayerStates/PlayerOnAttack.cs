using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PlayerOnAttack", menuName = "Gameplay/Player/States/PlayerOnAttack", order = 1)]
public class PlayerOnAttack : State
{
    private Unit currentUnit;
    private Unit enemyCurrentUnit;
    private PlayerFSM FSM;

    public override bool ExecuteState(FSM fsm)
    {
        if (!isInit)
        {
            FSM = fsm as PlayerFSM;
            currentUnit = FSM.GetOwner().CurrentSelectedUnit;
            enemyCurrentUnit = currentUnit.CurrentEnemy;
            isInit = OnStartState();
        }

        if(OnExecuteState() || ForceQuit)
        {
            FSM.GetOwner().CurrentSelectedUnit.CurrentEnemy = null;
            FSM.GetOwner().CurrentSelectedUnit = null;
            FSM.ChangeState(PlayerFSM.PlayerStates.IDLE);

            return OnEndState();
        }

        return false;
    }

    protected override bool OnEndState()
    {
        currentUnit = null;
        enemyCurrentUnit = null;
        ForceQuit = false;

        return true;
    }

    protected override bool OnExecuteState()
    {
        if(FSM.GetOwner().CurrentSelectedUnit.HasAttacked)
            return true;
        return false;
    }

    protected override bool OnStartState()
    {
        ((UnitFSM)currentUnit.fSM).ForceChangeState(UnitFSM.UnitStates.ATTACK);
        ((UnitFSM)enemyCurrentUnit.fSM).ForceChangeState(UnitFSM.UnitStates.ATTACKED); 

        return true;
    }
}
