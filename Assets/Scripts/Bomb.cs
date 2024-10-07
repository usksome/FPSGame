using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public float time;
    public float damage;
    public AudioClip explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;

        if (time <= 0)
        {
            if (!GetComponent<AudioSource>().isPlaying)   // 소리가 나고 있는지... 소리가 안나고 있으면 소리를 낸다.
            {
                GetComponent<AudioSource>().PlayOneShot(explosionSound);
            }
          
            GetComponent<Animator>().SetTrigger("Explosion");

            Invoke("DestroyThis", 3f);

        }

    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Health>().Damage(damage);
        }
    }



}
