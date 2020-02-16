using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridMapSettings", menuName = "MapConfig/GridConfig", order = 1)]
public class GridMapSettings : ScriptableObject
{
    [SerializeField] public int SizeX;
    [SerializeField] public int SizeY;
    [SerializeField] public int SizeZ;
    [SerializeField] public float ObstaclePercentage;

}
