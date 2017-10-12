using UnityEngine;
using System.Collections;

/// <author>Georgina Perera, Robert McDonnell</author>

public class Raycast : MonoBehaviour {

    public Vector3 lastHit;
	
	//Gets position of last surface raycast from click hits
    //Ignores anything on layer 8 (Ceiling)
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int ignore = ~(1 << 8);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ignore))
                lastHit = hit.point;
        }
	}
}
