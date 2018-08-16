using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Color goodColor;
    public Color badColor;

    public Image[] hearts;
    Health m_Health;

    // Use this for initialization
    void Start()
    {
        m_Health = GameObject.FindObjectOfType<PlayerAnimator>().GetComponent<Health>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].color = i <= (m_Health.hearts - 1) ? goodColor : badColor;
        }
    }
}
