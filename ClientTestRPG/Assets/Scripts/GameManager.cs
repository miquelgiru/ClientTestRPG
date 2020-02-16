﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Instance
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    #endregion

    private int turnIndex = 0;
    public Turn[] Turns;
    public GridManager gridManager;
    public LineRenderer pathDebugDraw;
    bool ispathfinding = false;

    public Material ReachableMaterial;
    public Material DefaultMaterial;
    private List<GridNode> highlightedNodes = new List<GridNode>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gridManager.Init();
        InitFSMs();
    }

    private void Update()
    {
        if (Turns.Length <= 0)
            return;

        if (Turns[turnIndex].Execute())
        {
            ++turnIndex;

            if(turnIndex > Turns.Length - 1)
            {
                turnIndex = 0;
            }
        }
    }

    private void InitFSMs()
    {
        foreach(Turn t in Turns)
        {
            t.Player.Init();
        }
    }

    #region Pathfinder
    public void PathfinderCall(GridNode target, Unit u)
    {
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
            Debug.Log("Path is not valid");
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

        Debug.Log("Pathfind found");
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

    private void ClearReachableNodes()
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

        openNodes.Add(center);
        int steps = 0;

        while (openNodes.Count > 0)
        {
            GridNode current = openNodes[0];
            u.Steps = steps;
            ++steps;
            if(u.Steps <= u.MaxSteps)
            {
                for (int x = -1; x <= 1; ++x)
                {
                    GridNode n = gridManager.GetNode(current.X + x, current.Z);

                    if (n != null)
                    {
                        if (n.IsWalkable)
                        {
                            if (!closedNodes.Contains(n))
                            {
                                openNodes.Add(n);
                                reachableNodes.Add(n);
                            }
                        }
                    }
                }

                for (int z = -1; z <= 1; ++z)
                {
                    GridNode n = gridManager.GetNode(current.X, current.Z + z);

                    if (n != null)
                    {
                        if (n.IsWalkable)
                        {
                            if (!closedNodes.Contains(n))
                            {
                                openNodes.Add(n);
                                reachableNodes.Add(n);
                            }
                        }
                    }
                }
             
            }

            openNodes.Remove(current);
            closedNodes.Add(current);
        }

        UptdateReachableNodes(reachableNodes);
    }
    #endregion
}