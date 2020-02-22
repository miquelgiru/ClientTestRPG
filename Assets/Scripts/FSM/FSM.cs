using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class FSM : MonoBehaviour
{
    protected PlayerHolder Owner;

    private void Start()
    {
        Init();
    }

    public abstract void Init();

    public void SetOwner(PlayerHolder owner)
    {
        Owner = owner;
    }

    public PlayerHolder GetOwner()
    {
        return Owner;
    }
}
