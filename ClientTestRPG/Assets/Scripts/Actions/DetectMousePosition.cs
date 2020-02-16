using UnityEngine;
using System.Collections;

public class DetectMousePosition : StateActions
{
    public override void Execute(FSM stateManager, Turn turn)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000))
        {
            GridNode n = GameManager.Instance.gridManager.GetNode((int)hit.point.x, (int)hit.point.z);
            stateManager.CurrentNode = n;

            if (n != null)
            {
                if(n != stateManager.PrevNode)
                {
                    stateManager.PrevNode = stateManager.CurrentNode;
                    stateManager.CurrentNode = n;
                    GameManager.Instance.PathfinderCall(stateManager.CurrentNode, GameObject.Find("Soldier1").GetComponent<Unit>());
                    GameManager.Instance.HighlightUnitMovementOptions(GameObject.Find("Soldier1").GetComponent<Unit>());
                }
            }
            
        }
    }
}
