using UnityEngine;
using System.Collections;

//<author>Robert McDonnell</author>
public class CameraOrbit : MonoBehaviour
{
    //Vector for camera focal point
    public Vector3 target = Vector3.zero;

    //Increments for how much the camera moves each frame
    public float hMove = 1f;
    public float vMove = 1f;

    //Move Camera left/right
    public void MoveHorizontal(bool left)
    {
        float dir = 1;
        //To make it go right, multiply left (positive) vector by -1.
        if (!left) dir *= -1;
        transform.RotateAround(transform.position, Vector3.up, hMove * dir);

    }

    //Move camera up/down
    public void MoveVertical(bool up)
    {
        float xVal = transform.eulerAngles.x;
        float dir = 1;
        //To make it go down, multiply up by -1
        if (!up) dir *= -1;

        //Restrict movement to at least 3 degrees above floor
        //Checks if next press would go beneath limit
        //doesn't allow movement if button press is going down
        //allows if button press is going up.

            if (up)
            {
                MoveAll(dir, xVal);
            }
        //Restrict movement to at most 90 degrees above floor
        //Checks if next press would go above limit
        //does inverse of above
            if (!up)
            {
                MoveAll(dir, xVal);
            }
            //All other orbital movement
            MoveAll(dir, xVal);
        
    }


    //Allows camera to oribit origin point
    //Sets x coordinate to allow for checking in MoveVertical();
    public void MoveAll(float dir, float xVal)
    {
        transform.RotateAround(transform.position, transform.TransformDirection(Vector3.right), vMove * dir);
        xVal = transform.eulerAngles.x;
    }

    //Allows the camera to move horizontally
    public void MoveFocusHorizontal(bool left)
    {
        float dir = 1;
        if (!left) dir *= -1;
        //Set the distance for the camera to move by
        Vector3 shift = new Vector3(-0.25F, 0, 0) * dir;
        //Move camera
        transform.Translate(shift);
    }

    //Allows the camera to move vertically
    public void MoveFocusVertical(bool up)
    {
        float dir = 1;
        if (!up) dir *= -1;
        //Set the distance for the target to move by
        Vector3 shift = new Vector3(0, 0, -0.25F) * dir;
        //Move camera
        transform.Translate(shift);
        //Move target
        target = target + shift;
    }

    public void ZoomIn()
    {
        //Set the distance for the target to move by
        Vector3 shift = new Vector3(0, 0, 0.25F);
        //Move camera
        transform.Translate(shift);
    }

    public void ZoomOut()
    {
        //Set the distance for the target to move by
        Vector3 shift = new Vector3(0, 0, -0.25F);
        //Move camera
        transform.Translate(shift);
    }
}


///OLD WASD MOVEMENT///
//THIS ISN'T RIGHT - The camera needs to move by an angle based on the amount that the focal target moves
//At the moment it just moves by the same distance that the target moves
/*public void MoveFocusHorizontal(bool left)
{
    float dir = 1;
    if (!left) dir *= -1;
    //Set the distance for the target to move by
    Vector3 shift = new Vector3(-0.25F, 0, 0) * dir;
    //Move camera
    transform.Translate(shift);
    //Move target
    target = target + shift;
}
//THIS ISN'T RIGHT - The camera needs to move by an angle based on the amount that the focal target moves
//At the moment it just moves by the same distance that the target moves
public void MoveFocusVertical(bool left)
{
    float dir = 1;
    if (!left) dir *= -1;
    //Set the distance for the target to move by
    Vector3 shift = new Vector3(0, -0.25F, 0) * dir;
    //Move camera
    transform.Translate(shift);
    //Move target
    target = target + shift;
}*/
