using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PlayerOnAttack", menuName = "Gameplay/Player/States/PlayerOnAttack", order = 1)]
public class PlayerOnAttack : State
{
    private Unit currentUnit;
    private Unit enemyCurrentUnit;

    public override bool ExecuteState(PlayerFSM fsm)
    {
        throw new System.NotImplementedException();
    }

    protected override bool OnEndState()
    {
        throw new System.NotImplementedException();
    }

    protected override bool OnExecuteState()
    {
        throw new System.NotImplementedException();
    }

    protected override bool OnStartState()
    {
        throw new System.NotImplementedException();
    }
}
