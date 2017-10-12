
namespace UnityGPPhysics
{
	/// <summary>Option for how to apply a force using Rigidbody.AddForce. Note: the interface of this class is based on the Unity Scripting API.</summary>
	/// <author>Liam Ireland</author>
	public enum ForceMode
	{
		/// <summary>Add a continuous force to the rigidbody, using its mass. (N)</summary>
		Force,
		/// <summary>Add a continuous acceleration to the rigidbody, ignoring its mass. (m/s²)</summary>
		Acceleration,
		/// <summary>Add an instant force impulse to the rigidbody, using its mass. (Ns)</summary>
		Impulse,
		/// <summary>Add an instant velocity change to the rigidbody, ignoring its mass. (m/s)</summary>
		VelocityChange
	}
}
