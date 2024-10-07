using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{

    public TextMeshProUGUI bulletNumberLabel;
    public GameObject trailPrefab;
    public GameObject particlePrefab; // ��ƼŬ prefab �޾ƿ���
    public AudioClip gunShotSound;   // �� �Ҹ�
    public Transform firingPosition; // �ѱ��� ��ġ�� ��������. (�� ��ġ ������ ����)

    public int bullet;   // ���� ������ �ִ� �Ѿ� ����
    public int totalBullet;   // ��ü �Ѿ� ����
    public int maxBulletMagazine;   // ��źâ�� �� �� �ִ� �Ѿ� ����
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
                bullet += totalBullet;   // ���� �ִ� �Ѿ� ��ŭ �� ���� �ְ�
                totalBullet = 0;   // ���� �ִ� �Ѿ��� 0�� �� ���̴�.
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
        Camera cam = Camera.main;    // ���� ī�޶� ����

        RaycastHit hit;    // �ѿ��� ������ ���� ���� �������� ���� �� ��� ���� �޾ƿ��� ���ؼ� ����ϴ� ����
        Ray r = cam.ViewportPointToRay(Vector3.one / 2);   // ȭ�� ���߾ӿ��� ���� �߻��Ѵ�. ���� ���� ��

        Vector3 hitPosition = r.origin + r.direction * 200;  // ���� �������� ���� �������� 200m ���� �� ���� ���� ����. (������ ��� �ȸ°� ������ ������ �Ÿ� ���� �δ� ��)

        if (Physics.Raycast(r, out hit, 1000))    // r�� �� ��(ī�޶� ����� ������ ��� ��), out hit�� ���� �� ����� ���� ����, �ִ�Ÿ��� 1000m �������� �߻��� �� �ִ�.
        {                                         // ��� �ε������� true, ���� ���µ� �ƹ� ���� �ε����� �ʾ����� false
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

            StartCoroutine(RemovalTrail(obj));       // coroutine �������� ȣ�� �ϰڴ�. (Invoke�� �ٸ�), ��, ������ �Լ� ȣ��, ���� ������� ����. �ڸ�ƾ�� �Լ������ϴٰ� �ڸ�ƾ�� ȣ���ϸ� �Լ� ������ ��ӵǰ� �ڸ�ƾ�� ��Ӱ��� �ȴ�.
        }

    }

    IEnumerator RemovalTrail(GameObject obj)    // ������ ���� ������� ��
    {
        yield return new WaitForSeconds(0.3f);

        Destroy(obj);
    }

}
