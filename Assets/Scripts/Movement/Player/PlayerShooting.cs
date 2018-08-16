using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerShooting : MonoBehaviour
{
    PlayerAnimator m_Animator;


    public PlayerBullet bulletPrefab;
    public int maxBulletCount = 3;
    public float shotDelay;
    public Transform barrel;
    private float shotTimer;
    public static List<PlayerBullet> currentBullets;
    // Use this for initialization
    bool isShooting;

    void Start()
    {
        m_Animator = GetComponent<PlayerAnimator>();
        currentBullets = new List<PlayerBullet>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Animator.shooting = Input.GetButton(PC2D.Input.FIRE);
        if (Time.time > shotTimer && (m_Animator.shooting) && maxBulletCount > currentBullets.Count)
        {
            float direction = (m_Animator.walled ? -1 : 1) * Mathf.Sign(transform.localScale.x);
            Instantiate(bulletPrefab, barrel.position, Quaternion.identity).direction = new Vector3(direction, 0, 0);
            shotTimer = Time.time + shotDelay;
        }
    }
}
