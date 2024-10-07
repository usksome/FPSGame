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

        Destroy(GameManager.instance.gameObject);   // 씬이 다시 게임씬으로 넘어갔을 때 게임 매니저가 다시 만들어진다. 그러다 보면 게임 매니저가 여러개 된다. 그래서 게임 매니저를 하나로 유지하기 위해 Destroy 해줌

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
