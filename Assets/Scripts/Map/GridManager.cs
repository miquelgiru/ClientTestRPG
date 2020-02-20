using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private GridNode[,] grid;
    private List<GridNode> nodeVizualizer = new List<GridNode>();
    [SerializeField] private float xScaleFactor = 0;
    [SerializeField] private float zScaleFactor = 0;

    [Header("3D map generation")]
    [SerializeField] private List<GridMapSettings> mapConfigurations;
    [SerializeField] private  Transform tilesParent = null;
    [SerializeField] private  Transform tilePrefab = null;
    private GridMapSettings selectedMapConfig = null;

    [Header("Debug Guizmos")]
    public Vector3 DebugCubeSize = new Vector3(.8f, .8f, .8f);

    public void Init()
    {
        selectedMapConfig = mapConfigurations[0];
        CreateGrid(selectedMapConfig.SizeX, selectedMapConfig.SizeZ, selectedMapConfig.ObstaclePercentage);
    }

    private void CreateGrid(int sizeX, int sizeZ, float percentageOfObstacles)
    {
        grid = new GridNode[sizeX, sizeZ];

        for(int i = 0; i < sizeX; ++i)
        {
            for (int j = 0; j < sizeZ; ++j)
            {
                //Create grid model
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, 0, j), Quaternion.identity, tilesParent).gameObject;
                tile.GetComponent<GridElement>().Init(percentageOfObstacles);

                //Create logic grid
                Vector3 tmp = new Vector3(i * xScaleFactor, 0, j * zScaleFactor);
                GridNode node = new GridNode(i, 0, j, tmp, null);


                //Check if is walkable
                Collider[] collisionNode = Physics.OverlapBox(tmp, DebugCubeSize / 2, Quaternion.identity);

                if(collisionNode.Length > 0)
                {
                    bool isWalkable = false;

                    foreach(Collider col in collisionNode)
                    {
                        GridElement elem = col.GetComponent<GridElement>();

                        if(elem != null)
                        {
                            if(elem.IsWalkable && node.Element == null)
                            {
                                isWalkable = true;
                            }

                            else
                            {
                                isWalkable = false;
                                node.Element = elem;
                            }
                        }
                    }

                    node.IsWalkable = isWalkable;
                    node.TileRenderer = tile.GetComponent<Renderer>();
                }

                grid[i, j] = node;
                nodeVizualizer.Add(node);
            }
        }
    }

    public GridNode GetNode(int x, int z)
    {
        if( x >= 0 && x < selectedMapConfig.SizeX && z >= 0 && z < selectedMapConfig.SizeZ)
            return grid[x, z];
        return null;
    }

    private void OnDrawGizmos()
    {
           
        foreach(GridNode pos in nodeVizualizer)
        {
            if (pos.IsWalkable)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;          

            Gizmos.DrawWireCube(pos.WorldPosition, DebugCubeSize);
        }
    }

    public GridNode GetNodeFromWorldPosition(Vector3 pos)
    {
        foreach(GridNode n in nodeVizualizer)
        {
            if (n.WorldPosition.x == pos.x && n.WorldPosition.z == pos.z)
                return n;
        }

        return null;
    }

    public bool PlaceUnitInitial(Unit unit, Vector3 pos)
    {
        GridNode node = GetNodeFromWorldPosition(pos);

        if(node != null)
        {
            unit.transform.position = new Vector3(node.X, 0, node.Z);
            unit.SetCurrentNode(node);
            return true;
        }

        return false;
    }
}
