using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public PlayerHolder PlayerOwner;
    public UnitStats Stats;
    public Unit CurrentEnemy = null;

    public Slider Healthbar;
    //public Part

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
        InitHealthBar();
    }

    public GridNode GetCurrentNode()
    {
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

    public void RecalculateHealthBar()
    {
        if (HasBeenAttacked)
        {
            Healthbar.value = Healthbar.maxValue - DamageTaken;
        }
    }

    public bool Die()
    {
        GameManager.Instance.UnRegisterUnit(this);
        PlayerOwner.UnregisterUnitToPlayer(this);
        DestroyImmediate(gameObject);
        return true;
    }

    private void InitHealthBar()
    {
        if(Healthbar != null)
        {
            Healthbar.maxValue = Stats.HealthPoints;
            Healthbar.value = Healthbar.maxValue;
        }
    }

    public void ShowFeedbackOnAttacked()
    {
        StartCoroutine(BleedInteractionCoroutine());
    }

    private IEnumerator BleedInteractionCoroutine()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ps.Play();
        yield return new WaitForSeconds(0.5f);
        ps.Stop();
    }

    //private void Update()
    //{
    //    Healthbar.transform.LookAt(Camera.main.transform, -Vector3.up);
    //    Vector3 rot = Healthbar.transform.rotation.eulerAngles;
    //    Healthbar.transform.rotation = Quaternion.Euler(rot.x, 0, rot.z);
    //}
}
