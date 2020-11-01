using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBattle : MonoBehaviour
{
    public GameObject Player;
    public int PlayerNum = 2;

    // Start is called before the first frame update
    void Start()
    {
        InstanciatePlayer(PlayerNum);
    }

    void InstanciatePlayer(int PlayerNum)
    {
        var pos_x = 0;
        var Interval = 30;
        for (int i = 0; i < PlayerNum; i++)
        {
            Instantiate(Player, new Vector3(pos_x, 0, 0), Quaternion.identity);
            pos_x += Interval;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
