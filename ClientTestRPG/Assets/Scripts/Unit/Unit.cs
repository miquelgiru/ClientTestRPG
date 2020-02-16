using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    public PlayerHolder PlayerOwner;
    private GridNode currentNode = null;

    public int Steps = 0;
    public int MaxSteps = 4;

    public List<GridNode> CurrentPath;

    public void Init()
    {
        PlayerOwner.RegisterUnitToPlayer(this);
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

}
