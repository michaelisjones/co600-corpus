using UnityEngine;
using System.Collections;

//ORIGINAL CODE OBTAINED FROM: https://www.youtube.com/watch?v=z7eojB_1wKg
//EDITED TO FIT NEEDS AND IS NOW COMPLETELY DIFFERENT


//<author>Robert McDonnell<author>
public class InputManager : MonoBehaviour
{
    CameraOrbit cam;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<CameraOrbit>();
    }

    //Key press listeners
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            cam.MoveHorizontal(false);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            cam.MoveHorizontal(true);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            cam.MoveVertical(true);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            cam.MoveVertical(false);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            cam.MoveFocusHorizontal(true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            cam.MoveFocusHorizontal(false);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            cam.MoveFocusVertical(true);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            cam.MoveFocusVertical(false);
        }
    }
}
