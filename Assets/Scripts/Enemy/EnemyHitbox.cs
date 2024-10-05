using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField]
    private Animator m_EnemyAnimator;
    

    public void Hit()
    {
        var light = m_EnemyAnimator.
            gameObject.transform.Find("Light");;
        light.gameObject.SetActive(true);
        m_EnemyAnimator.SetTrigger("ReceiveAttack");
    }
}
