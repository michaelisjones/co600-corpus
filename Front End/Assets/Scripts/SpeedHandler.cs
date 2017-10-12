using UnityEngine;
using UnityEngine.UI;
using UnityGPPhysics;
using System.Collections;

/// <summary>Script to apply to a slider to give slow motion</summary>
/// <author>Robert McDonnell</author>
public class SpeedHandler : MonoBehaviour
{
	public float slider;

	public void Update()
	{
		slider = GameObject.Find("StartSpeed").GetComponent<Slider>().value;
	}
}