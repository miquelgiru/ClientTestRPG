using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour
{
    public bool IsWalkable = true;


    public void Init(float obstaclePercentage)
    {
        GenerateObstacle(obstaclePercentage);
    }

    private void GenerateObstacle(float obstaclePercentage)
    {
        float rand = Random.value;

        IsWalkable = rand > (obstaclePercentage / 100) ? true : false;

        if(!IsWalkable)
        {
            CreateCubeCollider();
        }
    }

    private void CreateCubeCollider()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.SetParent(transform);
        cube.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        cube.transform.localScale = new Vector3(10, 20, 10);
    }
}
