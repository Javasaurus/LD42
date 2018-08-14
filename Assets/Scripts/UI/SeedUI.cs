using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedUI : MonoBehaviour
{

    public TMPro.TextMeshProUGUI seedField;

    private void Start()
    {
        int newSeed = Random.Range(100000000, 999999999);
        LevelGenerator.SEED = newSeed;
        seedField.text = newSeed.ToString();
    }

    public void CheckValue()
    {
        int seed = LevelGenerator.SEED;
        bool isNumeric = int.TryParse(seedField.text, out seed);
        CheckIntValue(seed);
    }

    public void CheckIntValue(int input)
    {
        LevelGenerator.SEED = input;
    }

}
