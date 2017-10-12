using UnityEngine;
using UnityEngine.UI;
using UnityGPPhysics;
using System.Collections;

/// <summary>Script to apply to a slider to give slow motion</summary>
/// <author>Robert McDonnell</author>
public class SpeedSlider : MonoBehaviour
{
    public Slider slider;
    public Text text;
    private bool disable;

    void Update()
	{
        disable = GameObject.Find("ObjectForButton").GetComponent<PausePlay>().start;

        //Automatically set sliders and text labels
        slider = GameObject.Find("StartSpeed").GetComponent<Slider>();
        text = GameObject.Find("Speed").GetComponent<Text>();

        //Disable ability to move slider once particles have spawned
        if (!disable)
        {
            slider.interactable = false;
        }

        //Update the text to reflect the slider value
        text.text = slider.value.ToString();
        
    }
}