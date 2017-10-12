using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace UnityGPPhysics
{
	/// <summary>A static class containing global physics constants and methods. Note: the interface of this class is based on the Unity Scripting API.</summary>
	/// <author>Liam Ireland, Michael Jones</author>
	public static class Physics
	{
		/// <summary>Two colliding objects with a relative velocity below this will not bounce (default 2). Must be positive.</summary>
		public static float bounceThreshold = 2.0f; // TODO: implement
		/// <summary>The default contact offset of the newly created colliders.</summary>
		public static float defaultContactOffset = 0.0f; // TODO: implement
        /// <summary>The defaultSolverIterations determines how accurately Rigidbody joints and collision contacts are resolved. (default 6). Must be positive.</summary>
		public static int defaultSolverIterations = 6; // TODO: implement
        /// <summary>The defaultSolverVelocityIterations affects how how accurately Rigidbody joints and collision contacts are resolved. (default 1). Must be positive.</summary>
        public static int defaultSolverVelocityIterations = 1; // TODO: implement
        /// <summary>The gravity applied to all rigid bodies in the scene.</summary>
		public static float gravity = -9.81f;
		/// <summary>Specifies whether queries (raycasts, spherecasts, overlap tests, etc.) hit Triggers by default.</summary>
		public static bool queriesHitTriggers = true;
		/// <summary>The mass-normalized energy threshold, below which objects start going to sleep.</summary>
		public static float sleepThreshold = 0f;

		// all colliders in the scene, updated no more than once per fixed update
		private static List<Collider> allColliders = new List<Collider>();
		// Time.fixedTime of last allColliders list update
		private static float allCollidersFixedTime;

		/// <summary>Returns true if the input box overlaps with any colliders.</summary>
		/// <param name="center">Centre of the box.</param>
		/// <param name="halfExtents">The half extension of the box (this is half the size).</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
		/// <returns>True if the input box overlaps with any colliders.</returns>
		/// <author>Liam Ireland</author>
		public static bool CheckBox(Vector3 center, Vector3 halfExtents,
		                            Quaternion orientation = default(Quaternion), // TODO: implement orientation
									QueryTriggerInteraction queryTriggerInteraction = default(QueryTriggerInteraction))
		{
			updateAllColliders();

			Bounds bounds = new Bounds(center, halfExtents * 2);
			bool trigger = queriesHitTriggers;
			switch (queryTriggerInteraction)
			{
				case QueryTriggerInteraction.UseGlobal:
					trigger = queriesHitTriggers;
					break;
				case QueryTriggerInteraction.Collide:
					trigger = true;
					break;
				case QueryTriggerInteraction.Ignore:
					trigger = false;
					break;
			}

			foreach (Collider collider in allColliders)
			{
				if (!collider.bounds.Equals(bounds) && bounds.Intersects(collider.bounds)
				    && !(collider.isTrigger && !trigger))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>Returns an array of colliders that intersect with the input box.</summary>
		/// <param name="center">Centre of the box.</param>
		/// <param name="halfExtents">The half extension of the box (this is half the size).</param>
		/// <param name="orientation">Orientation.</param>
		/// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
		/// <returns>An array containing all the colliders in the input box.</returns>
		/// <author>Liam Ireland</author>
		public static Collider[] OverlapBox(Vector3 center, Vector3 halfExtents,
											Quaternion orientation = default(Quaternion), // TODO: implement orientation
											QueryTriggerInteraction queryTriggerInteraction = default(QueryTriggerInteraction))
		{
			updateAllColliders();

			List<Collider> overlappingColliders = new List<Collider>();

			Bounds bounds = new Bounds(center, halfExtents * 2);
			bool trigger = queriesHitTriggers;
			switch (queryTriggerInteraction)
			{
				case QueryTriggerInteraction.UseGlobal:
					trigger = queriesHitTriggers;
					break;
				case QueryTriggerInteraction.Collide:
					trigger = true;
					break;
				case QueryTriggerInteraction.Ignore:
					trigger = false;
					break;
			}

			foreach (Collider collider in allColliders)
			{
				if (collider.bounds.Intersects(bounds) && !(collider.isTrigger && !trigger))
				{
					overlappingColliders.Add(collider);
				}
			}

			return overlappingColliders.ToArray();
		}

		/// <summary>Returns true if there are any colliders overlapping the sphere defined by position and radius in world coordinates.</summary>
		/// <param name="position">Center of the sphere.</param>
		/// <param name="radius">Radius of the sphere.</param>
		/// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
		/// <returns>True if there are any colliders overlapping the sphere defined by position and radius in world coordinates.</returns>
		/// <author>Michael Jones, Liam Ireland</author>
		public static bool CheckSphere(Vector3 position, float radius,
		                               QueryTriggerInteraction queryTriggerInteraction = default(QueryTriggerInteraction))
		{
			updateAllColliders();

			bool collideWithTriggers = queriesHitTriggers;
			switch (queryTriggerInteraction)
			{
				case QueryTriggerInteraction.UseGlobal:
					collideWithTriggers = queriesHitTriggers;
					break;
				case QueryTriggerInteraction.Collide:
					collideWithTriggers = true;
					break;
				case QueryTriggerInteraction.Ignore:
					collideWithTriggers = false;
					break;
			}

			foreach (Collider collider in allColliders)
			{
				if (!collider.isTrigger && collideWithTriggers) // TODO: check sphere
				{
					if (collider is SphereCollider)
					{
						if (Vector3.Distance(position, collider.bounds.center) < radius + collider.bounds.extents.y)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		/// <summary>Returns an array with all colliders touching or inside the sphere.</summary>
		/// <param name="position">Center of the sphere.</param>
		/// <param name="radius">Radius of the sphere.</param>
		/// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
		/// <returns>An array with all colliders touching or inside the sphere.</returns>
		/// <author>Michael Jones, Liam Ireland</author>
		public static Collider[] OverlapSphere(Vector3 position, float radius,
		                                       QueryTriggerInteraction queryTriggerInteraction = default(QueryTriggerInteraction))
		{
			updateAllColliders();

			List<Collider> overlappingColliders = new List<Collider>();

			bool collideWithTriggers = queriesHitTriggers;
			switch (queryTriggerInteraction)
			{
				case QueryTriggerInteraction.UseGlobal:
					collideWithTriggers = queriesHitTriggers;
					break;
				case QueryTriggerInteraction.Collide:
					collideWithTriggers = true;
					break;
				case QueryTriggerInteraction.Ignore:
					collideWithTriggers = false;
					break;
			}

			foreach (Collider collider in allColliders)
			{
				if (!(collider.isTrigger && !collideWithTriggers)) // TODO: check sphere
				{
					if (collider is SphereCollider)
					{
						if (Vector3.Distance(position, collider.bounds.center) < radius + collider.bounds.extents.y)
						{
							overlappingColliders.Add(collider);
						}
					}
				}
			}

			return overlappingColliders.ToArray();
		}


		/// <summary>Update allColliders if we are in the next fixedUpdate.</summary>
		/// <author>Michael Jones</author>
		private static void updateAllColliders()
		{
			if (Time.fixedTime > allCollidersFixedTime)
			{
				allCollidersFixedTime = Time.fixedTime;

				List<GameObject> rootGameObjects = new List<GameObject>();
				SceneManager.GetActiveScene().GetRootGameObjects(rootGameObjects);

				allColliders = new List<Collider>(); // TODO: try modifying existing list rather than rebuilding it

				foreach (GameObject gameObject in rootGameObjects)
				{
					foreach (Collider collider in gameObject.GetComponentsInChildren<Collider>())
					{
						allColliders.Add(collider);
					}
				}
			}
		}
	}
}
