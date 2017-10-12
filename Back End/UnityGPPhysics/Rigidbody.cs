using UnityEngine;
using System.Collections.Generic;

namespace UnityGPPhysics
{
	/// <summary>A Rigidbody component handles forces applied to an object. Note: the interface of this class is based on the Unity Scripting API.</summary>
	/// <author>Liam Ireland, Michael Jones</author>
	public class Rigidbody : MonoBehaviour
	{
		/// <summary>The mass of the Rigidbody</summary>
		public float mass = 1;
		/// <summary>The velocity of the Rigidbody</summary>
		[HideInInspector]
		public Vector3 velocity;
		/// <summary>The mass-normalized energy threshold, below which objects start going to sleep.</summary>
		public float sleepThreshold = Physics.sleepThreshold;
		/// <summary>True if you want gravity applied to the object.</summary>
		public bool useGravity = true;
		/// <summary>Drag is used to slow down an object. The higher the drag, the more it will slow down.</summary>
		public float drag = 0;
		/// <summary>The density of the fluid the object is in. e.g. air</summary>
		public float densityOfFluid = 1.225f; // density of air (kg/m^3);

		private bool sleeping = false;
		/// <summary>A variable which describes how aerodynamic an object is. Used in calculating drag.</summary>
		private float dragCoefficient = 0.50f; // 0.5 by default, possibly to change later.
		// TODO: implement drag

		/// <summary>Adds a force to a Rigidbody</summary>
		/// <param name="force">Force vector in world coordinates.</param>
		/// <param name="mode">Type of force to apply.</param>
		/// <author>Liam Ireland, Michael Jones, Robert McDonnell</author>
		public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
		{
			switch (mode)
			{
				case ForceMode.Force:
					velocity += force / mass * Time.fixedDeltaTime;
					break;
				case ForceMode.Acceleration:
					velocity += force * Time.fixedDeltaTime;
					break;
				case ForceMode.Impulse:
					velocity += force / mass;
					break;
				case ForceMode.VelocityChange:
					velocity += force;
					break;
			}
		}

		/// <summary>Adds a force to a Rigidbody</summary>
		/// <param name="x">Size of force along the world x-axis.</param>
		/// <param name="y">Size of force along the world y-axis.</param>
		/// <param name="z">Size of force along the world z-axis.</param>
		/// <param name="mode">Type of force to apply.</param>
		/// <author>Michael Jones</author>
		public void AddForce(float x, float y, float z, ForceMode mode = ForceMode.Force)
		{
			AddForce(new Vector3(x, y, z), mode);
		}

		/// <summary>Calculate the drag</summary>
		/// <author>Liam Ireland</author>
		private void changeDrag()
		{
			// TODO: drag = 0.5 * dragCoefficient * densityOfFluid * referenceArea * velocity2
		}

		/// <summary>FixedUpdate is called every fixed framerate frame.</summary>
		/// <author>Liam Ireland, Michael Jones</author>
		public void FixedUpdate()
		{
			if (useGravity)
				velocity.y += Physics.gravity * Time.deltaTime;

			sleeping = velocity.magnitude <= sleepThreshold;

			if(!sleeping)
				transform.Translate(velocity * Time.deltaTime);
		}

		/// <summary>True if a rigidbody is not moving.</summary>
		/// <returns>True if the rigidbody is not moving.</returns>
		/// <author>Liam Ireland, Michael Jones</author>
		public bool IsSleeping()
		{
			return sleeping;
		}

		/// <summary>Wake me up inside.</summary>
		public void WakeUp() // can't wake up
		{
			sleeping = false;
		}
	}
}
