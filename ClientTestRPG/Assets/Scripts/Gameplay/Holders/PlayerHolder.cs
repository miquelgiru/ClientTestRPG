using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Turn", menuName = "Gameplay/PlayerHolder", order = 1)]
public class PlayerHolder : ScriptableObject
{
    [System.NonSerialized] public FSM fSM;
    GameObject fSMObject;
    [SerializeField] GameObject fSMObjectPrefab;

    public List<Unit> Units = new List<Unit>();


    public void Init()
    {
        fSMObject = Instantiate(fSMObjectPrefab) as GameObject;
        fSM = fSMObject.GetComponent<FSM>();
    }

    public void RegisterUnitToPlayer(Unit u)
    {
        if(!Units.Contains(u))
            Units.Add(u);
    }

    public void UnregisterUnitToPlayer(Unit u)
    {
        if (Units.Contains(u))
            Units.Remove(u);
    }
}
