using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PlayerOnSelectState", menuName = "Gameplay/Player/States/PlayerOnSelectState", order = 1)]
public class PlayerOnSelectState : State
{
    private Unit currentUnit;
    private PlayerHolder player;
    private PlayerFSM.PlayerStates nextState;

    public override bool ExecuteState(PlayerFSM fsm)
    {
        if (!isInit)
        {
            player = fsm.GetOwner();
            currentUnit = player.CurrentSelectedUnit;
            isInit = OnStartState();
        }

        if(OnExecuteState() || ForceQuit)
        {
            fsm.ChangeState(nextState);
            OnEndState();
        }

        return false;
    }

    protected override bool OnStartState()
    {
        GameManager.Instance.HighlightUnitMovementOptions(currentUnit);
        return true;
    }

    protected override bool OnExecuteState()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Floor"))
                {
                    GridNode target = GameManager.Instance.gridManager.GetNodeFromWorldPosition(hit.transform.position);
                    if (target != null)
                    {
                        if (GameManager.Instance.IsNodeReachable(target))
                        {
                            Debug.Log("Is reachable");
                            nextState = PlayerFSM.PlayerStates.MOVE_UNIT;
                            GameManager.Instance.PathfinderCall(target, currentUnit);
                        }

                        else
                        {
                            Debug.Log("Unselect unit");
                            nextState = PlayerFSM.PlayerStates.IDLE;
                            player.CurrentSelectedUnit = null;
                        }

                        return true;
                    }
                }
            }
        }
        return false;
    }

    protected override bool OnEndState()
    {
        GameManager.Instance.ClearReachableNodes();
        return true;
    }
}
