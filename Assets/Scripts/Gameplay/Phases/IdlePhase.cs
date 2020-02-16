using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "IdlePhase", menuName = "Gameplay/Phases/IdlePhase", order = 1)]
public class IdlePhase : Phase
{
    public override bool IsFinished(Turn turn)
    {
        return false;
    }

    public override void OnStartPhase(Turn turn)
    {
        if (isInit)
            return;
        isInit = true;
    }

    public override void OnEndPhase(Turn turn)
    {
    }
}
