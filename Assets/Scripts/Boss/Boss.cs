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
    private bool esperaSalto;
    //[SerializeField] private Rigidbody2D rbSalto;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    void OnValidate()
    {
        tiempoEspera.y = Mathf.Max(tiempoEspera.x, tiempoEspera.y);
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        StartCoroutine(Behaviour());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    private IEnumerator Behaviour()
    {
        yield return new WaitForSeconds(Random.Range(tiempoEspera.x, tiempoEspera.y));
        StartCoroutine(Attack1());
    }

    private IEnumerator Attack1()
    {
        anim.SetTrigger("Attack1");
        int n = 3;//Random.Range(1,2);
        for(int i=0; i<n; i++)
        {
            yield return new WaitUntil(() => esperaSalto);
            yield return StartCoroutine(Jump());
            piso.transform.DOMove(Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height))), tiempoAire);
            yield return new WaitForSeconds(tiempoAire);
            Debug.Log("dd");
            yield return StartCoroutine(Fall());
        }
    }

    private IEnumerator Jump()
    {
        while (bossSprite.localPosition.y < 6.5f)
        {
            bossSprite.localPosition += velSalto * Time.deltaTime * Vector3.up;
            yield return null;
        }
    }

    private IEnumerator Fall()
    {
        while (bossSprite.localPosition.y > 0)
        {
            bossSprite.localPosition -= velSalto * Time.deltaTime * Vector3.up;
            yield return null;
        }
        Debug.Log("deee");
        bossSprite.localPosition = Vector3.zero;
    }

    public void EsperaSalto()
    {
        esperaSalto = true;
    }

}