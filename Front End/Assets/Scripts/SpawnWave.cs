using UnityEngine;
using UnityEngine.UI;
using UnityGPPhysics;
using UnityGPShockwaves;
using System.Collections;

/// <summary>Script to switch button from play to pause and back on click</summary>
/// <author>Robert McDonnell, Georgina Perera</author>
public class SpawnWave : MonoBehaviour
{
    private GameObject scene;
    private Vector3 point;

    //Spawns a shockwavehandler, which is updated with the inital speed and spawn point
    //obtained through the click and sliders in the front end
    public void spawn()
    {
        ShockWaveHandler handler = GameObject.Find("Shock Wave Handler").GetComponent<ShockWaveHandler>();
        handler.shockWave.Setup();
        
        point = Camera.main.GetComponent<Raycast>().lastHit;
        handler.shockWave.initialSpeed = GameObject.Find("StartSpeed").GetComponent<SpeedHandler>().slider;
        handler.shockWave.Setup();
        handler.shockWave.Spawn(point);
    }    
}