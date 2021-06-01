using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatoController : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Walk,
        Attack
    }

    [Header("Estado Inicial.")]
    public FSMStates currentState = FSMStates.Walk;
    
    [Header("Limitadores do Gato.")]
    public Vector2 maxPos;
    public Vector2 minPos;
    float direction = 1f;

    [Header("Velocidade.")]
    public float speed = 2f;

    [Header("GhostManager.")]
    public GhostManager ghostManager;

    Animator anim;
    Rigidbody2D rb;

    public float timer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }


    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        CheckState();
        //ChangeState();
    }

    void CheckState()
    {
        if (currentState == FSMStates.Idle)
        {
            IdleState();
        }
        else if (currentState == FSMStates.Walk)
        {
            WalkState();
        }
        else if (currentState == FSMStates.Attack)
        {
            AttackState();
        }
    }
/*
    void ChangeState()
    {
        switch(currentState)
        {
            case FSMStates.Attack:
                StartCoroutine(TimeToChangeState(1.2f, FSMStates.Idle));
                break;

            case FSMStates.Idle:
                float random = Random.Range(3f, 5f);
                StartCoroutine(TimeToChangeState(random, FSMStates.Walk));
                break;

            case FSMStates.Walk:
                float random2 = Random.Range(10f, 12f);
                StartCoroutine(TimeToChangeState(random2, FSMStates.Idle));
                break;
        }
    }

    IEnumerator TimeToChangeState(float time, FSMStates stateToChange)
    {
        yield return new WaitForSeconds(time);
        currentState = stateToChange;
    }
*/
    private void WalkState()
    {
        float random = Random.Range(10f, 12f);
        if (timer >= random)
        {
            timer = 0;
            currentState = FSMStates.Idle;
        }
        direction = transform.localScale.x;
        anim.SetBool("andando", true);

        if(direction > 0)
        {
            rb.MovePosition(transform.position + transform.right * speed * Time.fixedDeltaTime);
        }
        else if ( direction < 0)
        {
            rb.MovePosition(transform.position - transform.right * speed * Time.fixedDeltaTime);
        }

        if (transform.position.x >= maxPos.x || transform.position.x <= minPos.x)
            ChangeDirection();
    }

    void ChangeDirection()
    {
        Vector3 temp = transform.localScale;
        temp.x *= -1;
        transform.localScale = temp;
    }


    void IdleState()
    {
        anim.SetBool("andando", false);
        float random = Random.Range(3f, 5f);
        if (timer >= random)
        {
            timer = 0;
            currentState = FSMStates.Walk;
        }
    }

    void AttackState()
    {
        anim.SetTrigger("ataque");
        float random = 1.2f;
        if (timer >= random)
        {
            timer = 0;
            currentState = FSMStates.Idle;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (transform.position.x > other.transform.position.x && transform.localScale.x > 0)
                ChangeDirection();
            else if (transform.position.x < other.transform.position.x && transform.localScale.x < 0)
                ChangeDirection();

            currentState = FSMStates.Attack;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.Play("MariposaMorrendo");
            ghostManager.recording = false;
            Destroy(collision.gameObject);
            GameManager.instance.podeInst = true;
        }
    }

}
