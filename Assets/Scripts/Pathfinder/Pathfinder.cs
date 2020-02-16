using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinder
{
    GridManager gridManager;
    Unit unit;
    GridNode startNode;
    GridNode endNode;

    public volatile bool isDone = false;
    public volatile float timer = 0;

    public List<GridNode> Path = new List<GridNode>();

    public delegate void PathfindFinished(List<GridNode> nodes, Unit unit);
    PathfindFinished OnPathfindFinished;

    public Pathfinder(Unit u, GridNode start, GridNode end, PathfindFinished callback)
    {
        unit = u;
        startNode = start;
        endNode = end;
        OnPathfindFinished = callback;
        gridManager = GameManager.Instance.gridManager;
    }

    public void FindPath()
    {
        Path = FindPathNow();
        unit.CurrentPath = Path;
        isDone = true;
    }

    public List<GridNode> FindPathNow()
    {
        List<GridNode> ret = new List<GridNode>();

        List<GridNode> openNodes = new List<GridNode>();
        List<GridNode> closedNodes = new List<GridNode>();

        openNodes.Add(startNode);

        while(openNodes.Count > 0)
        {
            GridNode current = openNodes[0];

            for(int i = 0; i < openNodes.Count; ++i)
            {
                //if (!current.Equals(openNodes[i]))
                {
                    if (openNodes[i].fCost < current.fCost ||
                    openNodes[i].fCost == current.fCost && openNodes[i].hCost < current.hCost)
                    {
                        current = openNodes[i];
                    }
                }
               
            }

            openNodes.Remove(current);
            closedNodes.Add(current);

            if (current.Equals(endNode)) 
            {
                ret = GetPath(startNode, current);
                break;
            }

            foreach(GridNode neightbour in GetNeightbours(current))
            {
                if (!closedNodes.Contains(neightbour))
                {
                    float cost = current.gCost + GetDistance(current, neightbour);
                    if(cost < neightbour.gCost || !openNodes.Contains(neightbour))
                    {
                        neightbour.gCost = cost;
                        neightbour.hCost = GetDistance(neightbour, endNode);
                        neightbour.ParentNode = current;

                        if (!openNodes.Contains(neightbour))
                        {
                            openNodes.Add(neightbour);
                        }
                    }
                }
            }
        }

        return ret;
    }

    List<GridNode> GetPath(GridNode start, GridNode end)
    {
        List<GridNode> ret = new List<GridNode>();
        GridNode current = endNode;

        while(current != start)
        {
            ret.Add(current);
            current = current.ParentNode;
        }

        ret.Add(unit.GetCurrentNode());

        ret.Reverse();
        return ret;
    }

    List<GridNode> GetNeightbours(GridNode node)
    {
        List<GridNode> ret = new List<GridNode>();

        for(int x = -1; x <= 1; ++x)
        {
            for (int z = -1; z <= 1; ++z)
            {
                if(x == 0 && z == 0)
                    continue;

                int _x = x + node.X;
                int _z = z + node.Z;

                GridNode n = GetNode(_x, _z);

                if (n != null)
                {
                    GridNode neightbour = GetNeightbour(n);

                    if (neightbour != null)
                    {
                        ret.Add(neightbour);
                    }
                }
               
            }

        }

        return ret;
    }

    GridNode GetNode( int x, int z)
    {
        return gridManager.GetNode(x, z);
    }

    GridNode GetNeightbour(GridNode node)
    {
        GridNode ret = null;

        if (node.IsWalkable)
        {
            ret = node;
        }
        return ret;
    }

    int GetDistance(GridNode A, GridNode B)
    {
        int x = Mathf.Abs(A.X - B.X);
        int z = Mathf.Abs(A.Z - B.Z);

        //if (x > z)
        //    return 14 * z + 10 * (x - z);

        //return 14 * x + 10 * (z - x);

        return x + z;

    }

    public void JobFinished()
    {
        Debug.Log("Path found");
        OnPathfindFinished?.Invoke(Path, unit);
    }
}
