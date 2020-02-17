using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class FSM : MonoBehaviour
{
    public bool forceExit = false;

    private void Start()
    {
        Init();
    }

    public abstract void Init();
}
