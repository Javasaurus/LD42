using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class CrusherBoss : MonoBehaviour
{

    Animator m_Animator;
    AudioSource m_Audiosource;

    public float health = 100;
    private float maxHealth = 100;
    public bool attacking;
    public bool swapping;
    public bool inPosition;

    public float playerSensitivity;

    public CrusherTarget crusherTarget;

    public Transform playerTransform;
    public Transform BossPivot;
 

    public Transform[] BossTargetPoints;
    public Transform BossLeftAnchor;
    public Transform BossRightAnchor;
    public Transform BossCenterAnchor;


    public bool facingRight = true;

    public float AttackTimer;
    public float SideSwapTimer;

    Coroutine movingToOtherSideRoutine;
    Coroutine movingRoutine;
    Coroutine retractRoutine;

    public float MinTimeBetweenSideSwap = 15f;
    public float MaxTimeBetweenSideSwap = 35f;
    public float MinTimeBetweenAttacks = 3f;
    public float MaxTimeBetweenAttacks = 10f;
    public float MinTimeToRetract = 1f;
    public float MaxTimeToRetract = 5f;

    public Image healthBar;

    Vector3 previousPosition;

    // Use this for initialization
    void Start()
    {
        playerTransform = GameObject.FindObjectOfType<PlayerAnimator>().transform;
        m_Animator = GetComponent<Animator>();
        maxHealth = health;
    }

    private void OnEnable()
    {
        ResetTimer();
    }


    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (health / maxHealth);
        if (swapping)
        {
            attacking = false;
            if (retractRoutine != null)
            {
                StopCoroutine(retractRoutine);
            }
            if (movingRoutine != null)
            {
                StopCoroutine(movingRoutine);
            }
            AttackTimer = Time.time + 10f;
            SideSwapTimer = Time.time + 10f;
            return;
        }

        if ((Time.time > SideSwapTimer) && !attacking && movingRoutine == null && retractRoutine == null && movingToOtherSideRoutine == null)
        {
            swapping = true;
            MoveToOtherSide();
        }
        else if (retractRoutine != null)
        {
            //stop moving !
            if (movingRoutine != null)
            {
                StopCoroutine(movingRoutine);
            }
        }
        else if ((inPosition | !attacking) && AttackTimer < Time.time)
        {
            if ((Mathf.Abs(transform.position.y - playerTransform.position.y) <= playerSensitivity))
            {
                if (movingRoutine != null)
                {
                    StopCoroutine(movingRoutine);
                }
            }
            attacking = true;
            StartCoroutine(DoDelayedAttack());
        }

        if (!swapping && !inPosition && !attacking && movingRoutine == null)
        {
            MoveToNextPoint();
        }
    }

    IEnumerator DoDelayedAttack()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        DoAttack();
    }

    void DoAttack()
    {
        attacking = true;
        if (Random.Range(0f, 1f) > 0.25f)
        {
            m_Animator.SetTrigger("Attack");
        }
        ResetTimer();
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
        ResetTimer();
        inPosition = false;
    }

    public void ResetTimer()
    {
        if (attacking)
        {
            AttackTimer = Time.time + (health / 3f + Random.Range(MinTimeBetweenAttacks, MaxTimeBetweenAttacks));
            attacking = false;
        }

        if (swapping)
        {
            SideSwapTimer = Time.time + (Random.Range(MinTimeBetweenSideSwap, MaxTimeBetweenSideSwap));
            swapping = false;
        }

        if (movingRoutine != null)
        {
            StopCoroutine(movingRoutine);
        }
        movingRoutine = null;

        if (movingToOtherSideRoutine != null)
        {
            StopCoroutine(movingToOtherSideRoutine);
        }
        movingToOtherSideRoutine = null;

        if (retractRoutine != null)
        {
            StopCoroutine(retractRoutine);
        }
        retractRoutine = null;
    }

    void Flip()
    {
        //we have to also make sure the boss pivot moves to the right place?
        float distance = Mathf.Abs(BossPivot.position.x - transform.position.x);

        if (facingRight && (BossCenterAnchor.position.x - BossPivot.position.x) < 0)
        {
            facingRight = false;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            transform.position -= new Vector3(distance, 0);

        }

        if (!facingRight && (BossCenterAnchor.position.x - BossPivot.position.x) >= 0)
        {
            facingRight = true;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            transform.position += new Vector3(distance, 0);
        }


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


    Vector3[] movingPoints;
    private void OnDrawGizmos()
    {
        if (movingPoints != null && movingPoints.Length > 0)
        {
            foreach (Vector3 position in movingPoints)
            {
                Gizmos.DrawCube(position, Vector3.one);
            }
        }
    }

    //move between targets
    public void MoveToOtherSide()
    {
        Debug.Log("Swapping sides");
        swapping = true;
        inPosition = false;
        attacking = false;

        Transform currentAnchor = facingRight ? BossLeftAnchor : BossRightAnchor;
        Transform targetAnchor = facingRight ? BossRightAnchor : BossLeftAnchor;

        movingPoints = new Vector3[4];
        movingPoints[0] = new Vector3(currentAnchor.position.x, currentAnchor.position.y + 20f, BossPivot.position.z);
        movingPoints[1] = new Vector3(targetAnchor.position.x, currentAnchor.position.y + 20f, BossPivot.position.z);
        //if we are facing right, that means we need to go outside of the box to make it return horizontally
        if (facingRight)
        {
            movingPoints[2] = new Vector3(targetAnchor.position.x + 10f, targetAnchor.position.y, BossPivot.position.z);
        }
        else
        {
            movingPoints[2] = new Vector3(targetAnchor.position.x - 10f, targetAnchor.position.y, BossPivot.position.z);
        }
        movingPoints[3] = new Vector3(targetAnchor.position.x, targetAnchor.position.y, BossPivot.position.z);
        movingToOtherSideRoutine = StartCoroutine(MoveObject(movingPoints));
    }

    Vector3[] swapPositions;
    int currentIndex;
    public IEnumerator MoveObject(Vector3[] wayPoints)
    {
        // get player seat number
        swapPositions = wayPoints;
        for (int i = 0; i < wayPoints.Length; i++)
        {
            currentIndex = i;
            float distance = Vector3.Distance(new Vector3(BossPivot.position.x, BossPivot.position.y, 0), wayPoints[i]);
            while (distance > .01f)
            {
                //move in the direction the body should be going...
                Vector3 direction = (wayPoints[i] - BossPivot.position).normalized;
                transform.position += direction * Time.deltaTime * 15f;
                distance = Vector3.Distance(new Vector3(BossPivot.position.x, BossPivot.position.y, 0), wayPoints[i]);
                //flip into the direction ?
                yield return null;
            }
            Flip();
            //    transform.position = wayPoints[i];
        }
        movingRoutine = null;
        movingToOtherSideRoutine = null;
        yield return new WaitForSeconds(0.5f);
        crusherTarget.transform.position = new Vector3(crusherTarget.transform.position.x, BossPivot.position.y, crusherTarget.transform.position.z);
        ResetTimer();
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

    public void PlayAudio()
    {
        if (!m_Audiosource)
        {
            m_Audiosource = GetComponent<AudioSource>();
        }
        if (!m_Audiosource.isPlaying)
        {
            m_Audiosource.PlayOneShot(m_Audiosource.clip);
        }
    }

}
