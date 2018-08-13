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
        if ((Time.time > AttackTimer) & !attacking && movingRoutine == null)
        {
            MoveToNextPoint();
        }
        else if (retractRoutine != null)
        {
            //stop moving !
            if (movingRoutine != null)
            {
                StopCoroutine(movingRoutine);
            }
        }
        else if (!attacking)
        {
            if ((inPosition || (Mathf.Abs(transform.position.y - playerTransform.position.y) <= playerSensitivity)))
            {
                if (movingRoutine != null)
                {
                    StopCoroutine(movingRoutine);
                }
                DoAttack();
            }
        }

        if (!inPosition && !attacking && movingRoutine == null)
        {
            MoveToNextPoint();
        }

    }

    void DoAttack()
    {
        attacking = true;
        m_Animator.SetTrigger("Attack");

    }

    void AsyncRetract()
    {
        retractRoutine = StartCoroutine(DoRetract());
    }

    IEnumerator DoRetract()
    {
        m_Animator.SetTrigger("Retract");
        retractRoutine = null;
        yield return null;
    }

    public void ResetAttack()
    {
        attacking = false;
        inPosition = false;
    }

    public void ResetTimer()
    {
        attacking = false;
        AttackTimer = Time.time + (health / 3f + Random.Range(MinTimeBetweenAttacks, MaxTimeBetweenAttacks));
        if (movingRoutine != null)
        {
            StopCoroutine(movingRoutine);
        }
        movingRoutine = null;

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
        movingRoutine = null;
        yield return new WaitForSeconds(0.5f);

    }
}
