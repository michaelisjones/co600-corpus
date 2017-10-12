using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//<author>Georgina Perera</author>
public class Overlay : MonoBehaviour {

	public GameObject window;
	public Text messageField;

    //Changes attribute of window gameobject to either display or hide 
    public void Show() {
		window.SetActive(true);
    }

	public void Hide() {
		window.SetActive (false);
	}
}
