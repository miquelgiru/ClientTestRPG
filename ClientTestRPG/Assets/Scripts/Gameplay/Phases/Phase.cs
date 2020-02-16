using UnityEngine;
using System.Collections;

public abstract class Phase : ScriptableObject
{
    public string PhaseName;
    public bool ForceExit = false;
    protected bool isInit = false;

    public abstract bool IsFinished(Turn turn);

    public abstract void OnStartPhase(Turn turn);

    public abstract void OnEndPhase(Turn turn);
}
