using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    public PlayerHolder PlayerOwner;
    public UnitStats Stats;
    public Unit CurrentEnemy = null;
    private GridNode currentNode = null;

    [Header("State Machine")]
    [System.NonSerialized] public FSM fSM;
    GameObject fSMObject;
    [SerializeField] GameObject fSMObjectPrefab;


    public int Steps = 0;
    public float DamageTaken = 0;
    public bool HasBeenAttacked = false;
    public bool CanAttack = false;

    public List<GridNode> CurrentPath;

    public bool HasAttacked = false;
    public bool HasMoved = false;

    public void Start()
    {
        GameManager.Instance.RegisterUnit(this);
        PlayerOwner.RegisterUnitToPlayer(this);

        fSMObject = Instantiate(fSMObjectPrefab, transform) as GameObject;
        fSM = fSMObject.GetComponent<FSM>();
        ((UnitFSM)fSM).SetUnitOwner(this);
    }

    public GridNode GetCurrentNode()
    {
        if(currentNode == null)
            currentNode = GameManager.Instance.gridManager.GetNode(0, 0);

        return currentNode;
    }

    public void SetCurrentPath(List<GridNode> path)
    {
        CurrentPath = path;
    }

    public void SetCurrentNode(GridNode node)
    {
        if (currentNode != null)
            currentNode.Occupied = false;

        currentNode = node;
        currentNode.Occupied = true;
    }

    public bool IsEnemyInRange(Unit enemy)
    {
        if (PlayerOwner == enemy.PlayerOwner)
            return false;
        Debug.Log("Range: " + Vector3.Distance(currentNode.WorldPosition, enemy.currentNode.WorldPosition));
        return (Vector3.Distance(currentNode.WorldPosition, enemy.currentNode.WorldPosition) < Stats.AttackRange);
    }

    public void RecalculateHealth()
    {
        if(Stats.HealthPoints <= DamageTaken)
        {
            ((UnitFSM)fSM).ChangeState(UnitFSM.UnitStates.DIE);
        }

        RecalculateHealthBar();
    }

    private void RecalculateHealthBar()
    {

    }

    public bool Die()
    {
        GameManager.Instance.UnRegisterUnit(this);
        PlayerOwner.UnregisterUnitToPlayer(this);
        DestroyImmediate(gameObject);
        return true;
    }
}
