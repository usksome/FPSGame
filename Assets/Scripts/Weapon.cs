using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{

    public TextMeshProUGUI bulletNumberLabel;
    public GameObject trailPrefab;
    public GameObject particlePrefab; // 파티클 prefab 받아오기
    public AudioClip gunShotSound;   // 총 소리
    public Transform firingPosition; // 총구의 위치를 가져오자. (빛 위치 조정을 위함)

    public int bullet;   // 현재 가지고 있는 총알 개수
    public int totalBullet;   // 전체 총알 개수
    public int maxBulletMagazine;   // 한탄창에 들어갈 수 있는 총알 개수
    public float damage;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && bullet>0)
        {
            if (animator != null)
            {
                animator.SetTrigger("Shot");
                
            }

            bullet--;
            Fire();
        }


        if (Input.GetButtonDown("Reload"))
        {
            if (animator != null)
            {
                animator.SetTrigger("Reload");
            }
            

            if (totalBullet >= maxBulletMagazine - bullet)
            {
                totalBullet -= maxBulletMagazine - bullet;
                bullet = maxBulletMagazine;
            }

            else
            {
                bullet += totalBullet;   // 남아 있는 총알 만큼 다 집어 넣고
                totalBullet = 0;   // 남아 있는 총알은 0가 될 것이다.
            }

        }


        bulletNumberLabel.text = bullet + "/" + totalBullet;


    }



    virtual protected void Fire()
    {
        RaycastFire();
    }



    void RaycastFire()   // RaycastFire
    {
        GetComponent<AudioSource>().PlayOneShot(gunShotSound);
        Camera cam = Camera.main;    // 메인 카메라 정보

        RaycastHit hit;    // 총에서 광선을 쏴서 내가 전방으로 빛을 쏜 결과 값을 받아오기 위해서 사용하는 변수
        Ray r = cam.ViewportPointToRay(Vector3.one / 2);   // 화면 정중앙에서 빛을 발사한다. 빛을 만든 것

        Vector3 hitPosition = r.origin + r.direction * 200;  // 빛의 원점에서 빛의 방향으로 200m 정도 간 곳에 점을 찍음. (광선이 어디 안맞고 밖으로 나가는 거리 제한 두는 것)

        if (Physics.Raycast(r, out hit, 1000))    // r은 쏠 빛(카메라 가운데서 앞으로 쏘는 빛), out hit은 빛을 쏜 결과를 받을 변수, 최대거리는 1000m 정도까지 발사할 수 있다.
        {                                         // 어딘가 부딪혔으면 true, 빛을 쐈는데 아무 데도 부딪히지 않았으면 false
            hitPosition = hit.point;

            GameObject particle = Instantiate(particlePrefab);
            particle.transform.position = hitPosition;
            particle.transform.forward = hit.normal;


            if (hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<Health>().Damage(damage);
            }

        }


        if (trailPrefab != null)    //
        {
            GameObject obj = Instantiate(trailPrefab);
            Vector3[] pos = new Vector3[] { firingPosition.position, hitPosition };
            obj.GetComponent<LineRenderer>().SetPositions(pos);

            StartCoroutine(RemovalTrail(obj));       // coroutine 형식으로 호출 하겠다. (Invoke랑 다른), 함, 기존은 함수 호출, 종료 순서대로 간다. 코르틴은 함수실행하다가 코르틴을 호출하면 함수 실행은 계속되고 코르틴도 계속가게 된다.
        }

    }

    IEnumerator RemovalTrail(GameObject obj)    // 생성된 빛을 사라지게 함
    {
        yield return new WaitForSeconds(0.3f);

        Destroy(obj);
    }

}
