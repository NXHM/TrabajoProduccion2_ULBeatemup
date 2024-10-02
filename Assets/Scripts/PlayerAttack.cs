using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement m_PlayerMovement;

    [SerializeField]
    private float m_AttackRange = 0.25f;

    private Transform m_AttackQueryPoint;

    private void Awake() {
        m_AttackQueryPoint = transform.Find("AttackQueryPoint");
    }

    public void AttackEnd()
    {
        m_PlayerMovement.DeactivateAttack1();
    }

    public void AttackQuery()
    {
        var collider = Physics2D.OverlapCircle(
            m_AttackQueryPoint.position,
            m_AttackRange,
            LayerMask.GetMask("Enemy")
        );
        if (collider != null)
        {
            // hubo colision
            Debug.Log("Hubo colision");
            collider.gameObject.GetComponent<EnemyHitbox>().Hit();
        }
    }

    /*private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(m_AttackQueryPoint.position, m_AttackRange);
    }*/
}
