using UnityEngine;
using System.Collections;

/// <summary>Rotate the transform by a constant Vector3 (15, 30, 45) every update.</summary>
/// <author>Robert McDonnell, Georgina Perera</author>
public class Rotator : MonoBehaviour {

	//Adding rotation over time, necessary for torque
	void Update () {
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
	}
}
