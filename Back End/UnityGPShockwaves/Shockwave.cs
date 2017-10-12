using UnityEngine;

namespace UnityGPShockwaves
{
    /// <summary>A generic interface for shock wave objects. Provides virtual methods to be overridden by subclass.</summary>
    /// <author>Michael Jones</author>
    public class Shockwave : ScriptableObject
    {
        /// <summary>The initial wave speed. (m/s)</summary>
        [Tooltip("The initial wave speed. (m/s)")]
        public float initialSpeed = 500F;
        /// <summary>The medium of the shock wave.</summary>
        [Tooltip("The properties of the shock wave medium, used for calculating shock wave effects. Uses default ShockWaveMedium if none given.")]
        public ShockwaveMedium waveMedium;
        /// <summary>The color used for low pressure particles.</summary>
        [Tooltip("The color used for low pressure particles.")]
        public Color color1 = Color.green;
        /// <summary>The color used for high pressure particles.</summary>
        [Tooltip("The color used for high pressure particles.")]
        public Color color2 = Color.red;
        /// <summary>Render a visualisation of the shock wave.</summary>
        [Tooltip("Note: Enabling visualisation may hinder performance.")]
        public bool visualisation = false;
        /// <summary>The accuracy of the simulation. Higher accuracy means higher performance cost.</summary>
        [Tooltip("Higher accuracy will result in a better simulation but may hinder performance. Defaults to 1.")]
        public float accuracy = 1F;
        /// <summary>Print debug statements.</summary>
        [Tooltip("Print debug statements.")]
        public bool debugMode = false;
        /// <summary>Disabled instance will have no effect.</summary>
        [HideInInspector]
        public bool enabled = true;

        /// <summary>Has the Setup function been called?</summary>
        protected bool setup = false;

        /// <summary>Setup message must be called by handler.</summary>
        public virtual void Setup()
        {
            if (!enabled) return;

            // TODO: fields bounds checks

            if (!waveMedium)
            {
                Debug.LogError("Missing wave medium!");
                return;
            }

            // warn if wave is too slow
            if (initialSpeed < waveMedium.speedOfSound) Debug.LogWarning("initialSpeed (" + initialSpeed + ") is less than shockWaveMedium.speedOfSound (" + waveMedium.speedOfSound + "). The shock wave will have no effect below the speed of sound.");

            setup = true;
        }

        /// <summary>Reset this instance.</summary>
        public virtual void Reset()
        {
            setup = false;
        }

        /// <summary>Spawns a shock wave at the given origin position.</summary>
        /// <param name="origin">The origin position in world space.</param>
        public virtual void Spawn(Vector3 origin)
        {
            if (!enabled) return;

            if (!setup)
            {
                Debug.LogError("Can't spawn shock wave. Call Shockwave.Setup first!");
                return;
            }
        }

        /// <summary>FixedUpdate message must be called by handler every fixed update.</summary>
        public virtual void FixedUpdate()
        {
            if (!enabled) return;
        }

        /// <summary>OnRenderObject message must be called by handler.</summary>
        public virtual void OnRenderObject()
        {
            if (!enabled || !visualisation) return;
        }
    }
}
