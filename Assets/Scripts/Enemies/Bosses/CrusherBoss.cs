using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CrusherBoss : MonoBehaviour
{

    Animator m_Animator;

    public int health = 10;
    public bool attacking;
    public bool inPosition;

    public float playerSensitivity;

    public Transform playerTransform;
    public Transform[] BossTargetPoints;

    public float AttackTimer;

    Coroutine movingRoutine;
    Coroutine retractRoutine;


    public float MinTimeBetweenAttacks = 3f;
    public float MaxTimeBetweenAttacks = 10f;
    public float MinTimeToRetract = 1f;
    public float MaxTimeToRetract = 5f;

    // Use this for initialization
    void Start()
    {
        playerTransform = GameObject.FindObjectOfType<PlayerAnimator>().transform;
        m_Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time > AttackTimer) & !attacking)
        {
            MoveToNextPoint();
        }

        if (!attacking && (inPosition || (Mathf.Abs(transform.position.y - playerTransform.position.y) <= playerSensitivity)))
        {
            if (movingRoutine != null)
            {
                StopCoroutine(movingRoutine);
            }
            DoAttack();
        }
    }

    void DoAttack()
    {
        m_Animator.SetTrigger("Attack");

    }

    void AsyncRetract()
    {
        retractRoutine = StartCoroutine(DoRetract());
    }

    IEnumerator DoRetract()
    {
        yield return new WaitForSeconds(health / 3f + Random.Range(MinTimeToRetract, MaxTimeToRetract));
        m_Animator.SetTrigger("Retract");
        retractRoutine = null;
    }

    public void ResetTimer()
    {
        attacking = false;
        AttackTimer = Time.time + (health / 3f + Random.Range(MinTimeBetweenAttacks, MaxTimeBetweenAttacks));
        MoveToNextPoint();
        retractRoutine = null;
    }


    //move between targets


    public void MoveToNextPoint()
    {
        if (BossTargetPoints.Length > 0 && retractRoutine == null)
        {
            inPosition = false;
            Vector3 newPosition = BossTargetPoints[Random.Range(0, BossTargetPoints.Length)].position;
            movingRoutine = StartCoroutine(MoveObject(new Vector3(transform.position.x, newPosition.y, transform.position.z)));
        }
    }

    public IEnumerator MoveObject(Vector3 newPosition)
    {
        // get player seat number
        Vector3 startPosition = transform.position;
        float timer = Time.time;

        while (transform.position != newPosition)
        {
            transform.position = Vector3.Lerp(startPosition, newPosition, Time.time - timer);
            yield return new WaitForSeconds(0.02f);
        }
        inPosition = true;
        yield return new WaitForSeconds(0.5f);

    }
}
