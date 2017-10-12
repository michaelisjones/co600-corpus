using UnityEngine;
using UnityEngine.UI;
using UnityGPPhysics;
using System.Collections;

/// <summary>Script to apply to a slider to give slow motion</summary>
/// <author>Robert McDonnell</author>
public class TimeHandler : MonoBehaviour
{
    private float slider;
    private bool buttonState;
    private float initFixedDeltaTime;
    private float initTimeScale;

    void Awake()
    {
        // allow changing fixed timestep in editor
        initFixedDeltaTime = Time.fixedDeltaTime;
        initTimeScale = Time.timeScale;

        Time.timeScale = 0;
    }


    //Constantly change time based on slider
    public void UpdateTime()
    {
        buttonState = GameObject.Find("ObjectForButton").GetComponent<PausePlay>().active;
        slider = GameObject.Find("Slider").GetComponent<Slider>().value;

        if (buttonState == true)
        {
            Time.timeScale = slider;
            float factor = initTimeScale / slider;
            Time.fixedDeltaTime = initFixedDeltaTime / factor;
        }
        else //paused
        {
            Time.timeScale = 0;
        }
    }
}
