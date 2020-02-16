using UnityEngine;
using System.Collections;


[CreateAssetMenu(fileName = "TickPhase", menuName = "Gameplay/Phases/TickPhase", order = 1)]
public class TickPhase : Phase
{
    public override bool IsFinished(Turn turn)
    {
    //    turn.Player.fSM.Tick(turn);
        return false;
    }

    public override void OnEndPhase(Turn turn)
    {

    }

    public override void OnStartPhase(Turn turn)
    {

    }
}
