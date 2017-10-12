using UnityEngine;
using System.Collections;

/// <summary>Camtera controller script for roll-a-ball example scene.</summary>
public class CameraController : MonoBehaviour {

	/// <summary>The player object.</summary>
	public GameObject player;

	private Vector3 offset;

	void Start () {
		offset = transform.position - player.transform.position;
	}

	void LateUpdate () {
		transform.position = player.transform.position + offset;
	}
}
