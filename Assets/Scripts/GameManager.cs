using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public LineRenderer pathDebugDraw;
    public Material ReachableMaterial;
    public Material ReachableEnemyMaterial;
    public Material DefaultMaterial;

    [Header("Turn Management")]
    public List<Turn> Turns;
    private int turnIndex = 0;
    private Turn currentTurn = null;


    private void Awake()
    {
        instance = this;
        gridManager.Init();
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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P)){

            if (Turns.Count - 1 == turnIndex)
                turnIndex = 0;

            ChangeTurn(Turns[++turnIndex]);
        }
    }

    private void InitFSMs()
    {
        foreach(Turn t in Turns)
        {
            t.Player.Init();
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
                    FinishGame(Turns[0]);
                    break;
                }
            }
        }
    }

    private void FinishGame(Turn winner)
    {
        Debug.Log(winner.Player.GetPlayerName() + "Has won the game!!");
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

    #region Turn Management
    public void ChangeTurn(Turn turn)
    {
        if(turn != currentTurn)
        {
            if(currentTurn != null)
                currentTurn.EndTurn();
            currentTurn = turn;
            currentTurn.StartTurn();
        }
    }

    public void PasTurn()
    {
        int index = Turns.IndexOf(currentTurn) + 1;

        if (index >= Turns.Count)
            index = 0;

        Turn next = Turns[index];
        ChangeTurn(next);

        Debug.Log("Turn: " + index);
    }
    #endregion

    #region Pathfinder
    public void PathfinderCall(GridNode target, Unit u)
    {
        if (target.Occupied)
            return;

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

        pathDebugDraw.positionCount = path.Count;
        List<Vector3> positions = new List<Vector3>();
        Vector3 offset = Vector3.up * .1f;
        positions.Add(unit.GetCurrentNode().WorldPosition + offset);

        for (int i = 0; i < path.Count; i++)
        {
            positions.Add(path[i].WorldPosition + Vector3.up * .1f);
        }

        unit.SetCurrentPath(path);
        pathDebugDraw.SetPositions(positions.ToArray());
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

    #endregion
}
