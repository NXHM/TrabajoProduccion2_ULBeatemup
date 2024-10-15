using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss : MonoBehaviour
{
    [SerializeField] private Vector2 tiempoEspera;
    [SerializeField] private Transform bossSprite;
    //[SerializeField] private Rigidbody2D rbSalto;

    void OnValidate()
    {
        tiempoEspera.y = Mathf.Max(tiempoEspera.x, tiempoEspera.y);
    }

    void Start()
    {
        StartCoroutine(Behaviour());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Behaviour()
    {
        yield return new WaitForSeconds(Random.Range(tiempoEspera.x, tiempoEspera.y));
        StartCoroutine(Attack1());
    }

    private IEnumerator Attack1()
    {
        int n = Random.Range(1,2);
        for(int i=0; i<n; i++)
        {
            yield return StartCoroutine(Jump());
            float aux = 0.5f;
            transform.DOMove(Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height))), aux);
            yield return new WaitForSeconds(aux);
            yield return StartCoroutine(Fall());
        }
    }

    private IEnumerator Jump()
    {
        while (bossSprite.localPosition.y < 6.5f)
        {
            bossSprite.localPosition += 20f * Time.deltaTime * Vector3.up;
            yield return null;
        }
    }

    private IEnumerator Fall()
    {
        while (bossSprite.localPosition.y > 0)
        {
            bossSprite.localPosition -= 20f * Time.deltaTime * Vector3.up;
            yield return null;
        }
        bossSprite.localPosition = Vector3.zero;
    }

}