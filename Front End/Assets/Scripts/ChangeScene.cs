using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


/// <summary>Script to apply to game object to set default resolution and
/// handle scene loading as well as quit</summary>
/// <author>Robert McDonnell</author>
public class ChangeScene : MonoBehaviour
{
    void Start()
    {
        Screen.SetResolution(1366,768, true, 60);
    }

    //Checks for Esc key, takes user back to main menu if pressed
    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    //Loads next screen based on button selected and destroys current scene
    public void Load(string levelName)
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    //Clean up scene
    //Removes all objects, keeping GUI and camera positions
    public void ResetScene(GameObject prefab)
    {
        Destroy(GameObject.Find("Scene"));
        Destroy(GameObject.Find("Scene(Clone)"));

        //Resets spawned shock wave
        GameObject handler = GameObject.FindGameObjectWithTag("ShockWaveHandler");
        handler.GetComponent<ShockWaveHandler>().shockWave.Reset();

        //Reset pause button
        GameObject play = GameObject.Find("Play");
        PausePlay script = play.GetComponent<PausePlay>();
        script.reset();

        Instantiate(prefab);
    }

    //Exits application
    public void QuitClick()
    {
        Application.Quit();
    }
}