
namespace UnityGPPhysics
{
	/// <summary>Describes how physical material properties of colliding objects are combined. Note: the interface of this class is based on the Unity Scripting API.</summary>
	/// <author>Michael Jones</author>
	public enum PhysicMaterialCombine
	{
		/// <summary>Averages the friction/bounce of the two colliding materials.</summary>
		Average,
		/// <summary>Uses the smaller friction/bounce of the two colliding materials.</summary>
		Minimum,
		/// <summary>Multiplies the friction/bounce of the two colliding materials.</summary>
		Multiply,
		/// <summary>Uses the larger friction/bounce of the two colliding materials.</summary>
		Maximum
	}
}
