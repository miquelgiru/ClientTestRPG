using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct AiStateNode
{
    public AiFSM.AiStates StateType;
    public State state;
}

public class AiFSM : FSM
{

    public enum AiStates { MANAGE_TURN, WAIT_FOR_TURN }
    public AiStates CurrentState;

    public List<AiStateNode> StateNodes;
    private Dictionary<AiStates, State> states = new Dictionary<AiStates, State>();

    public override void Init()
    {
        RegisterStates();
    }

    public void ChangeState(AiStates newState)
    {
        if (CurrentState != newState)
        {
            CurrentState = newState;
            states[CurrentState].isInit = false;
        }
    }

    private void RegisterStates()
    {
        foreach (AiStateNode n in StateNodes)
        {
            State state = Instantiate(n.state);
            states.Add(n.StateType, state);
        }
    }
    private void Update()
    {
        states[CurrentState].ExecuteState(this);
    }

}
