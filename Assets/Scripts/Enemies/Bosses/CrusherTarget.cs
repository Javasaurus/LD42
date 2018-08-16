using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherTarget : MonoBehaviour
{
    public CrusherBoss crusherBoss;
    public Transform Reference;
    public Transform Visuals;
    public Vector3 previousPosition;

    // Use this for initialization
    void Start()
    {
        previousPosition = Reference.position;
    }

    // Update is called once per frame
    void Update()
    {
        //flip the movement and apply it to this object
        Vector3 movement = (Reference.position - previousPosition);
        movement.x *= -1;
        transform.position += movement + new Vector3(0, Mathf.Sign(movement.y) * Time.deltaTime * Mathf.Sin(Time.time), 0);
        previousPosition = Reference.position;
        //mimic the localscale, but invert the x
        Visuals.localScale = Reference.localScale;
    }

    public void OnDamage()
    {
        crusherBoss.health--;
        if (crusherBoss.health <= 0)
        {
            //TRIGGER A CUT SCENE ????
    //        LevelTrigger.currentRoom.GetComponent<RoomTransition>().DoPlayerTransition(GameObject.FindGameObjectWithTag("Player").transform);
        }
    }

}
