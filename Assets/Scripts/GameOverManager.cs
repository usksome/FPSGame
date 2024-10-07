using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverManager : MonoBehaviour
{

    public TextMeshProUGUI titleLabel;
    public TextMeshProUGUI enemyKilledLabel;
    public TextMeshProUGUI timeLeftLabel;



    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        


        int enemyLeft = GameManager.instance.enemyLeft;
        float timeLeft = GameManager.instance.timeLeft;


        if (enemyLeft <= 0)
        {
            titleLabel.text = "Cleared!";
        }

        else
        {
            titleLabel.text = "Game Over...";
        }

        enemyKilledLabel.text = "Enemy Killed : " + (10 - enemyLeft);
        timeLeftLabel.text = "Time Left : " + timeLeft.ToString("#.##");

        Destroy(GameManager.instance.gameObject);   // ���� �ٽ� ���Ӿ����� �Ѿ�� �� ���� �Ŵ����� �ٽ� ���������. �׷��� ���� ���� �Ŵ����� ������ �ȴ�. �׷��� ���� �Ŵ����� �ϳ��� �����ϱ� ���� Destroy ����

    }


    public void PlayAgainPressed()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Quit()
    {
        Application.Quit();   //  
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
