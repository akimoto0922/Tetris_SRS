using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject GameClearWindow;
    public GameObject GameOverWindow;
    public GameObject ResultWindow;

    int score;
    bool makeMino;
    bool isActive;
    int[,] field = new int[12, 22];

    // Update is called once per frame
    void Update()
    {
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
        }
    }
}
