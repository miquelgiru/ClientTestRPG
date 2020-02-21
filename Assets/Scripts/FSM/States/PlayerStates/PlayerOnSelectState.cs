using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PlayerOnSelectState", menuName = "Gameplay/Player/States/PlayerOnSelectState", order = 1)]
public class PlayerOnSelectState : State
{
    private Unit currentUnit;
    private PlayerHolder player;
    private PlayerFSM.PlayerStates nextState;

    public override bool ExecuteState(FSM fsm)
    {
        if (!isInit)
        {
            player = ((PlayerFSM)fsm).GetOwner();
            currentUnit = player.CurrentSelectedUnit;
            isInit = OnStartState();
        }

        if(OnExecuteState() || ForceQuit)
        {
            ((PlayerFSM)fsm).ChangeState(nextState);
            return OnEndState();
        }

        return false;
    }

    protected override bool OnStartState()
    {
        ForceQuit = false;

        if(!currentUnit.HasMoved)
            GameManager.Instance.HighlightUnitMovementOptions(currentUnit);
        GameManager.Instance.HighlightReachableEnemyunits(currentUnit);

        ((UnitFSM)currentUnit.fSM).ForceChangeState(UnitFSM.UnitStates.SELECTED);

        return true;
    }

    protected override bool OnExecuteState()
    {
        if (Input.GetMouseButtonDown(0)) // TODO: Input manager should handle this
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
                            nextState = PlayerFSM.PlayerStates.MOVE_UNIT;
                            GameManager.Instance.PathfinderCall(target, currentUnit);
                        }

                        else
                        {
                            nextState = PlayerFSM.PlayerStates.IDLE;
                            player.SetSelectedUnit(null);
                        }

                        return true;
                    }
                }

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Unit"))
                {
                    Unit enemy = hit.collider.gameObject.GetComponent<Unit>();
                    if (!player.IsUnitFromPlayer(enemy))
                    {
                        if (currentUnit.IsEnemyInRange(enemy))
                        {
                            nextState = PlayerFSM.PlayerStates.ATTACK_UNIT;
                            currentUnit.CurrentEnemy = enemy;
                            return true;
                        }

                        else
                        {
                            currentUnit.CanAttack = false;

                            if (currentUnit.HasMoved)
                            {
                                ((UnitFSM)currentUnit.fSM).ForceChangeState(UnitFSM.UnitStates.IDLE);
                                nextState = PlayerFSM.PlayerStates.IDLE;
                                ForceQuit = true;
                            }
                        }
                    }

                    else
                    {
                        ((UnitFSM)player.CurrentSelectedUnit.fSM).ForceChangeState(UnitFSM.UnitStates.IDLE);
                        nextState = PlayerFSM.PlayerStates.IDLE;
                        player.SetSelectedUnit(null);

                        if (enemy.HasMoved)
                        {
                            player.SetSelectedUnit(enemy);
                            nextState = PlayerFSM.PlayerStates.SELECT_UNIT;
                        }
                        
                        ForceQuit = true;
                    }
                }
            }
        }
        return false;
    }

    protected override bool OnEndState()
    {
        GameManager.Instance.ClearReachableNodes();
        ForceQuit = false;
        isInit = false;
        return true;
    }
}
