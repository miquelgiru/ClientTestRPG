using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    #region Instance
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    #endregion

   
    private bool ispathfinding = false;
    private List<GridNode> highlightedNodes = new List<GridNode>();
    private List<Unit> allUnits = new List<Unit>();

    [Header("Gid Management")]
    public GridManager gridManager;
    public Material ReachableMaterial;
    public Material ReachableEnemyMaterial;
    public Material DefaultMaterial;

    [Header("Units Setup")]
    public UnitsSetup setup;


    [Header("Turn Management")]
    public Text NewTurnText;
    public List<Turn> Turns;
    private int turnIndex = 0;
    private Turn currentTurn = null;
    


    private void Awake()
    {
        instance = this;
        gridManager.Init();
        InitUnits();
    }

    private void Start()
    {      
        InitFSMs();

        if (Turns.Count > 0)
        {
            currentTurn = Turns[0];
            currentTurn.StartTurn();
        }
    }

    private void InitFSMs()
    {
        foreach(Turn t in Turns)
        {
            t.Player.Init();
        }
    }

    private void InitUnits()
    {
        foreach(UnitsSetup.UnitSetup u in setup.Player1Setup)
        {
            GameObject unit = Instantiate(u.UnitPrefab);
            SetupUnit(unit.GetComponent<Unit>(), u.InitialPos);
        }

        foreach (UnitsSetup.UnitSetup u in setup.AISetup)
        {
            GameObject unit = Instantiate(u.UnitPrefab);
            SetupUnit(unit.GetComponent<Unit>(), u.InitialPos);
        }
    }

    private void SetupUnit(Unit unit, Vector3 pos)
    {
        if(!gridManager.PlaceUnitInitial(unit, pos))
        {
            Debug.LogError("Unit could not be placed because the position setup is incorrect or the position is already taken: " + pos);
        }
    }

    public void OnPlayerEliminated(PlayerHolder player)
    {
        foreach(Turn t in Turns)
        {
            if(t.Player == player)
            {
                Turns.Remove(t);

                if(Turns.Count == 1)
                {
                    StartCoroutine(FinishGame(Turns[0]));
                    break;
                }
            }
        }
    }

    private IEnumerator FinishGame(Turn winner)
    {
        NewTurnText.text = winner.Player.GetPlayerName() + " Has won";
        NewTurnText.transform.parent.gameObject.SetActive(true);
        Debug.Log(winner.Player.GetPlayerName() + "Has won the game!!");

        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #region Unit Management
    public void RegisterUnit(Unit unit)
    {
        if (!allUnits.Contains(unit)) 
            allUnits.Add(unit);

        Vector3 pos = unit.transform.position;
        unit.SetCurrentNode(gridManager.GetNodeFromWorldPosition(pos));
    }

    public void UnRegisterUnit(Unit unit)
    {
        if (allUnits.Contains(unit))
            allUnits.Remove(unit);
    }
    #endregion

    #region Pathfinder
    public void PathfinderCall(GridNode target, Unit u)
    {
        //if (target.Occupied)
        //    return;

        if (!ispathfinding)
        {
            ispathfinding = true;
            PathfinderManager.Instance.RequestPathfind(u, u.GetCurrentNode(), target, PathfinderCallback);
        }
    }

    private void PathfinderCallback(List<GridNode> path, Unit unit)
    {
        ispathfinding = false;

        if(path == null)
        {
            return;
        }

        unit.SetCurrentPath(path);
    }

    #endregion

    #region Tile Management

    private void UptdateReachableNodes(List<GridNode> nodes)
    {
        ClearReachableNodes();

        foreach (GridNode n in nodes)
        {
            n.TileRenderer.material = ReachableMaterial;
        }

        highlightedNodes = nodes;
    }

    public void ClearReachableNodes()
    {
        foreach (GridNode n in highlightedNodes)
        {
            n.TileRenderer.material = DefaultMaterial;
        }

        highlightedNodes.Clear();
    }

    public void HighlightUnitMovementOptions(Unit u)
    {
        GridNode center = u.GetCurrentNode();

        List<GridNode> reachableNodes = new List<GridNode>();
        List<GridNode> openNodes = new List<GridNode>();
        List<GridNode> closedNodes = new List<GridNode>();
        List<GridNode> NodesToClean = new List<GridNode>();


        openNodes.Add(center);
        int steps = 0;

        while (openNodes.Count > 0)
        {
            GridNode current = openNodes[0];
            steps = current.Steps;
            current.Steps = 0;
           

            if (u.Steps <= u.Stats.MoveRange)
            {
                foreach(GridNode n in GetNeighbours(current, steps))
                {
                    NodesToClean.Add(n);
                    if (!n.Occupied)
                    {
                        if (!closedNodes.Contains(n))
                        {
                            int newStepCost = n.Steps;

                            if (newStepCost <= u.Stats.MoveRange)
                            {
                                if (!openNodes.Contains(n))
                                {
                                    openNodes.Add(n);
                                    u.Steps = newStepCost;
                                    reachableNodes.Add(n);
                                }
                            }
                        }
                    }
                }
            }

            openNodes.Remove(current);
            closedNodes.Add(current);
        }

        foreach(GridNode n in NodesToClean)
        {
            n.Steps = 0;
        }

        UptdateReachableNodes(reachableNodes);
    }

    public List<GridNode> GetNeighbours(GridNode center, int step)
    {
        List<GridNode> ret = new List<GridNode>();

        for (int x = -1; x <= 1; ++x)
        {            
            GridNode n = gridManager.GetNode(center.X + x, center.Z);

            if (n != null)
            {
                if (n.IsWalkable)
                {
                    n.Steps = step + 1;
                    ret.Add(n);
                }
            }
        }

        for (int z = -1; z <= 1; ++z)
        {
            GridNode n = gridManager.GetNode(center.X, center.Z + z);

            if (n != null)
            {
                if (n.IsWalkable)
                {
                    n.Steps = step + 1;
                    ret.Add(n);
                }
            }
        }

        return ret;
    }

    public bool IsNodeReachable(GridNode node)
    {
        return highlightedNodes.Contains(node);
    }

    public void HighlightReachableEnemyunits(Unit unit)
    {
        foreach (Unit u in allUnits)
        {
            if (u.PlayerOwner != unit.PlayerOwner)
            {
                if (unit.IsEnemyInRange(u))
                {
                    u.GetCurrentNode().TileRenderer.material = ReachableEnemyMaterial;
                    highlightedNodes.Add(u.GetCurrentNode());
                }
            }
        }
    }
    #endregion

    #region AI Management

    public List<Unit> GetAllUnits()
    {
        return allUnits;
    }

    public Unit GetClosestEnemy(Unit current)
    {
        Unit ret = null;
        float closestDist = float.MaxValue;
        foreach(Unit u in allUnits)
        {
            if(u.PlayerOwner != current.PlayerOwner)
            {
                float dis = Vector3.Distance(current.transform.position, u.transform.position);
                if (dis < closestDist)
                {
                    closestDist = dis;
                    ret = u;
                }
            }           
        }

        return ret;
    }

    #endregion

    #region Turn Management

    public void ChangeTurn(Turn turn)
    {
        if (turn != currentTurn)
        {
            if (currentTurn != null)
                currentTurn.EndTurn();
            currentTurn = turn;
            currentTurn.StartTurn();
        }
    }

    public void PassTurn()
    {
        StartCoroutine(PassTurnCoroutine());
    }


    private IEnumerator PassTurnCoroutine()
    {
        GameManager.Instance.ClearReachableNodes();

        int index = Turns.IndexOf(currentTurn) + 1;

        if (index >= Turns.Count)
            index = 0;

        NewTurnText.transform.parent.gameObject.SetActive(true);
        NewTurnText.text = Turns[index].Player.name + " Turn";
        yield return new WaitForSeconds(1.0f);
        NewTurnText.transform.parent.gameObject.SetActive(false);

        Turn next = Turns[index];
        ChangeTurn(next);

    }
    #endregion
}
