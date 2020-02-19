using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct StateNodeUnit
{
    public UnitFSM.UnitStates StateType;
    public State state;
}

public class UnitFSM : FSM
{
    private Unit OwnerUnit;

    public enum UnitStates { SELECTED, MOVE, ATTACK, ATTACKED, IDLE, DIE, NONE }
    public UnitStates CurrentState;
    public UnitStates ForcedState = UnitStates.NONE;

    public List<StateNodeUnit> StateNodes;
    private Dictionary<UnitStates, State> states = new Dictionary<UnitStates, State>();

    public override void Init()
    {
        CurrentState = UnitStates.IDLE;
        RegisterStates();
    }


    private void RegisterStates()
    {
        foreach (StateNodeUnit n in StateNodes)
        {
            State state = Instantiate(n.state);
            states.Add(n.StateType, state);
        }
    }

    public void ChangeState(UnitStates newState)
    {
        if (CurrentState != newState && newState != UnitStates.NONE)
        {
            CurrentState = newState;
            states[CurrentState].isInit = false;
            ForcedState = UnitStates.NONE;
        }
    }
    public void ForceChangeState(UnitStates newState)
    {
        ForcedState = newState;
        states[CurrentState].ForceQuit = true;
    }


    private void Update()
    {
        states[CurrentState].ExecuteState(this);
    }

    public void SetUnitOwner(Unit owner)
    {
        OwnerUnit = owner;
    }

    public Unit GetUnitOwner()
    {
        return OwnerUnit;
    }
}
