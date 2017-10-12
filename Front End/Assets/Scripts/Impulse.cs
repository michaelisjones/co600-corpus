using UnityEngine;
using UnityGPPhysics;

/// <summary>Add the given impulse to the attached Rigidbody upon awake.</summary>
///<author>Michael Jones</author>
public class Impulse : MonoBehaviour {

	/// <summary>The impulse vector to apply.</summary>
	public Vector3 impulse;

	private UnityGPPhysics.Rigidbody rb;

	void Awake () {
		rb = GetComponent<UnityGPPhysics.Rigidbody>();
		rb.AddForce(impulse, UnityGPPhysics.ForceMode.Impulse);
	}
}
