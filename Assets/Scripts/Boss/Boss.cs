using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss : MonoBehaviour
{
    [SerializeField] private Vector2 tiempoEspera;
    [SerializeField] private float velSalto = 20f, tiempoAire = 1.5f;
    [SerializeField] private Transform piso, bossSprite;
    [SerializeField] private Animator anim;
    [SerializeField] private Collider2D col;
    private bool esperaSalto;
    //[SerializeField] private Rigidbody2D rbSalto;

    void OnValidate()
    {
        tiempoEspera.y = Mathf.Max(tiempoEspera.x, tiempoEspera.y);
    }

    void Start()
    {
        StartCoroutine(Behaviour());
    }

    private IEnumerator Behaviour()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(tiempoEspera.x, tiempoEspera.y));
            yield return StartCoroutine(Attack1());
        }
    }

    private IEnumerator Attack1()
    {
        anim.SetTrigger("Attack1");
        yield return new WaitUntil(() => esperaSalto);
        esperaSalto = false;
        col.enabled = false;
        int n = Random.Range(3,6);
        for(int i=0; i<n; i++)
        {   
            yield return StartCoroutine(Jump());
            Vector3 posRandom = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(10, Screen.width-10), 0f));
            posRandom.y = Random.Range(-1.5f, -4.85f);
            posRandom.z = 0;
            piso.transform.DOMove(posRandom, tiempoAire);
            bossSprite.eulerAngles = Vector3.forward * 90;
            bossSprite.DOLocalMoveX(2.2f, 0);
            //bossSprite.localPosition = new Vector3(2.2f, 1.7f);
            yield return new WaitForSeconds(tiempoAire);
            if(i < n-1) yield return StartCoroutine(Fall());
        }
        bossSprite.eulerAngles = Vector3.zero;
        bossSprite.DOLocalMoveX(0f, 0);
        yield return StartCoroutine(Land());
        col.enabled = true;
    }

    private IEnumerator Jump()
    {
        while (bossSprite.localPosition.y < 8f)
        {
            bossSprite.localPosition += velSalto * Time.deltaTime * Vector3.up;
            yield return null;
        }
    }

    private IEnumerator Fall()
    {
        while (bossSprite.localPosition.y > 1.7f)
        {
            bossSprite.localPosition -= velSalto * Time.deltaTime * Vector3.up;
            yield return null;
        }
        //bossSprite.localPosition = Vector3.zero;
    }

    private IEnumerator Land()
    {
        bool aux=true;
        while (bossSprite.localPosition.y > 0)
        {
            bossSprite.localPosition -= velSalto * Time.deltaTime * Vector3.up;
            if (aux && bossSprite.localPosition.y <= 0.2f)
            {
                aux = false;
                anim.SetTrigger("Attack1");
            }
            yield return null;
        }
        bossSprite.localPosition = Vector3.zero;
    }

    public void EsperaSalto()
    {
        esperaSalto = true;
    }

}