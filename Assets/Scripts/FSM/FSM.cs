using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class FSM : MonoBehaviour
{
    public bool forceExit = false;
    protected PlayerHolder Owner;

    private void Start()
    {
        Init();
    }

    public abstract void Init();

    public abstract void SetOwner(PlayerHolder owner);
    public abstract PlayerHolder GetOwner();

}
