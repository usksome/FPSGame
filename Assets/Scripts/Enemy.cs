using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, Health.IHealthListener
{

    enum State
    {
        Idle,
        Walk,
        Attack,
        Dying
    };


    public GameObject player;
    NavMeshAgent agent;

    Animator animator;

    State state;
    float timeForNextState = 2;


    AudioSource audio;


    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case State.Idle:
                float distance = (player.transform.position - (transform.position + GetComponent<CapsuleCollider>().center)).magnitude;     // 플레이어 중심에서 적캐릭터 중심을 뺐다. Vector3
                if (distance < 1.0f)   // 거리가 1.0m 보다 가까우면... 붙은 상태
                {
                    Attack();   // 공격
                }
                else 
                {
                    timeForNextState -= Time.deltaTime;   // 시간이 가기를 기다림.
                    if (timeForNextState < 0)   // 
                    {
                        StartWalk();
                    }
                    
                }
                break;

            case State.Walk:
                if (agent.remainingDistance < 1.0f || !agent.hasPath)   // agent가 만약 남은 거리가 1m 이하면 원래 목표로 했떤 지점에 도착했다라고 생각을 하도록... 또는 지금 가려는 길이 실제로 길이 있는지(hasPath)...
                {

                    StartIdle();

                }
                break;


            case State.Attack:
                
                
                timeForNextState -= Time.deltaTime;
                if (timeForNextState < 0)     // 남은 시간이 0보다 작아지면...
                {
                    StartIdle();
                }

                break;

                


        }
    }


    void Attack()
    {
        state = State.Attack;
        timeForNextState = 1.5f;   // 공격 모션 후 1.5초 후
        animator.SetTrigger("Attack");
    }

    void StartWalk()
    {
        audio.Play();
        state = State.Walk;
        agent.destination = player.transform.position;
        agent.isStopped = false;
        animator.SetTrigger("Walk");
    }

    void StartIdle()
    {
        audio.Stop();
        state = State.Idle;
        timeForNextState = Random.Range(1f, 2f);
        agent.isStopped = true;
        animator.SetTrigger("Idle");
    }


    public void Die()
    {
        
        state = State.Dying;
        agent.isStopped = true;
        animator.SetTrigger("Die");
        Invoke("DestroyThis", 2.5f);   // 2.5초 있다가 적이 사라지기...

    }

    void DestroyThis()
    {
        GameManager.instance.EnemyDied();   // 게임 매니저한테 적이 죽었다는 사실을 알려줘야 한다.
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")   // 만약 other.tag가 Player라고 한다면 
        {
            other.GetComponent<Health>().Damage(5);   // other.getcomponent<health>에게 데미지를 준다.
        }
    }



}
