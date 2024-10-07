using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{


    public float hp = 10;
    public IHealthListener healthListener;
    public float iinvincibleTime;   // ���� �ð�

    public AudioClip hitSound;
    public AudioClip dieSound;


    public Image hpGauge;

    float maxHP;
    float lastAttackedtime;   // ���������� ���� �ð���?



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
        if (hp > 0 && lastAttackedtime + iinvincibleTime < Time.time)   // ������ ���� ���� �Ŀ� ���ʳ� �������� üũ�ؼ� ������ ������ �������� �޴´�. 
        {
            hp -= damage;

            if (hpGauge != null)
            {
                hpGauge.fillAmount = hp / maxHP;   //
            }
        

           
            lastAttackedtime = Time.time;   // ������ ���ݽð��� ���� �ð����� ����

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
