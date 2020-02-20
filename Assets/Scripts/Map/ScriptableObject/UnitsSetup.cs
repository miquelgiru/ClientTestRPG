using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitsSetup", menuName = "MapConfig/UnitsSetup", order = 1)]
public class UnitsSetup : ScriptableObject
{
    [System.Serializable]
    public class UnitSetup
    {
        public GameObject UnitPrefab;
        public Vector3 InitialPos;
    }

    //NEXT STEPS: this should be an array of arrays to handle more than two players
    public UnitSetup[] Player1Setup;
    public UnitSetup[] AISetup;

}
