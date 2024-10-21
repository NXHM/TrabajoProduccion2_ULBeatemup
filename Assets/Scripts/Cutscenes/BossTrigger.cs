using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossTrigger : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector m_PlayableDirector;

    [SerializeField]
    private GameObject boss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.SetActive(true);
            m_PlayableDirector.Play();
            gameObject.SetActive(false);
        }
    }
}
