using UnityEngine;

namespace UnityGPPhysics
{
	/// <summary>Describes a collision. Note: the interface of this class is based on the Unity Scripting API.</summary>
	/// <author>Liam Ireland</author>
	public class Collision
	{
		/// <summary>The collider we hit (Read Only).</summary>
		public Collider collider;
		/// <summary>The contact points. Every contact contains a contact point, normal and the two colliders.</summary>
		public ContactPoint[] contacts;
		/// <summary>The game object whose collider we are colliding with. (Read Only).</summary>
		public GameObject gameObject;
		/// <summary>The total impulse applied to this contact pair to resolve the collision.</summary>
		public Vector3 impulse;
		/// <summary>The relative linear velocity of the two colliding objects (Read Only).</summary>
		public Vector3 relativeVelocity;
		/// <summary>The Rigidbody we hit (Read Only). This is null if the object we hit is a collider with no rigidbody attached.</summary>
		public Rigidbody rigidbody;
		/// <summary>The Transform of the object we hit (Read Only).</summary>
		public Transform tranform;

		/// <summary>Constructor for a Collision.</summary>
		/// <param name="collider">The collider we hit (Read Only).</param>
		/// <param name="gameObject">The game object whose collider we are colliding with. (Read Only).</param>
		/// <param name="impulse">The total impulse applied to this contact pair to resolve the collision.</param>
		/// <param name="relativeVelocity">The relative linear velocity of the two colliding objects (Read Only).</param>
		/// <param name="rigidbody">The Rigidbody we hit (Read Only). This is null if the object we hit is a collider with no rigidbody attached.</param>
		/// <param name="tranform">The Transform of the object we hit (Read Only).</param>
		/// <author>Liam Ireland</author>
		public Collision(Collider collider, GameObject gameObject, Vector3 impulse, Vector3 relativeVelocity, Rigidbody rigidbody, Transform tranform)
		{
			this.collider = collider;
			this.gameObject = gameObject;
			this.impulse = impulse;
			this.relativeVelocity = relativeVelocity;
			this.rigidbody = rigidbody;
			this.tranform = tranform;
		}
	}
}
