using UnityEngine;
using System.Linq;

namespace UnityGPPhysics
{
	/// <summary>A sphere-shaped primitive collider. Note: the interface of this class is based on the Unity Scripting API.</summary>
	/// <author>Tomasz Mackow, Michael Jones, Liam Ireland, Georgina Perera, Robert McDonnell</author>
	class SphereCollider : Collider
	{
		/// <summary>The center of the sphere in the object's local space.</summary>
		public Vector3 center = Vector3.zero;
		/// <summary>The radius of the sphere measured in the object's local space.</summary>
		public float radius = 0.5f;

		/// <summary></summary>
		/// <author>Michael Jones</author>
		public new void Awake()
		{
			base.Awake();

			center = transform.localPosition;
			float[] components = { transform.localScale.x, transform.localScale.y, transform.localScale.z };
			radius = components.Max() / 2;
		}

		/// <summary></summary>
		/// <author>Michael Jones, Liam Ireland, Robert McDonnell, Georgina Perera, Tomasz Mackow</author>
		public new void FixedUpdate()
		{
			base.FixedUpdate();

			center = transform.localPosition;
			float[] components = { transform.localScale.x, transform.localScale.y, transform.localScale.z };
			radius = components.Max() / 2;

			if (Physics.CheckSphere(bounds.center, radius))
			{
				Collider[] intersectingColliders = Physics.OverlapSphere(bounds.center, radius);

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
                                    Vector3 momentum = attachedRigidbody.velocity * attachedRigidbody.mass;

                                    // colliding with static (>>Currently not working<<)
                                    if (intersecting.attachedRigidbody == null)
									{
                                        Vector3 normal = default(Vector3);
                                        if (intersecting is SphereCollider)
                                        {
                                            normal = ((SphereCollider)intersecting).center - this.center;
                                        }
                                        resolvedImpulse = -Vector3.Project(momentum, normal);
                                        resolvedImpulse += resolvedImpulse * material.bounciness;

                                        if (attachedRigidbody.useGravity)
											resolvedImpulse -= new Vector3(0, Physics.gravity, 0) * Time.fixedDeltaTime * attachedRigidbody.mass;
									}
									// colliding with dynamic
									else
									{
										Vector3 normal = default(Vector3);
										if (intersecting is SphereCollider)
										{
											normal = ((SphereCollider)intersecting).center - this.center;
										}
										resolvedImpulse = -Vector3.Project(momentum, normal);
										resolvedImpulse += resolvedImpulse * material.bounciness;

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
