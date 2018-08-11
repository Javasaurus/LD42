using UnityEngine;

[CreateAssetMenu]
public class Enemy : ScriptableObject
{

    //all their art stuff

    public CombatType combatType;

    public int dmg = 1;
    public int health;
    public float fireRate;

    [Range(0, 1)]
    public float dropChance;

}

public enum CombatType
{
    SimpleMelee,
    CircleMelee,
    OneDirectionRanged,
    ThreeDirectionRanged
}