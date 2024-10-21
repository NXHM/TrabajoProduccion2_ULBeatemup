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
        // Combina las capas "Enemy" y "Projectiles" en la misma máscara
        int layerMask = LayerMask.GetMask("Enemy", "Projectiles");

        // Detecta colisiones con las capas "Enemy" y "Projectiles"
        var collider = Physics2D.OverlapCircle(
            m_AttackQueryPoint.position,
            m_AttackRange,
            layerMask
        );

        if (collider != null)
        {
            Debug.Log("Hubo colisión con: " + collider.name);

            // Verificar si es un enemigo
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                collider.gameObject.GetComponent<EnemyHitbox>().Hit();
            }
            // Verificar si es un proyectil
            else if (collider.gameObject.layer == LayerMask.NameToLayer("Projectiles"))
            {
                // Aquí puedes agregar la lógica que quieras, por ejemplo destruir el proyectil o desactivarlo
                Debug.Log("Colisionó con un proyectil");
                Destroy(collider.gameObject); // O devolver al pool si es un proyectil de pool
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (m_AttackQueryPoint != null)
        {
            Gizmos.color = Color.green; // Cambia el color para depuración visual
            Gizmos.DrawWireSphere(m_AttackQueryPoint.position, m_AttackRange); // Dibuja el rango de ataque en la escena
        }
    }


    /*private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(m_AttackQueryPoint.position, m_AttackRange);
    }*/
}
