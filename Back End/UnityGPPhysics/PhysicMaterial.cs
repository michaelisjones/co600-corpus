using System;
using UnityEngine;

namespace UnityGPPhysics
{
	/// <summary>Physical material properties (friction, bounciness). Note: the interface of this class is based on the Unity Scripting API.</summary>
	/// <author>Michael Jones</author>
	[CreateAssetMenu(menuName = "Physic Material (GP)", fileName = "New Physic Material")]
	public class PhysicMaterial : ScriptableObject
	{
		/// <summary>Determines how the bounciness is combined.</summary>
		public PhysicMaterialCombine bounceCombine;
		/// <summary>How bouncy is the surface? A value of 0 will not bounce. A value of 1 will bounce without any loss of energy.</summary>
		public float bounciness;
		/// <summary>The friction used when already moving. This value has to be between 0 and 1.</summary>
		public float dynamicFriction = 0.6f;
		/// <summary>Determines how the friction is combined.</summary>
		public PhysicMaterialCombine frictionCombine;
		/// <summary>The friction coefficient used when an object is lying on a surface.</summary>
		public float staticFriction = 0.6f;
	}
}
