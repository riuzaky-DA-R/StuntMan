using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public float Timer;
    public Text Timer_Display;
    public bool HasTimeEnded;
    // Start is called before the first frame update
    private void Awake()
    {
        Timer = 180;
        HasTimeEnded = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (Timer > 0)
        {
            Timer -= Time.fixedDeltaTime;
        }
        else 
        {
            HasTimeEnded = true;
            Timer = 0;
        }
        Timer_Display.text = Timer.ToString("F2");
        DisplayTime(Timer);
    }
    void DisplayTime(float TimeLeft)
    {
        float minutes = Mathf.FloorToInt(TimeLeft / 60);
        float seconds = Mathf.FloorToInt(TimeLeft % 60);
        Timer_Display.text = minutes.ToString() + ":" + seconds.ToString();
    }
}
