using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Turn", menuName = "Gameplay/PlayerHolder", order = 1)]
public class PlayerHolder : ScriptableObject
{
    [System.NonSerialized] public FSM fSM;
    GameObject fSMObject;
    [SerializeField] GameObject fSMObjectPrefab;

    [System.NonSerialized] public List<Unit> Units = new List<Unit>();
    [System.NonSerialized] public Unit CurrentSelectedUnit = null;

    public void Init()
    {
        fSMObject = Instantiate(fSMObjectPrefab) as GameObject;
        fSM = fSMObject.GetComponent<FSM>();
        fSM.SetOwner(this);
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

    public bool IsUnitFromPlayer(Unit unit)
    {
        return Units.Contains(unit);
    }

    public void SetSelectedUnit(Unit unit)
    {
        CurrentSelectedUnit = unit;
    }
}
