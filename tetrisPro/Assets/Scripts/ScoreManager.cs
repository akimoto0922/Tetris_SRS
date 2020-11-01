using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public GameObject PlayerController;
    public Text scoreText;
    public Text highScoreText;

    int score;
    int highScore;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        score = PlayerController.GetComponent<PlayerController>().score;
        // 最高記録入れ替え
        if (highScore < score)
        {
            highScore = score;
        }

        // スコアを text に表示する
        scoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();
    }
}
