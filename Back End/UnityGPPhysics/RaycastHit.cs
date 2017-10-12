using UnityEngine;

namespace UnityGPPhysics
{
	/// <summary>Structure used to get information back from a raycast. Note: the interface of this class is based on the Unity Scripting API.</summary>
	/// <author>Michael Jones, Tomasz Mackow</summary>
	public struct RaycastHit
	{
		/// <summary>The barycentric coordinate of the triangle we hit.</summary>
		public Vector3 barycentricCoordinate;
		/// <summary>The Collider that was hit</summary>
		public Collider collider;
		/// <summary>The distance from the ray's origin to the impact point.</summary>
		public float distance;
		/// <summary>The normal of the surface the ray hit.</summary>
		public Vector3 normal;
		/// <summary>The impact point in world space where the ray hit the collider.</summary>
		public Vector3 point;
		/// <summary>The Rigidbody of the collider that was hit. If the collider is not attached to a rigidbody then it is null.</summary>
		public Rigidbody rigidbody;
		/// <summary>The Transform of the rigidbody or collider that was hit.</summary>
		public Transform transform;
		/// <summary>The index of the triangle that was hit.</summary>
		public int triangleIndex;
	}
}
