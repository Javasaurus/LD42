using UnityEngine;

[CreateAssetMenu]
public class Enemy : ScriptableObject
{

    //all their art stuff

    public int dmg = 1;

    [Range(0, 1)]
    public float dropChance;

}
