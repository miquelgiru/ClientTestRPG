using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
    public int X, Y, Z;
    public Vector3 WorldPosition;
    public bool IsWalkable = true;
    public GridElement Element;
    public GridNode ParentNode;
    public Renderer TileRenderer;
    public int Steps = 0;
    public float hCost;
    public float gCost;
    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public GridNode(int x, int y, int z, Vector3 worldPos, GridElement element, bool isWalkable = true)
    {
        X = x;
        Y = y;
        Z = z;
        WorldPosition = worldPos;
        IsWalkable = isWalkable;
        Element = element;
    }
}
