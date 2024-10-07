using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{


    public float hp = 10;
    public IHealthListener healthListener;
    public float iinvincibleTime;   // 무적 시간

    public AudioClip hitSound;
    public AudioClip dieSound;


    public Image hpGauge;

    float maxHP;
    float lastAttackedtime;   // 마지막으로 맞은 시간은?



    // Start is called before the first frame update
    void Start()
    {
        maxHP = hp;
        healthListener = GetComponent<Health.IHealthListener>();   // component
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Damage (float damage)
    {
        if (hp > 0 && lastAttackedtime + iinvincibleTime < Time.time)   // 마지막 공격 받은 후에 몇초나 지났는지 체크해서 지난게 맞으면 데미지를 받는다. 
        {
            hp -= damage;

            if (hpGauge != null)
            {
                hpGauge.fillAmount = hp / maxHP;   //
            }
        

           
            lastAttackedtime = Time.time;   // 마지막 공격시간을 현재 시간으로 세팅

            if (hp <= 0)
            {

                if (dieSound != null)
                {
                    GetComponent<AudioSource>().PlayOneShot(dieSound);
                }


                if (healthListener != null)
                {
                    healthListener.Die();
                }
            }

            else
            {
                if (hitSound != null)
                GetComponent<AudioSource>().PlayOneShot(hitSound);
            }
        }
    }


    public interface IHealthListener
    {
        void Die();
    }



}
