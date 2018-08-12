using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorCheck : MonoBehaviour
{
    public float RaycastOffset = 0.5f;
    public float RaycastLength = 0.5f;
    public LayerMask doorMask;
    private BoxCollider2D m_Collider;

    private void Start()
    {
        m_Collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 origin = new Vector3(transform.position.x, m_Collider.bounds.max.y + RaycastOffset, transform.position.z);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.up, RaycastLength, doorMask);
        Debug.DrawLine(origin, origin + Vector3.up * RaycastLength, Color.cyan);
        if (hit.collider != null && hit.collider.tag == "Door")
        {
            Debug.Log("Hit door");
            hit.collider.GetComponent<RoomTransition>().DoPlayerTransition(transform);
        }
    }
}
