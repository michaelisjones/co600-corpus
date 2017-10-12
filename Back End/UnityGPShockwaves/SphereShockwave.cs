using UnityEngine;

namespace UnityGPShockwaves
{
	/// <summary>A shock wave represented using spheres.</summary>
	/// <author>Liam Ireland, Tomasz Mackow</author>
	public class SphereShockwave : MonoBehaviour
	{
        /// <summary>The sphere collider respresenting the shockwave.</summary>
        private SphereCollider sphereCollider;
        private RaycastHit[] raycasthits;
        private float force = 100;

        /// <summary>This function is called when the scene is started.</summary>
        /// <author>Liam Ireland</author>
        public void Awake()
        {
            sphereCollider = gameObject.AddComponent<SphereCollider>() as SphereCollider;
            sphereCollider.center = transform.position;
            sphereCollider.radius = 0;
            sphereCollider.isTrigger = true;
        }

        /// <summary>Called every fixed framerate frame.</summary>
        /// <author>Liam Ireland, Tomasz Mackow</author>
        public void FixedUpdate()
        {
            sphereCollider.radius += 300F*Time.fixedDeltaTime; // 300m/s
        }

        /// <summary>Called when a collision is detected.</summary>
        /// <param name="other">The collider we're colliding with</param>
        /// <author>Liam Ireland, Tomasz Mackow</author>
        public void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody != null) // if the collider we're colliding with has a rigidbody i.e. it isn't the floor or a wall
            {

                Debug.Log("Contact was made!");

                bool wallInbetween = false;

                raycasthits = Physics.RaycastAll(new Ray(sphereCollider.center, other.transform.position - sphereCollider.center));

                foreach (RaycastHit raycasthit in raycasthits)
                {
                    if (raycasthit.collider != sphereCollider && raycasthit.collider != other)
                    {
                        if (raycasthit.collider.attachedRigidbody == null) // isolate dem walls
                        {
                            wallInbetween = true;
                            break; // break out da loop
                        }
                    }
                }

                if (wallInbetween)
                {
                    other.attachedRigidbody.AddExplosionForce(0, sphereCollider.center, sphereCollider.radius * 2, 0, ForceMode.Impulse);
                }
                else
                {
                    other.attachedRigidbody.AddExplosionForce(force, sphereCollider.center, sphereCollider.radius * 2, 0, ForceMode.Impulse);
                }
            }
        }
    }
}