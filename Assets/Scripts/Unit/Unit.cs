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

    public List<GridNode> CurrentPath;

    public bool HasAttacked = false;
    public bool HasMoved = false;

    public void Start()
    {
        GameManager.Instance.RegisterUnit(this);
        PlayerOwner.RegisterUnitToPlayer(this);

        fSMObject = Instantiate(fSMObjectPrefab) as GameObject;
        fSM = fSMObject.GetComponent<FSM>();
        ((UnitFSM)fSM).SetOwner(this);
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
        currentNode = node;
    }

    public bool IsEnemyInRange(Unit enemy)
    {
        return (Vector3.Distance(currentNode.WorldPosition, enemy.currentNode.WorldPosition) <= Stats.AttackRange);
    }

    public void RecalculateHealth()
    {
        if(Stats.HealthPoints <= DamageTaken)
        {
            ((UnitFSM)fSM).ChangeState(UnitFSM.UnitStates.DIE);
        }
    }

    public bool Die()
    {
        GameManager.Instance.UnRegisterUnit(this);
        PlayerOwner.UnregisterUnitToPlayer(this);
        DestroyImmediate(gameObject);
        return true;
    }
}
