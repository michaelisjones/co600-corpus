using UnityEngine;
using UnityEngine.UI;
using UnityGPPhysics;
using UnityGPShockwaves;
using System.Collections;

/// <summary>Script to switch button from play to pause and back on click</summary>
/// <author>Robert McDonnell, Georgina Perera</author>
public class PausePlay : MonoBehaviour
{
    private Button button;
    public bool active;
    public bool start = true;

    void Start()
    {
        active = false;
        //Automatically set Button and text label
        button = GameObject.Find("Play").GetComponent<Button>();
    }


    //Method to be called from empty game object to activate on click
    //Changes the button text to either Pause or Play
    public void OnClick()
    {
        if (active  == false )//Playing
        {
            play();
            active = true;
        }
        else //Paused
        {
            pause();
            active = false;
        }
    }

    //Set input value for slider from var in memory and change icon
    private void play()
    {
        button.GetComponentInChildren<Text>().text = "ll";
    }

    //Store current input value from slider into memory, stop time,
    //change icon
    private void pause()
    {
        button.GetComponentInChildren<Text>().text = "►";
    }

    //Reset variables to defaults for scene reload
    public void reset()
    {
        start = true;
        pause();
        active = false;
    }
}