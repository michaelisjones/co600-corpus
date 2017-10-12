using UnityEngine;
using UnityEngine.Assertions;

namespace UnityGPShockwaves
{
    /// <summary>Stores properties of a wave medium, defaults to properties of air at Earth sea-level.</summary>
    /// <author>Michael Jones, Tomasz Mackow</author>
    [CreateAssetMenu(menuName = "Shock Wave Medium", fileName = "New Shock Wave Medium")]
	public class ShockwaveMedium : ScriptableObject
	{
		/// <summary>Normal atmospheric pressure of the medium.</summary>
		public float atmosphericPressure = 100000F;
		/// <summary>Heat capacity of the medium.</summary>
		public float heatCapacity = 1.4F;
		/// <summary>Speed of sound in the medium.</summary>
		public float speedOfSound = 343F;

        void Awake()
        {
            Assert.IsTrue(atmosphericPressure > 0F, "atmosphericPressure value must be >0, given: " + atmosphericPressure);
            Assert.IsTrue(heatCapacity > 0F, "heatCapacity value must be >0, given: " + heatCapacity);
            Assert.IsTrue(speedOfSound > 0F, "speedOfSound value must be >0, given: " + speedOfSound);
        }

        /// <summary>Get the shock pressure of a shock wave travelling through the medium at the given speed.</summary>
        /// <returns>The shock pressure.</returns>
        /// <param name="speed">The speed of the shock wave.</param>
        public float GetShockPressure(float speed)
        {
            float machNumber = speed / speedOfSound;
            return ((2 * heatCapacity * Mathf.Pow(machNumber, 2) - (heatCapacity - 1))
                    / (heatCapacity + 1)) * atmosphericPressure;
        }
	}
}
