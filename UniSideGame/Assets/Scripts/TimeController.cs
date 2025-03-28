using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public bool isCountDown = true;//trueで時間をカウントダウン計測する
    public float gameTime = 0;//ゲームの最大時間
    public bool isTimeOver = false;//trueでタイマー停止
    public float displayTime = 0;//表示時間

    float times = 0;//現在時間

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isCountDown)
        {
            //カウントダウン
            displayTime = gameTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimeOver == false)
        {
            times += Time.deltaTime;
            if  (isCountDown)
            {
                //カウントダウン
                displayTime = gameTime - times;
                if (displayTime <= 0.0f)
                {
                    displayTime = 0.0f;
                    isTimeOver = true;
                }
            }
            else
            {
                //カウントアップ
                displayTime = times;
                if (displayTime >= gameTime)
                {
                    displayTime = gameTime;
                    isTimeOver = true;
                }
            }
            //Debug.Log("TIMES: " + displayTime);
        }
    }
}
