using UnityEngine;
using System.Collections;

public abstract class StateActions : MonoBehaviour
{
    public abstract void Execute(FSM fsm, Turn turn);
   
}
