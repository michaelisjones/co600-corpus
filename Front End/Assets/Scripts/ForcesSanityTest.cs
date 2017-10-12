using UnityEngine;
using System.Collections;

/// <summary>Sanity test for Rigidbody.AddForce. This class can be used to test that adding the same force to a rigidbody every fixed update will result in the same behaviour regardless of time scale.</summary>
public class ForcesSanityTest : MonoBehaviour {

    [Tooltip("N")]
    public Vector3 force;

    Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Time.time > 3)
        {
            Debug.Log(transform.localPosition);
            Object.Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(force, ForceMode.Force);
    }
}
