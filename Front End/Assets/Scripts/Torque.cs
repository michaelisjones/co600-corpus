using UnityEngine;
using UnityGPPhysics;

//<author>Georgina Perera, Robert McDonnell</author>
public class Torque : MonoBehaviour
{
    public Vector3 torque = new Vector3(10,5,10);
    private UnityGPPhysics.Rigidbody rb;

    // Allow for a rigidbody to have torque applied by replicating the rotation over time, through an impulse
    void Awake()
    {
        rb = GetComponent<UnityGPPhysics.Rigidbody>();

        rb.transform.eulerAngles = torque;

        //This would work, but Impulse was not implemented entirely correctly
        //rb.AddTorque(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * Time.fixedDeltaTime, UnityGPPhysics.ForceMode.Impulse);
        rb.transform.Rotate(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * Time.fixedDeltaTime);
    }
}
