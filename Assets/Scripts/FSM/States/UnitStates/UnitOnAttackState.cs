using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "UnitAttackState", menuName = "Gameplay/Unit/States/UnitAttackState", order = 1)]
public class UnitOnAttackState : State
{
    Unit target;
    int counter = 0;
    private UnitFSM FSM;

    public override bool ExecuteState(FSM fsm)
    {
        FSM = fsm as UnitFSM;
        target = FSM.GetUnitOwner().CurrentEnemy;


        if (!isInit)
        {
            UnitFSM targetFSM = target.fSM as UnitFSM;
            targetFSM.ForceChangeState(UnitFSM.UnitStates.ATTACKED);

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
        FSM.ChangeState(UnitFSM.UnitStates.IDLE);

        //Attack management
        FSM.GetUnitOwner().HasAttacked = true;
        FSM.GetUnitOwner().CanAttack = true;
        target.DamageTaken += FSM.GetUnitOwner().Stats.DamageAttackPoints;
        target.HasBeenAttacked = true;

        isInit = false;
        ForceQuit = false;
        counter = 0;
        return true;
    }

    protected override bool OnExecuteState()
    {
        //TODO: Show attack feedback
        //NEXT STEPS: Wait for attack animation to finish
        //NEXT STEPS: counter should be the attack animation
        if (counter > 20)
        {
            return true;
        }

        ++counter;
        return false;
    }

    protected override bool OnStartState()
    {
        return true;
    }
}
