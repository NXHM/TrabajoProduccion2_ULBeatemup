using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector m_PlayableDirector;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            m_PlayableDirector.Play();
            gameObject.SetActive(false);
        }
    }
}
