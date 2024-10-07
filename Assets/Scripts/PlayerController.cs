using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, Health.IHealthListener
{

    public GameObject[] weapons;   // ���� �ֱ�

    public float walkingSpeed = 7;

    public float mouseSens = 10;

    public float jumpSpeed = 10;

    public Transform cameraTransform;  // Transform�� ���ؼ� ī�޶��� ���� ���� ���� �� �������� ����

    float verticalAngle;   // ����
    float horizontalAngle;   // ����

    float verticalSpeed;

    bool isGrounded;   // ���� �پ� �ִ��� �ƴ���...
    float groundedTimer;   // ���߿� �� �ִ� �亯�� �󸶳� �������� ��� �Դ���...

    int currentWeapon;

    CharacterController characterController;  // �÷��̾�� ĳ���� ��Ʈ�ѷ��� ����

    


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // ���콺 Ŀ���� ������ ������ ���� ȭ�� �ȿ� ���콺 Ŀ���� ������.
        Cursor.visible = false;   // ���콺 Ŀ�� ������ �ʰ� ��

        isGrounded = true;
        groundedTimer = 0;

        verticalSpeed = 0;
        verticalAngle = 0;   //���� ������ �⺻������ 0 ����...
        horizontalAngle = transform.localEulerAngles.y;   // ���� ������ 0�� �ƴ� �� �����Ƿ�, �¿� ����, ���� �÷��̾� ������ ������ �´�. (eulerAngle.y - y���� �߽����� �� ������ ������ ���� ������ �¿�� ��� ���� ������ ������ �ִ����� ������ �� �� �ִ�.


        characterController = GetComponent<CharacterController>();   // �� ��ũ��Ʈ�� �پ� �ִ� ������Ʈ�� ĳ���� ��Ʈ�ѷ���� ��ǻ��Ʈ�� ������ �ͼ� �����̵��� �ش޶�� �ǹ�


        currentWeapon = 0;   //  ó�� ���� ����� ������ ����
        UpdateWeapon();
    }

    // Update is called once per frame
    void Update()
    {

        if (!characterController.isGrounded)  // ���� �Ⱥپ� �ִ���...    (0.5�� �̻� false�� ���;� ���� ������ �������� �� �Ŵ�.)
        {

            if (isGrounded)  // ���� �پ� �ִ���...
            {
                groundedTimer += Time.deltaTime;
                if (groundedTimer > 0.5f)   // �ð��� �󸶳� ���� �Ǿ�����... ���� 0.5�� �̻��̶�� 
                {
                    isGrounded = false;   // ������ �������Ŵ�.
                }
            }


        }

        else    // �׷��� �ʴٸ�
        {
            isGrounded = true;    // ���� �پ� �ֳ�
            groundedTimer = 0;   // Ÿ�̸Ӵ� �ٽ� �ʱ�ȭ
        }

        
        //����

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalSpeed = jumpSpeed;
            isGrounded = false;
        }


        //���� �̵�
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));  // �������� �� ���, x�� ���� ������, y�� 0,  z�� ���� ������

        if (move.magnitude > 1)  // �������� �ε巴�� �ϱ� ����, ������ ũ�Ⱑ ���࿡ 1���� ũ�� 1�� ������ ���̴�.
        {
            move.Normalize(); // ������ ũ�⸦ 1�� ������ ���̴�. �밢�� ������ �������� ���� ����
        }

        move = move * walkingSpeed * Time.deltaTime;   //
        move = transform.TransformDirection(move);     // ĳ���Ͱ� �ٶ󺸴� �������� �ٲ��ִ� ���
        characterController.Move(move); // ���� ��� ������ ���� ���ʹ�� ������ �޶�� �ǹ�



        //��/�� ���콺

        float turnPlayer = Input.GetAxis("Mouse X") * mouseSens;   // ���콺�� X �� ������ �´�. ���콺 X�� ���η� �󸶳� ���������� �� �� �ִ�. ���콺 ���� ����.
        horizontalAngle += turnPlayer;   // ���� ������ ���� turnPlayer ����ŭ �����ش�.

        if (horizontalAngle > 360) horizontalAngle -= 360;   // ������ ���� 360���� ������ 360��ŭ ���ش�. �� ���� 0���� 360���̿� �ֵ��� �������ִ� ���̴�.
        if (horizontalAngle < 0) horizontalAngle += 360;   // ���� 0 ���� �۾Ƶ� 360�� �����༭ 0���� 360 ���̰� �ֵ��� ���ش�.

        Vector3 currentAngle = transform.localEulerAngles;   // ���� ĳ���� �Ĵٺ��� �ִ� ���� ������ ������ �´�.
        currentAngle.y = horizontalAngle;   // currentAngle�� y���� ������ horizontalAngle�� �ٲ��ش�.
        transform.localEulerAngles = currentAngle;  // �ٽ� y�� �ٲ� ���� �־��ش�.


        //��/�� ���콺
        // ���� �������� ĳ���Ͱ� �����̰��� �ϴ� ������ ���谡 ����. �Ĵٺ��� ������ �����Ų��.

        float turnCam = Input.GetAxis("Mouse Y") * mouseSens;
        verticalAngle -= turnCam;   // ī�޶� �� ���� ����� �Ѵ�.
        verticalAngle = Mathf.Clamp(verticalAngle, -86f, 89f);   //verticalAngle�� -86f�� 86f ���̿� ������ �� �ֵ��� ���ش�.
        currentAngle = cameraTransform.localEulerAngles;   //
        currentAngle.x = verticalAngle;   // X���� �߽����� �� ������ �����Ѵ�. X���� �߽����� ������ ������ �ޱ� ������ X���� �ٲٰ� �ȴ�.
        cameraTransform.localEulerAngles = currentAngle;  // �ٽ� ī�޶� �־��ش�.

        // ���� ���콺�� X���� �߽����� ���ư��� �����̴�.



        //����

        verticalSpeed -= 10 * Time.deltaTime;   // �߷� ���ӵ� * Time.deltaTime �ӵ��� ��ȭ
        if (verticalSpeed < -10)
        {
            verticalSpeed = -10;   // ��Ƽ�� �ӵ��� ���� �ӵ��� -10���� ���ߵ���
        }


        Vector3 verticalMove = new Vector3(0, verticalSpeed, 0);   // ���� �������� ��� ������ ���ΰ�...
        verticalMove = verticalMove *Time.deltaTime;   // �ӵ��� �ð��� ���Ѵ�.
        CollisionFlags flag = characterController.Move(verticalMove);   //


        if ((flag & CollisionFlags.Below) != 0)
        {
            verticalSpeed = 0;
        }



        //���� ����

        if (Input.GetButtonDown("ChangeWeapon"))
        {
            currentWeapon++;
            if (currentWeapon >= weapons.Length)
            {
                currentWeapon = 0;
            }

            UpdateWeapon();

        } 

    }

    void UpdateWeapon()
    {
        foreach (GameObject w in weapons)
        {
            w.SetActive(false);
        }

        weapons[currentWeapon].SetActive(true);
    }


    public void Die()
    {
        GetComponent<Animator>().SetTrigger("Die");

        Invoke("TellIamDie", 1.0f);
    }

    void TellIamDie()
    {
        GameManager.instance.GameOverScene();
    }

}
