using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Turn", menuName = "Gameplay/Turn", order = 1)]
public class Turn : ScriptableObject
{

    private int phaseIndex = 0;

    public PlayerHolder Player;
    public Phase[] Phases;

   public bool Execute()
   {
        bool ret = false;

        Phases[phaseIndex].OnStartPhase(this);

        if (Phases[phaseIndex].IsFinished(this))
        {
            Phases[phaseIndex].OnEndPhase(this);
            ++phaseIndex;

            if(phaseIndex > Phases.Length - 1)
            {
                phaseIndex = 0;
                ret = true;
            }
        }

        return ret;
   }


    public void ForceEndPhase()
    {
        Phases[phaseIndex].ForceExit = true;
    }
}
