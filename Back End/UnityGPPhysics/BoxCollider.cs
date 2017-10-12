using UnityEngine;

namespace UnityGPPhysics
{
	/// <summary>Box collider. Note: the interface of this class is based on the Unity Scripting API.</summary>
	/// <author>Liam Ireland, Georgina Perera, Robert McDonnell</author>
	class BoxCollider : Collider
	{
		/// <summary>The coordinates for the centre of the box.</summary>
		public Vector3 center = Vector3.zero;
		/// <summary>The size of the box.</summary>
		public Vector3 size = Vector3.one;

		/// <summary>The Awake function is called when the simulation starts.</summary>
		/// <author>Liam Ireland</author>
		public new void Awake()
		{
			base.Awake();

			center = transform.localPosition;
			size = transform.localScale;

			// TODO: generate bounds
			//Vector3 worldScale = transform.localScale;
			//Transform p = transform.parent;
			//while (p != null)
			//{
			//	worldScale = Vector3.Scale(worldScale, p.localScale);
			//	p = p.parent;
			//}

			//bounds = new Bounds(transform.position, worldScale);
		}

		/// <summary>FixedUpdate message.</summary>
		/// <author>Michael Jones, Liam Ireland, Robert McDonnell, Georgina Perera, Tomasz Mackow</author>
		public new void FixedUpdate()
		{
			base.FixedUpdate();

			center = transform.localPosition;
			size = transform.localScale;

			// TODO: update bounds size using world scale
			//Vector3 worldScale = transform.localScale;
			//Transform parent = transform.parent;
			//while (parent != null)
			//{
			//	worldScale = Vector3.Scale(worldScale, parent.localScale);
			//	parent = parent.parent;
			//}
			//
			//if (bounds.center != transform.position)
			//if (bounds.center != transform.position || bounds.size != worldScale)
			//{
			//	bounds.center = transform.position;
			//	bounds.size = worldScale;
			//	bounds.extents = bounds.size / 2;
			//	bounds.max = bounds.center + bounds.extents;
			//	bounds.min = bounds.center - bounds.extents;
			//}

			if (Physics.CheckBox(bounds.center, bounds.extents))
			{
				Collider[] intersectingColliders = Physics.OverlapBox(bounds.center, bounds.extents);

				foreach (Collider intersecting in intersectingColliders)
				{
					if (!intersecting.Equals(this)) // don't collide with self
					{
						if (!isTrigger)
						{
							// don't collide if both static
							if (!(attachedRigidbody == null && intersecting.attachedRigidbody == null))
							{
								// new Collision object
								Vector3 collisionImpulse = Vector3.zero;
								Vector3 collisionVelocity = Vector3.zero;
								if (attachedRigidbody != null)
								{
									collisionImpulse = attachedRigidbody.velocity * attachedRigidbody.mass;
									collisionVelocity = attachedRigidbody.velocity;
								}
								if (intersecting.attachedRigidbody != null)
								{
									collisionImpulse += intersecting.attachedRigidbody.velocity * intersecting.attachedRigidbody.mass;
									collisionVelocity -= intersecting.attachedRigidbody.velocity;
								}
								Collision collision = new Collision(intersecting, intersecting.gameObject, collisionImpulse, collisionVelocity,
																	intersecting.attachedRigidbody, intersecting.transform);

								// dynamic collider
								if (attachedRigidbody != null)
								{
									Vector3 resolvedImpulse = Vector3.zero;

									// colliding with static
									if (intersecting.attachedRigidbody == null)
									{
										resolvedImpulse = -attachedRigidbody.velocity * attachedRigidbody.mass
																			- (attachedRigidbody.velocity * attachedRigidbody.mass
																			   * material.bounciness);

										if (attachedRigidbody.useGravity)
											resolvedImpulse -= new Vector3(0, Physics.gravity, 0) * Time.fixedDeltaTime * attachedRigidbody.mass;
									}
									// colliding with dynamic
									else
									{
										resolvedImpulse = -attachedRigidbody.velocity * attachedRigidbody.mass
																			- (attachedRigidbody.velocity * attachedRigidbody.mass
																			   * material.bounciness);

										if (attachedRigidbody.useGravity)
											resolvedImpulse -= new Vector3(0, Physics.gravity, 0) * Time.fixedDeltaTime * attachedRigidbody.mass;
									}

									collide(collision, resolvedImpulse);
								}

								collide(collision);
							}
						}
						else
						{
							trigger(intersecting);
						}
					}
				}
			}
		}
	}
}
