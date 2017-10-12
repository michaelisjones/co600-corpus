using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGPPhysics;

/// <summary>Player controller script for roll-a-ball example scene.</summary>
public class PlayerController : MonoBehaviour {

	public float speed;
	public Text countText;
	public Text winText;

	private UnityGPPhysics.Rigidbody rb;
	private int count;

	void Start ()
	{
		rb = GetComponent<UnityGPPhysics.Rigidbody>();
		count = 0;
		SetCountText ();
		winText.text = "";
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);
	}

	void OnTriggerEnter (UnityEngine.Collider other)
	{
		if (other.gameObject.CompareTag("Pick Up"))
		{
			other.gameObject.SetActive (false);
			count++;
			SetCountText ();
		}
	}

	void SetCountText ()
	{
		countText.text = "Count: " + count.ToString ();
		if (count >= 8)
		{
			winText.text = "You win!";
		}
	}
}
