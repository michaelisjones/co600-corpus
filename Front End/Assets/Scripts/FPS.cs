using UnityEngine;
using UnityEngine.UI;
using UnityGPPhysics;
using System.Collections;

/// <summary>Script to make frame per second visible</summary>
/// <author>Georgina Perera</author>
public class FPS : MonoBehaviour
{
    //Calculations based on https://www.youtube.com/watch?v=QQN0cuw7BxE
    //Way of displaying to Gui changed
    public Text text;
    private const float FPS_UPDATE_INTERVAL = 0.02f;
    private float fpsAccum = 0;
    private int fpsFrames = 0;
    private float fpsTimeLeft = FPS_UPDATE_INTERVAL;
    private float fps = 0;

    //Calculates the time left in each frame based of each delta time step
    //then counts up the Frames done in each second and displays the value to
    //a text field in the GUI.
    void Update()
    {
        fpsTimeLeft -= Time.deltaTime;
        fpsAccum += Time.timeScale / Time.deltaTime;
        fpsFrames++;

        if (fpsTimeLeft <= 0)
        {
            fps = fpsAccum / fpsFrames;
            fpsTimeLeft = FPS_UPDATE_INTERVAL;
            fpsAccum = 0;
            fpsFrames = 0;
        }
        text = GameObject.Find("FPS").GetComponent<Text>();
        text.text = fps.ToString();
    }
}