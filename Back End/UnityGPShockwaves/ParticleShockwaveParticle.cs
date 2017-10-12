using UnityEngine;

namespace UnityGPShockwaves
{
    /// <summary>
    /// A particle has a position coordinate and a velocity represented as a direction vector and speed.
    /// A particle has an alive/dead state, and should not act or have any effect after being killed.
    /// Please note: This class is only used in the V3 particle Shockwave (ParticleShockwaveV3).
    /// </summary>
    /// <author>Michael Jones, Tomasz Mackow</author>
    public class ParticleShockwaveParticle
    {

        /// <summary>The position (in world space) of the particle.</summary>
        public Vector3 position;
        /// <summary>The normalized velocity of the particle.</summary>
        public Vector3 direction;
        /// <summary>The speed of the particle.</summary>
        public float speed;
        /// <summary>The previous position (in world space) of the particle.</summary>
        public Vector3 prevPosition;

        /// <summary>The constant acceleration of the particle, normally a negative number or zero.</summary>
        public float constantAcceleration;
        /// <summary>The initial particle speed.</summary>
        public float initSpeed;
        /// <summary>The time at which the particle was created.</summary>
        public float initTime;

        /// <summary>Alive particles are visible and active. Dead particles will be overwritten and garbage-collected.</summary>
        bool alive = true;
        /// <summary>The number of particles spawned in a batch with this particle.</summary>
        int particlesCount;

        /// <summary>Constructor for a particle.</summary>
        /// <param name="position">Position (in world space) of the particle.</param>
        /// <param name="direction">The normalized velocity of the particle.</param>
        /// <param name="speed">The speed of the particle.</param>
        /// <param name="constantAcceleration">Constant acceleration of the particle, normally a negative number or zero.</param>
        /// <param name="particlesCount">The number of particles spawned at the same time as this one.</param>
        public ParticleShockwaveParticle(Vector3 position, Vector3 direction, float speed,
                                         float constantAcceleration, int particlesCount)
        {
            this.position = position;
            this.direction = direction;
            this.speed = speed;
            this.constantAcceleration = constantAcceleration;
            this.particlesCount = particlesCount;

            initSpeed = speed;
            initTime = Time.fixedTime;
        }

        /// <summary>Constructor for a particle. Takes a velocity parameter instead of direction and speed.</summary>
        /// <param name="position">Position (in world space) of the particle.</param>
        /// <param name="velocity">Velocity of the particle.</param>
        /// <param name="constantAcceleration">Constant acceleration of the particle, normally a negative number or zero.</param>
        /// <param name="particlesCount">The number of particles spawned at the same time as this one.</param>
        public ParticleShockwaveParticle(Vector3 position, Vector3 velocity,
                                         float constantAcceleration, int particlesCount)
        {
            this.position = position;
            this.direction = velocity.normalized;
            this.speed = velocity.magnitude;
            this.constantAcceleration = constantAcceleration;
            this.particlesCount = particlesCount;

            initSpeed = speed;
            initTime = Time.fixedTime;
        }

        /// <summary>Copy constructor for a particle.</summary>
        /// <param name="particle">The ParticleShockwaveParticle to copy</param>
        public ParticleShockwaveParticle(ParticleShockwaveParticle particle)
            : this(particle.position, particle.direction, particle.speed, particle.constantAcceleration, particle.particlesCount)
        {
            initSpeed = particle.initSpeed;
            initTime = particle.initTime;

            alive = particle.alive;
        }

        /// <summary>
        /// Returns the radius of the sphere sector represented by this particle.
        /// Radius is currently simply the distance travelled by the particle.
        /// </summary>
        /// <returns>The radius.</returns>
        public float GetRadius()
        {
            float time = Time.fixedTime - initTime;
            return (initSpeed * time) + ((constantAcceleration * Mathf.Pow(time, 2)) / 2F);
        }

        /// <summary>Calculates the surface area represented by this particle.</summary>
        /// <returns>The surface area of the particle. (m²)</returns>
        public float GetSurfaceArea()
        {
            return (4.0F * Mathf.PI * Mathf.Pow(GetRadius(), 2)) / particlesCount;
        }

        /// <summary>Returns the velocity of the particle.</summary>
        /// <returns>The velocity of the particle.</returns>
        public Vector3 GetVelocity()
        {
            return this.direction * this.speed;
        }

        /// <summary>Returns the velocity of the particle.</summary>
        /// <returns>The velocity of the particle.</returns>
        public Vector3 GetVelocity(float speed)
        {
            return this.direction * speed;
        }

        /// <summary>Returns the velocity of the particle.</summary>
        /// <returns>The velocity of the particle.</returns>
        public Vector3 GetVelocity(Vector3 direction, float speed)
        {
            return direction * speed;
        }

        /// <summary>Setter for position.</summary>
        /// <param name="position">The position (in world space) of the particle.</param>
        public void SetPosition(Vector3 position)
        {
            this.prevPosition = this.position;
            this.position = position;
        }

        /// <summary>Setter for direction.</summary>
        /// <param name="direction">The normalized velocity of the particle.</param>
        public void SetDirection(Vector3 direction)
        {
            this.direction = direction;
        }

        /// <summary>Setter for speed.</summary>
        /// <param name="speed">The speed of the particle.</param>
        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        /// <summary>Kill the particle.</summary>
        public void Kill()
        {
            alive = false;
        }

        /// <summary>Is the particle alive?</summary>
        /// <returns>If the particle is alive.</returns>
        public bool IsAlive()
        {
            return alive;
        }
    }
}
