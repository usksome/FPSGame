using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, Health.IHealthListener
{

    public GameObject[] weapons;   // 무기 넣기

    public float walkingSpeed = 7;

    public float mouseSens = 10;

    public float jumpSpeed = 10;

    public Transform cameraTransform;  // Transform을 통해서 카메라의 각도 정도 같은 거 가져오기 위함

    float verticalAngle;   // 각도
    float horizontalAngle;   // 각도

    float verticalSpeed;

    bool isGrounded;   // 땅에 붙어 있는지 아닌지...
    float groundedTimer;   // 공중에 떠 있는 답변이 얼마나 오랫동안 들어 왔는지...

    int currentWeapon;

    CharacterController characterController;  // 플레이어는 캐릭터 컨트롤러로 조종

    


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 마우스 커서가 안으로 들어오면 게임 화면 안에 마우스 커서가 갇힌다.
        Cursor.visible = false;   // 마우스 커서 보이지 않게 함

        isGrounded = true;
        groundedTimer = 0;

        verticalSpeed = 0;
        verticalAngle = 0;   //수직 각도는 기본적으로 0 으로...
        horizontalAngle = transform.localEulerAngles.y;   // 수평 각도는 0이 아닐 수 있으므로, 좌우 각도, 현재 플레이어 각도를 가지고 온다. (eulerAngle.y - y축을 중심으로 한 각도를 가지고 오기 때문에 좌우로 어느 정도 각도가 기울어져 있는지를 가지고 올 수 있다.


        characterController = GetComponent<CharacterController>();   // 이 스크립트가 붙어 있는 오브젝트의 캐릭터 컨트롤러라는 컴퓨넌트를 가지고 와서 움직이도록 해달라는 의미


        currentWeapon = 0;   //  처음 시작 무기는 총으로 시작
        UpdateWeapon();
    }

    // Update is called once per frame
    void Update()
    {

        if (!characterController.isGrounded)  // 땅에 안붙어 있는지...    (0.5초 이상 false가 나와야 나는 땅에서 떨어진게 된 거다.)
        {

            if (isGrounded)  // 땅에 붙어 있는지...
            {
                groundedTimer += Time.deltaTime;
                if (groundedTimer > 0.5f)   // 시간은 얼마나 지속 되었는지... 만약 0.5초 이상이라면 
                {
                    isGrounded = false;   // 땅에서 떨어진거다.
                }
            }


        }

        else    // 그렇지 않다면
        {
            isGrounded = true;    // 땅에 붙어 있네
            groundedTimer = 0;   // 타이머는 다시 초기화
        }

        
        //점프

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalSpeed = jumpSpeed;
            isGrounded = false;
        }


        //평행 이동
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));  // 움직여야 할 방법, x는 가로 움직임, y는 0,  z는 세로 움직임

        if (move.magnitude > 1)  // 움직임을 부드럽게 하기 위해, 벡터의 크기가 만약에 1보다 크면 1에 맞춰줄 것이다.
        {
            move.Normalize(); // 벡터의 크기를 1로 맞춰줄 것이다. 대각선 움직임 빨라지는 것을 예방
        }

        move = move * walkingSpeed * Time.deltaTime;   //
        move = transform.TransformDirection(move);     // 캐릭터가 바라보는 방향으로 바꿔주는 명령
        characterController.Move(move); // 지금 방금 전까지 만든 벡터대로 움직여 달라는 의미



        //좌/우 마우스

        float turnPlayer = Input.GetAxis("Mouse X") * mouseSens;   // 마우스의 X 값 가지고 온다. 마우스 X가 가로로 얼마나 움직였는지 알 수 있다. 마우스 감도 곱함.
        horizontalAngle += turnPlayer;   // 수평 각도에 대해 turnPlayer 값만큼 더해준다.

        if (horizontalAngle > 360) horizontalAngle -= 360;   // 더해준 값이 360도를 넘으면 360만큼 빼준다. 즉 값이 0에서 360사이에 있도록 조정해주는 것이다.
        if (horizontalAngle < 0) horizontalAngle += 360;   // 값이 0 보다 작아도 360을 더해줘서 0에서 360 사이가 있도록 해준다.

        Vector3 currentAngle = transform.localEulerAngles;   // 현재 캐릭터 쳐다보고 있는 가로 방향을 가지고 온다.
        currentAngle.y = horizontalAngle;   // currentAngle의 y방향 각도만 horizontalAngle로 바꿔준다.
        transform.localEulerAngles = currentAngle;  // 다시 y만 바꾼 값을 넣어준다.


        //상/하 마우스
        // 상하 움직임은 캐릭터가 움직이고자 하는 각도와 관계가 없다. 쳐다보는 각도만 변경시킨다.

        float turnCam = Input.GetAxis("Mouse Y") * mouseSens;
        verticalAngle -= turnCam;   // 카메라 턴 값을 빼줘야 한다.
        verticalAngle = Mathf.Clamp(verticalAngle, -86f, 89f);   //verticalAngle이 -86f와 86f 사이에 존재할 수 있도록 해준다.
        currentAngle = cameraTransform.localEulerAngles;   //
        currentAngle.x = verticalAngle;   // X축을 중심으로 한 각도를 조정한다. X축을 중심으로 돌리기 때문에 앵글 벡터의 X값을 바꾸게 된다.
        cameraTransform.localEulerAngles = currentAngle;  // 다시 카메라에 넣어준다.

        // 상하 마우스는 X축을 중심으로 돌아가기 때문이다.



        //낙하

        verticalSpeed -= 10 * Time.deltaTime;   // 중력 가속도 * Time.deltaTime 속도의 변화
        if (verticalSpeed < -10)
        {
            verticalSpeed = -10;   // 버티컬 속도는 종단 속도인 -10에서 멈추도록
        }


        Vector3 verticalMove = new Vector3(0, verticalSpeed, 0);   // 수직 방향으로 어떻게 움직일 것인가...
        verticalMove = verticalMove *Time.deltaTime;   // 속도에 시간을 곱한다.
        CollisionFlags flag = characterController.Move(verticalMove);   //


        if ((flag & CollisionFlags.Below) != 0)
        {
            verticalSpeed = 0;
        }



        //무기 변경

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
