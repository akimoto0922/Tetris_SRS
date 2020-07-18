using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject GameClearWindow;
    public GameObject GameOverWindow;

    Vector3 playerPos;

    int score;
    bool makeMino;
    bool isActive;
    int[,] field = new int[12, 22];

    // Start is called before the first frame update
    void Start()
    {
        playerPos = Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = Player.transform.position;


        score = Player.GetComponent<PlayerController>().score;
        makeMino = Player.GetComponent<PlayerController>().makeMino;
        isActive = Player.GetComponent<PlayerController>().isActive;
        field = Player.GetComponent<PlayerController>().field;

        // ゲームオーバー
        if (isActive == false && score < 4000)
        {
            GameOverWindow.SetActive(true);
            makeMino = false;
        }

        // リトライ時に非表示にする
        if (isActive == true)
        {
            GameClearWindow.SetActive(false);
            GameOverWindow.SetActive(false);
        }

        //　ゲームクリア
        if (score >= 4000)
        {
            // クリア時の Window を表示
            GameClearWindow.SetActive(true);
            makeMino = false;

            var ClearMino = 6; 
            for (int i = 1; i < field.GetLength(0) - 1; i++)
            {

                for (int j = 1; j < field.GetLength(1) - 1; j++)
                {
                    field[i, j] = ClearMino;
                }
            }

            playerPos.x = 19;
            playerPos.y = 14;
            Player.transform.position = playerPos;
        }
        else if (4000 > score && score >= 2000)
        {
            playerPos.x = 25;
            playerPos.y = 8;
            Player.transform.position = playerPos;
        }
        else
        {
            playerPos.x = 20;
            playerPos.y = 1;
            Player.transform.position = playerPos;
        }
    }
}
