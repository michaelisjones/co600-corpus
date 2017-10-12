using UnityEngine;
using UnityEngine.UI;
using UnityGPPhysics;
using System.Collections;

/// <summary>Script to apply to a slider to give slow motion</summary>
/// <author>Robert McDonnell</author>
public class TimeSlider : MonoBehaviour
{
    public Slider slider;
    public Text text;

    void Update()
    {

        //Automatically set sliders and text labels
        slider = GameObject.Find("Slider").GetComponent<Slider>();
        text = GameObject.Find("Text").GetComponent<Text>();

        //Update the text to reflect the slider value
        text.text = slider.value.ToString();

        GameObject.Find("ObjectForButton").GetComponent<TimeHandler>().UpdateTime();
    }
}