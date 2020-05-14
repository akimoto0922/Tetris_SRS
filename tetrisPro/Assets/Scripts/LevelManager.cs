using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject gamemanager;
    public GameObject GameClearWindow;
    public GameObject player;

    Vector3 playerPos;

    int score;
    bool makeMino;
    bool isActive;
    int[,] field = new int[12, 22];

    // Start is called before the first frame update
    void Start()
    {
        playerPos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;

        score = gamemanager.GetComponent<GameManager>().score;
        makeMino = gamemanager.GetComponent<GameManager>().makeMino;
        isActive = gamemanager.GetComponent<GameManager>().isActive;
        field = gamemanager.GetComponent<GameManager>().field;

        // キャラクターの移動
        if (score >= 4000)
        {
            // クリア時の Window を表示
            GameClearWindow.SetActive(true);
            isActive = false;
            makeMino = false;

            for (int i = 1; i < field.GetLength(0) - 1; i++)
            {

                for (int j = 1; j < field.GetLength(1) - 1; j++)
                {
                    field[i, j] = 6;
                }
            }

            playerPos.x = 19;
            playerPos.y = 14;
            player.transform.position = playerPos;
        }
        else if (4000 > score && score >= 2000)
        {
            playerPos.x = 25;
            playerPos.y = 8;
            player.transform.position = playerPos;
        }
        else
        {
            playerPos.x = 20;
            playerPos.y = 1;
            player.transform.position = playerPos;
        }
    }
}
