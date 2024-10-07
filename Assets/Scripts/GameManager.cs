using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public int enemyLeft = 10;
    public float timeLeft = 60;

    bool isPlaying = true;


    private void Awake()
    {
       instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {        

        DontDestroyOnLoad(gameObject);   // 게임 오브젝트가 이 씬이 사라질 때 파괴되지 않고 다음 씬에도 여전히 살아있게 된다.

    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;   // 남은 시간을 처리하기 위해서 deltaTime을 빼주면 된다.


        if (isPlaying )
        {
            if (timeLeft <= 0)
            {
                GameOverScene();
            }

        }

    }


    public void EnemyDied()
    {
        enemyLeft --;
        if (enemyLeft <= 0 )
        {
            GameOverScene();
        }
    }
   

    public void GameOverScene()
    {
        isPlaying = false;
        SceneManager.LoadScene("GameOverScene");
    }
    
    



}
