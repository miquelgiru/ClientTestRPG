using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "Gameplay/Unit/UnitStats", order = 1)]
public class UnitStats : ScriptableObject
{
    public int HealthPoints;
    public int DamageAttackPoints;
    public float AttackRange;
    public int MoveRange;
    public float MovementSpeed;
}
