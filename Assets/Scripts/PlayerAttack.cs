using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement m_PlayerMovement;

    public void AttackEnd(int num)
    {
        Debug.Log("AnimationEvent");
        m_PlayerMovement.DeactivateAttack1();
    }
}
