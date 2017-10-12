
namespace UnityGPPhysics
{
	/// <summary>Overrides the global Physics.queriesHitTriggers. Note: the interface of this class is based on the Unity Scripting API.</summary>
	/// <author>Michael Jones</author>
	public enum QueryTriggerInteraction
	{
		/// <summary>Queries use the global Physics.queriesHitTriggers setting.</summary>
		UseGlobal,
		/// <summary>Queries never report Trigger hits.</summary>
		Ignore,
		/// <summary>Queries always report Trigger hits.</summary>
		Collide
	}
}
