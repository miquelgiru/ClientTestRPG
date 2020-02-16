using UnityEngine;
using System.Collections;

public class PlayerFSM : FSM
{
    public override void Init()
    {
        State interactions = new State();
        interactions.actions.Add(new DetectMousePosition());
        CurrentState = interactions;

        State wait = new State();

        allStates.Add("interactions", interactions);
        allStates.Add("wait", wait);
    }
}
