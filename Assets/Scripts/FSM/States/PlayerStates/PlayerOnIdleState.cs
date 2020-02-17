using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PlayerIdleState", menuName = "Gameplay/Player/States/PlayerIdleState", order = 1)]
public class PlayerOnIdleState : State
{
    private PlayerHolder player;
    private PlayerFSM.PlayerStates nextState;

    public override bool ExecuteState(PlayerFSM fsm)
    {
        if (!isInit)
        {
            player = fsm.GetOwner();
            isInit = OnStartState();
        }

        if(OnExecuteState() || ForceQuit)
        {
            fsm.ChangeState(nextState);
            return OnEndState();
        }

        return false;
    }

    protected override bool OnStartState()
    {
        isInit = true;
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

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Unit"))
                {
                    Unit unit = hit.collider.GetComponent<Unit>();
                    if (player.IsUnitFromPlayer(unit))
                    {
                        player.SetSelectedUnit(unit);
                        Debug.Log("Unit Selecxted");
                        nextState = PlayerFSM.PlayerStates.SELECT_UNIT;
                        return true;
                    }
                }
            }
        }
        
        return false;
    }
    protected override bool OnEndState()
    {
        isInit = false;
        player = null;
        ForceQuit = false;

        return true;
    }
}
