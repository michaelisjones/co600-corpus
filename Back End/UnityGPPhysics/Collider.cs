using System;
using UnityEngine;

namespace UnityGPPhysics
{
	/// <summary>An abstract class for all colliders to inherit from. Note: the interface of this class is based on the Unity Scripting API.</summary>
	/// <author>Liam Ireland, Michael Jones, Tomasz Mackow</author>
	public abstract class Collider : MonoBehaviour
	{
		/// <summary>The rigidbody the collider is attached to. Is null if the collider is not attached to a Rigidbody</summary>
		[HideInInspector]
		public Rigidbody attachedRigidbody;
		/// <summary>A box completely surrounding the object.</summary>
		[HideInInspector]
		public Bounds bounds;
		/// <summary>A trigger collider do not physically interact with other colliders.</summary>
		public bool isTrigger;
		/// <summary>The material used by the collider.</summary>
		public PhysicMaterial material;
		/// <summary>The physic material of this collider shared with other colliders.</summary>
		[HideInInspector]
		public PhysicMaterial sharedMaterial;

		protected bool collidingAlready; // TODO: remove
		protected bool collidedLastFixedUpdate; // TODO: remove

		/// <summary>The Awake function is called when the simulation starts.</summary>
		/// <author>Liam Ireland</author>
		public void Awake()
		{
			attachedRigidbody = GetComponent<Rigidbody>();
			bounds = GetComponent<Renderer>().bounds; // TODO: generate bounds if there is no renderer

			if (material == null)
				material = ScriptableObject.CreateInstance<PhysicMaterial>();
			sharedMaterial = material;
			if (material != null)
				material = Instantiate(material);
		}

		/// <summary>FixedUpdate message.</summary>
		public void FixedUpdate()
		{
			collidingAlready = collidedLastFixedUpdate; // TODO: remove
			collidedLastFixedUpdate = false; // TODO: remove
			bounds = GetComponent<Renderer>().bounds; // TODO: generate bounds if there is no renderer
		}

		///<summary>The closest point to the bounding box of the attached collider.</summary>
		/// <param name="position">A given point which needs its closest point on the bounds locating.</param>
		/// <author>Tomasz Mackow</author>
		public Vector3 ClosestPointOnBounds(Vector3 position)
		{
			return bounds.ClosestPoint(position);
		}

		/// <summary>Called when a collision is detected.</summary>
		/// <param name="collision">A collision object which holds all the details about a collision.</param>
		/// <param name="impulse">Resolved impulse on collider.</param>
		/// <param name="torque">Resolved torque on collider.</param>
		/// <author>Liam Ireland, Michael Jones</author>
		protected void collide(Collision collision, Vector3 impulse = default(Vector3),
		                         Vector3 torque = default(Vector3))
		{
			collidedLastFixedUpdate = true; // TODO: remove

			if (!collidingAlready) // TODO: remove check – requires fixing collider model
			{
				if (impulse != default(Vector3))
					attachedRigidbody.AddForce(impulse, ForceMode.Impulse);

				// TODO: add torque
				// if (torque != default(Vector3))
				// 	attachedRigidbody.AddTorque(torque, ForceMode.Impulse);
			}

			// TODO: send messages
		}

		/// <summary>Called when the collider is triggered.</summary>
		/// <param name="other">The other Collider involved in this collision.</param>
		/// <author>Michael Jones</author>
		protected void trigger(Collider other)
		{
			// TODO: send messages
		}
	}
}
