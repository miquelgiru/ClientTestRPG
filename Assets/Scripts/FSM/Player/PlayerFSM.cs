using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct StateNode
{
    public PlayerFSM.PlayerStates StateType;
    public State state;
}

public class PlayerFSM : FSM
{
    public enum PlayerStates { SELECT_UNIT, MOVE_UNIT, ATTACK_UNIT, IDLE }
    public PlayerStates CurrentState;

    public List<StateNode> StateNodes;
    private Dictionary<PlayerStates, State> states = new Dictionary<PlayerStates, State>();


    public override void Init()
    {
       // CurrentState = PlayerStates.IDLE;
        RegisterStates();
    }

    public  void ChangeState(PlayerStates newState)
    {
        if(CurrentState != newState)
        {
            Debug.Log("State changed to: " + newState.ToString());
            CurrentState = newState;
            states[CurrentState].isInit = false;
        }
    }

    private void RegisterStates()
    {
        foreach(StateNode n in StateNodes)
        {
            states.Add(n.StateType, n.state);
        }
    }

    private void Update()
    {
        states[CurrentState].ExecuteState(this);
    }

    public override void SetOwner(PlayerHolder owner)
    {
        Owner = owner;
    }

    public override PlayerHolder GetOwner()
    {
        return Owner;
    }
}
