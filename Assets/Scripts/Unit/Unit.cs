using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    public PlayerHolder PlayerOwner;
    public UnitStats Stats;
    private GridNode currentNode = null;

    public int Steps = 0;

    public List<GridNode> CurrentPath;

    public bool HasMoved = false;

    public void Start()
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

    public void SetCurrentNode(GridNode node)
    {
        currentNode = node;
    }

    private void Update()
    {
        if(currentNode != null)
            Debug.Log(currentNode.WorldPosition.ToString());
    }
}
