using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TileRandomizer : MonoBehaviour {

    public Sprite[] availableSprites;

    void OnEnable()
    {
        GetComponent<SpriteRenderer>().sprite = availableSprites[Random.Range(0, availableSprites.Length)];
    }
}
