using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "AiManageTurnState", menuName = "Gameplay/AI/States/AiManageTurnState", order = 1)]
public class AiOnManageTurn : State
{
    private AiFSM fSM = null;
    private Unit[] units;

    private bool isAttacking = false;
    private bool isMoving = false;
    private int index = 0;

    public override bool ExecuteState(FSM fsm)
    {
        if (!isInit)
        {
            fSM = fsm as AiFSM;
            isInit = OnStartState();
        }

        if (OnExecuteState() || ForceQuit)
        {
            fSM.ChangeState(AiFSM.AiStates.WAIT_FOR_TURN);
            return OnEndState();
        }

        return false;

    }

    protected override bool OnEndState()
    {
        isAttacking = false;
        isMoving = false;
        fSM = null;
        units = null;
        index = 0;

        GameManager.Instance.PasTurn();

        return true;
    }

    protected override bool OnExecuteState()
    {
        if (units.Length <= 0)
            return false;

        if (index >= units.Length)
        {
            return true;
        }

        if (ManageUnitTurn(units[index]))
        {
            ++index;
        }

        return false;
    }

    protected override bool OnStartState()
    {
        units = fSM.GetOwner().Units.ToArray();
        return true;
    }


    private bool ManageUnitTurn(Unit u)
    {
        UnitFSM fsm = u.fSM as UnitFSM;

        if (!isAttacking)
        {
            Unit enemy = null;
            if (CheckEnemyInRange(u, out enemy))
            {
                u.CurrentEnemy = enemy;
                fsm.ForceChangeState(UnitFSM.UnitStates.ATTACK);
                isAttacking = true;
            }           
        }
        else
        {
            if (u.HasAttacked)
            {
                isAttacking = false;
                return true;
            }
        }
        

        if (!isMoving)
        {
            GridNode[] randomPos = GameManager.Instance.GetNeighbours(u.GetCurrentNode(), Random.Range(0, 2)).ToArray();
            GridNode target = randomPos[Random.Range(0, randomPos.Length - 1)];

            if(target != u.GetCurrentNode())
            {
                GameManager.Instance.PathfinderCall(target, u);
                fsm.ForceChangeState(UnitFSM.UnitStates.MOVE);

                isMoving = true;
            }          
        }
        else
        {
            if (u.HasMoved)
            {
                isMoving = false;
                return true;
            }
        }

        return false;
    }

    private bool CheckEnemyInRange(Unit unit, out Unit enemy)
    {
        foreach(Unit u in GameManager.Instance.GetAllUnits())
        {
            if (!fSM.GetOwner().IsUnitFromPlayer(u) && unit.IsEnemyInRange(u))
            {
                enemy = u;
                return true;
            }
        }

        enemy = null;
        return false;
    }
}
