using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AiManageTurnState", menuName = "Gameplay/AI/States/AiManageTurnState", order = 1)]
public class AiOnManageTurn : State
{
    private AiFSM fSM = null;
    private Unit[] units;

    private bool isAttacking = false;
    private bool isMoving = false;
    private int index = 0;
    private bool isPathRequested = false;

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
        isPathRequested = false;

        GameManager.Instance.PassTurn();

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
            isPathRequested = false;
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

        if (!isAttacking && !isMoving)
        {
            if(u.CurrentPath != null)
                Move(u, fsm);

            if (!Attack(u,fsm))
            {
                if (!isPathRequested)
                    isPathRequested = RequestPathForUnit(u);
            }    
        }
        else
        {
            if (u.HasAttacked)
            {
                isAttacking = false;
                return true;
            }

            if (u.HasMoved && !isAttacking)
            {
                isMoving = false;
                u.CurrentPath = null;
                if(!Attack(u, fsm))
                {
                    return true;
                }
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

    private bool Attack(Unit u, UnitFSM fsm)
    {
       
        Unit enemy = null;
        if (CheckEnemyInRange(u, out enemy))
        {
            u.CurrentEnemy = enemy;
            fsm.ForceChangeState(UnitFSM.UnitStates.ATTACK);
            isAttacking = true;
            Debug.Log("Enemy attacked");
            return true;
        }

        return false;
    } 

    private bool Move(Unit u, UnitFSM fsm)
    {
        int steps = u.Stats.MoveRange;// Random.Range(1, u.Stats.MoveRange);

        if (steps == 0)
            return false;       
        u.CurrentPath = RecalculatePathToSteps(u.CurrentPath,u);
        fsm.ForceChangeState(UnitFSM.UnitStates.MOVE);
        isMoving = true;
        return true;
    }

    private bool RequestPathForUnit(Unit u)
    {
        Unit target = GameManager.Instance.GetClosestEnemy(u);

        if(target != null)
        {
            GameManager.Instance.PathfinderCall(target.GetCurrentNode(), u);
            return true;
        }

        return false;
    }

    private List<GridNode> RecalculatePathToSteps(List<GridNode> path, Unit u)
    {
        List<GridNode> ret = new List<GridNode>();
        int steps = u.Stats.MoveRange;
        GridNode lastPos = path[0];

        for(int i = 0; steps > 0; --steps)
        {
            if(lastPos.X != path[i].X && lastPos.Z != path[i].Z)
            {
                --steps;
            }

            ret.Add(path[i]);
            ++i;
        }

        return ret;
    }
}
