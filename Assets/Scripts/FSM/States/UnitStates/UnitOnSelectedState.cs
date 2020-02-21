using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "UnitSelectedState", menuName = "Gameplay/Unit/States/UnitSelectedState", order = 1)]
public class UnitOnSelectedState : State
{
    public override bool ExecuteState(FSM fsm)
    {
        if (!isInit)
        {
            ((UnitFSM)fsm).GetUnitOwner().GetComponent<Renderer>().material = ((UnitFSM)fsm).GetUnitOwner().SelectedMaterial;
            isInit = OnStartState();
        }

        if (OnExecuteState() || ForceQuit)
        {
            if (ForceQuit)
            {
                ((UnitFSM)fsm).ChangeState(((UnitFSM)fsm).ForcedState);
            }

            ((UnitFSM)fsm).GetUnitOwner().GetComponent<Renderer>().material = ((UnitFSM)fsm).GetUnitOwner().DefaultMaterial;
            return OnEndState();
        }

        return false;
    }

    protected override bool OnEndState()
    {
        isInit = false;
        ForceQuit = false;
        return true;
    }

    protected override bool OnExecuteState()
    {
        //NEXT STEPS: Unit syas some messages saying what to do
        return false;
    }

    protected override bool OnStartState()
    {
        return true;
    }
}
