using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PlayerOnAttack", menuName = "Gameplay/Player/States/PlayerOnAttack", order = 1)]
public class PlayerOnAttack : State
{
    private Unit currentUnit;
    private Unit enemyCurrentUnit;

    public override bool ExecuteState(PlayerFSM fsm)
    {
        if (!isInit)
        {
            currentUnit = fsm.GetOwner().CurrentSelectedUnit;
            enemyCurrentUnit = currentUnit.CurrentEnemy;
            isInit = OnStartState();
        }

        if(OnExecuteState() || ForceQuit)
        {
            Debug.Log("Enemy Attacked: " + enemyCurrentUnit.gameObject.name);
            fsm.GetOwner().CurrentSelectedUnit.CurrentEnemy = null;
            fsm.GetOwner().CurrentSelectedUnit.HasAttacked = true;
            fsm.GetOwner().CurrentSelectedUnit = null;

            fsm.ChangeState(PlayerFSM.PlayerStates.IDLE);

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
        //Attack
        return true;
    }

    protected override bool OnStartState()
    {
        return true;
    }
}
