using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerOnMoveState", menuName = "Gameplay/Player/States/PlayerOnMoveState", order = 1)]
public class PlayerOnMoveState : State
{
    private Unit currentUnit = null;
    private List<GridNode> path = null;

    private int index = 0;

    public override bool ExecuteState(PlayerFSM fsm)
    {
        if (!isInit)
        {
            isInit = OnStartState();
            currentUnit = fsm.GetOwner().CurrentSelectedUnit;
        }

        if (currentUnit != null)
            path = currentUnit.CurrentPath;

        if(path != null)
        {
            if (OnExecuteState() || ForceQuit)
            {
                fsm.ChangeState(PlayerFSM.PlayerStates.SELECT_UNIT);
                return OnEndState();
            }
        }
       
        return false;
    }

    protected override bool OnStartState()
    {
        index = 0;
        path = null;
        return true;
    }

    protected override bool OnExecuteState()
    {
        Vector3 from = path[index].WorldPosition;
        Vector3 to = path[index + 1].WorldPosition;
        Vector3 move = Vector3.MoveTowards(currentUnit.transform.position, to, Time.deltaTime * 2);
        currentUnit.transform.position = move;

        float dis = Vector3.Distance(currentUnit.transform.position, to);
        if (dis <= 0.05)
        {
            currentUnit.transform.position = to;
            if (index + 2 == path.Count)
            {
                currentUnit.SetCurrentNode(path[index + 1]);
                Debug.Log("Target reached");
                return true;
            }

            index++;

        }

        return false;
    }

    protected override bool OnEndState()
    {
        isInit = false;
        currentUnit.HasMoved = true;
        currentUnit = null;
        path = null;
        ForceQuit = false;

        return true;
    }
}
