using UnityEngine;

namespace UnityGPPhysics
{
	/// <summary>Describes the contact point of collision. Note: the interface of this class is based on the Unity Scripting API.</summary>
	/// <author>Tomasz Mackow</author>
	public struct ContactPoint
	{
		/// <summary>The normal of the contact point.</summary>
		public Vector3 normal;
		/// <summary>The other collider in contact at the point.</summary>
		public Collider otherCollider;
		/// <summary>The point of contact.</summary>
		public Vector3 point;
		/// <summary>The separation distance between the colliders and the contact point.</summary>
		public float separation;
		/// <summary>The first collider in contact at the point.</summary>
		public Collider thisCollider;
	}
}
