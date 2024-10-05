using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_BackgroundMusic;
    [SerializeField]
    private AudioClip m_FightMusic;

    private AudioSource m_AudioSource;

    private void Awake() 
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void StartFightMusic()
    {
        m_AudioSource.clip = m_FightMusic;
        m_AudioSource.Play();
    }
    
}
