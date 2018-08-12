using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRandomizer : MonoBehaviour
{

    public Sprite[] availableSprites;


    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < (int)(transform.localScale.y); i++)
        {
            GameObject sprite = new GameObject();
            sprite.transform.SetParent(this.transform);
            sprite.transform.localPosition = new Vector3(0, i, -0.5f);
            sprite.AddComponent<SpriteRenderer>().sprite = availableSprites[Random.Range(0, availableSprites.Length - 1)];
        }
    }

}
