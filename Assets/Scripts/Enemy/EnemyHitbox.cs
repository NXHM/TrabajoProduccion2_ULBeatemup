using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField]
    private Animator m_EnemyAnimator;
    [SerializeField] private bool isBoss;

    [SerializeField] Boss boss;

    public void Hit()
    {
        if (!isBoss)
        {
            var light = m_EnemyAnimator.
                gameObject.transform.Find("Light");
            light.gameObject.SetActive(true);
            m_EnemyAnimator.SetTrigger("ReceiveAttack");
        }
        else
        {
            Debug.Log("SnorlaxHit");
            boss.TakeDamage(10);
        }
    }
}
