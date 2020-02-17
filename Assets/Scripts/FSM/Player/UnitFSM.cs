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
    private Unit Owner;

    public enum UnitStates { SELECTED, MOVE, ATTACK, ATTACKED, IDLE, DIE, NONE }
    public UnitStates CurrentState;
    public UnitStates ForcedState;

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
            states.Add(n.StateType, n.state);
        }
    }

    public void ChangeState(UnitStates newState)
    {
        if (CurrentState != newState && newState != UnitStates.NONE)
        {
            CurrentState = newState;
            states[CurrentState].isInit = false;
            ForcedState = UnitStates.NONE;
            Debug.Log(newState.ToString());
        }
    }
    public void ForceChangeState(UnitStates newState)
    {
        states[CurrentState].ForceQuit = true;
        ForcedState = newState;
    }


    private void Update()
    {
        states[CurrentState].ExecuteState(this);
    }

    public void SetOwner(Unit owner)
    {
        Owner = owner;
    }

    public Unit GetOwner()
    {
        return Owner;
    }
}
