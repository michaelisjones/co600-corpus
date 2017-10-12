using System.Collections.Generic;
using UnityEngine;

namespace UnityGPShockwaves
{
    /// <summary>A particles Shockwave that stores a particle as a ParticleShockwaveParticle class.</summary>
    /// <author>Michael Jones, Tomasz Mackow, Liam Ireland</author>
    [CreateAssetMenu(menuName = "Shock Wave (ParticleShockwaveV3)", fileName = "New Shock Wave")]
    public class ParticleShockwaveV3 : Shockwave
    {
        /// <summary>The default number of particles to spawn at the start. This value is multiplied by the accuracy value. Note that the actual number of particles may differ due to rounding.</summary>
        [Tooltip("The default number of particles to spawn at the start. This value is multiplied by the accuracy value. Note that the actual number of particles may differ due to rounding.")]
        public int defaultInitialCount = 10000;
        /// <summary>The maximum number of particles that can exist. Value is multiplied by initialCount.</summary>
        [Tooltip("The maximum number of particles that can exist. Value is multiplied by the initial count. An error will be logged if the number of particles reaches this number.")]
        public float maxCountMultiplier = 10;
        /// <summary>The radius of the shock wave at the start. Can be zero. (m)</summary>
        [Tooltip("The radius of the shock wave at the start. Can be zero. (m)")]
        public float initialRadius = 0F;

        /// <summary>The constant acceleration of the wave, usually negative. (m/s²)</summary>
        [Tooltip("The constant acceleration of the wave, should usually be a negative value. (m/s²)")]
        public float constantAcceleration = -5F;

        /// <summary>The amount of energy absorbed when a particle hits a static collider. Value between 0 and 1.</summary>
        [Tooltip("The amount of energy absorbed when a particle hits a static collider. Value between 0 and 1.")]
        public float absorptionFactor = 0.2F; // TODO: calculate absorption ?
        /// <summary>Absorb some energy particles when they hit static colliders. Energy loss calculated using absorptionFactor.</summary>
        public bool enableAbsorption = true;

        /// <summary>With dynamic bounces on, particles will bounce off of dynamic colliders (as well as static colliders).</summary>
        [Tooltip("With dynamic bounces on, particles will bounce off of dynamic colliders (as well as static colliders).")]
        public bool dynamicBounces = false;

        /// <summary>With transmittance on, particles will continue through walls as well as bounce off walls.</summary>
        [Tooltip("With transmittance on, particles will continue through walls as well as bounce off walls.")]
        public bool transmittance = false;

        /// <summary>The desired number of particles to spawn at the start after multiplying by the accuracy value. Note that the actual number of particles may differ due to rounding.</summary>
        int initialCount;
        /// <summary>The GL shader material.</summary>
        Material glMaterial = null;
        /// <summary>The highest shock pressure value of any particle so far.</summary>
        float highShockPressure = -1F;

        /// <summary>Particles in the shock wave.</summary>
        ParticleShockwaveParticle[] particles;
        /// <summary>The upper-most assigned index of the particles array.</summary>
        int particlesUpperIndex = -1;
        /// <summary>Indexes of dead particles that can be overwritten.</summary>
        Stack<int> particlesOverwriteStack = new Stack<int>();

        private int aliveParticlesCount;

        //float tempTotalForces = 0F;

        /// <summary>Setup message must be called by handler.</summary>
        public override void Setup()
        {
            base.Setup();

            // set to false until end of function
            setup = false;

            if (!enabled) return;

            initialCount = Mathf.RoundToInt(defaultInitialCount * accuracy);

            if (initialCount <= 0) throw new UnityException("initialCount value must be >0, given value: " + initialCount);
            if (maxCountMultiplier < 1) throw new UnityException("maxCountMultiplier value must be >=1, given value: " + maxCountMultiplier);

            // TODO: warn if number of particles is too low for accurate simulation

            // warn if radius is negative and use absolute value
            if (initialRadius < 0F)
            {
                Debug.LogWarning("initialRadius value should be >0, given: " + initialRadius + ". The absolute value will be used instead.");
                // use absolute value
                initialRadius = Mathf.Abs(initialRadius);
            }
            // warn if acceleration is positive
            if (constantAcceleration > 0F) Debug.LogWarning("constantAcceleration value should usually be negative, given: " + constantAcceleration);
            // warn if particle absorption factor is >1 or <0 and clamp value
            if (absorptionFactor > 1F || absorptionFactor < 0F)
            {
                Debug.LogWarning("absorptionFactor value should be between 0 and 1, given: " + absorptionFactor + ". The value will be clamped.");
                // clamp value
                absorptionFactor = Mathf.Clamp(absorptionFactor, 0F, 1F);
            }

            particles = new ParticleShockwaveParticle[Mathf.RoundToInt(initialCount * maxCountMultiplier)];
            highShockPressure = waveMedium.GetShockPressure(initialSpeed);

            // create material https://docs.unity3d.com/ScriptReference/GL.html
            glMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
            glMaterial.hideFlags = HideFlags.HideAndDontSave;
            glMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            glMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            glMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            glMaterial.SetInt("_ZWrite", 0);

            setup = true;
        }

        /// <summary>Reset this instance.</summary>
        public override void Reset()
        {
            base.Reset();

            initialCount = 0;
            glMaterial = null;
            highShockPressure = -1F;
            particles = null;
            particlesUpperIndex = -1;
            particlesOverwriteStack = new Stack<int>();

            //tempTotalForces = 0F;
        }

        /// <summary>Spawns a shock wave at the given origin position.</summary>
        /// <param name="origin">The origin position in world space.</param>
        public override void Spawn(Vector3 origin)
        {
            base.Spawn(origin);

            if (!enabled || !setup) return;

            SpawnParticlesSphere(initialCount, initialRadius, origin);
        }

        /// <summary>FixedUpdate message must be called by handler.</summary>
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!enabled || !setup) return;

            int debugCountInteractions = 0;
            int debugCountBounces = 0;

            float deltaTime = Time.fixedDeltaTime;
            ParticleShockwaveParticle p;

            // update particles
            for (int i = 0; i < particlesUpperIndex + 1; i++)
            {
                // get particle object reference
                p = particles[i];

                // skip particle if null or dead
                if (p == null || !p.IsAlive()) continue;


                float distance = p.speed * deltaTime;
                float shockPressure = waveMedium.GetShockPressure(p.speed);
                if (shockPressure > highShockPressure)
                    highShockPressure = shockPressure;

                Vector3 newPosition = p.position;
                Vector3 newDirection = p.direction;
                float newSpeed = p.speed;

                // NOTE:
                // "hit" refers to the particle hitting any collider
                // "bounce" refers to the particle hitting a static collider
                // "interaction" refers to the particle hitting a dynamic collider

                // set cast point to the next position of the particle if there is no bounce
                Vector3 castPoint = p.position + (p.direction * distance);

                // check for interaction or bounce
                RaycastHit interaction;
                bool doesHit = Physics.Linecast(p.position, castPoint, out interaction);
                if (doesHit)
                {

                    // handle bounces
                    RaycastHit bounce;
                    bool doesBounce = Physics.Raycast(p.position, p.direction, out bounce, distance);
                    if (!dynamicBounces && bounce.rigidbody) doesBounce = false;
                    if (doesBounce)
                    {
                        if (enableAbsorption && absorptionFactor >= 1F)
                        {
                            KillParticle(i);
                        }
                        else
                        {
                            newDirection = Vector3.Reflect(p.direction, bounce.normal);
                            newPosition = bounce.point + (newDirection * (distance - bounce.distance));

                            if (enableAbsorption)
                            {
                                // NOTE: newDirection is not updated here as an optimisation
                                newSpeed *= 1F - absorptionFactor;
                            }

                            if (transmittance) {
                                float newSpeed2 = (p.speed * 0.7F) + (p.constantAcceleration * deltaTime);
                                SpawnParticle(castPoint, p.position, p.direction * newSpeed2, constantAcceleration, 1);
                            }
                        }

                        if (debugMode) debugCountBounces++;
                    }
                    else
                    {
                        newPosition = castPoint;
                    }

                    // handle interactions
                    if (doesHit && interaction.rigidbody) // apply force if we hit a dynamic collider
                    {
                        float forceMagnitude = p.GetSurfaceArea() * shockPressure;
                        forceMagnitude *= (distance - bounce.distance) / distance;
                        forceMagnitude *= deltaTime;
                        Vector3 force = p.direction * forceMagnitude;
                        interaction.rigidbody.AddForceAtPosition(force, bounce.point, ForceMode.Impulse);

                        if (debugMode) debugCountInteractions++;

                        //tempTotalForces += forceMagnitude;
                    }
                }
                else // didn't hit anything
                {
                    newPosition = castPoint;
                }


                // apply acceleration
                newSpeed += (p.constantAcceleration * deltaTime);

                // kill particle if slow
                if (newSpeed < waveMedium.speedOfSound)
                    KillParticle(i);

                // set new position, direction and speed
                p.SetPosition(newPosition);
                p.SetDirection(newDirection);
                p.SetSpeed(newSpeed);
            }

            //if (debugMode && (debugCountBounces > 0 || debugCountInteractions > 0)) Debug.Log("fixedTime: " + Time.fixedTime + ", bounces: " + debugCountBounces + ", interactions: " + debugCountInteractions);

            //if (debugMode) Debug.Log("tempTotalForces: " + tempTotalForces);
        }

        /// <summary>OnRenderObject message must be called by handler.</summary>
        public override void OnRenderObject()
        {
            base.OnRenderObject();

            if (!enabled || !setup || !visualisation || !glMaterial || particlesUpperIndex <= -1 || aliveParticlesCount <= 0) return;

            glMaterial.SetPass(0);

            GL.PushMatrix();
            GL.Begin(GL.LINES);
            // render particles https://docs.unity3d.com/ScriptReference/GL.html
            for (int i = 0; i < particlesUpperIndex + 1; i++)
            {
                if (particles[i] == null || !particles[i].IsAlive()) continue;

                // calculate particle color
                Color c;
                float interp = waveMedium.GetShockPressure(particles[i].speed) / highShockPressure;
                c = Color.Lerp(color1, color2, interp);

                // set saturation and value to 1
                float H, S, V;
                Color.RGBToHSV(c, out H, out S, out V);
                c = Color.HSVToRGB(H, 1F, 1F);

                GL.Color(c);
                GL.Vertex3(particles[i].prevPosition.x, particles[i].prevPosition.y, particles[i].prevPosition.z);
                GL.Vertex3(particles[i].position.x, particles[i].position.y, particles[i].position.z);
            }
            GL.End();
            GL.PopMatrix();
        }

        /// <summary>Kills the particle in the particles array at the given index.</summary>
        /// <param name="index">array index</param>
        void KillParticle(int index)
        {
            aliveParticlesCount--;
            particles[index].Kill();
            particlesOverwriteStack.Push(index);
        }

        /// <summary>Spawns a particle at the given position (in world space).</summary>
        /// <param name="position">Position in world space.</param>
        /// <param name="velocity">Velocity.</param>
        /// <param name="acceleration">Acceleration.</param>
        /// <param name="count">The number of particles spawned at the same time as this one.</param>
        /// <param name="visible">Whether or not to render the particle.</param>
        void SpawnParticle(Vector3 position, Vector3 prevPosition, Vector3 velocity, float acceleration, int count, bool visible = true)
        {
            // get best array index
            int index;
            if (particlesOverwriteStack.Count > 0) index = particlesOverwriteStack.Pop();
            else if (particlesUpperIndex < particles.Length - 1) index = ++particlesUpperIndex;
            else
            {
                Debug.LogWarning("Cannot spawn particle; particles array is full.");
                return;
            }

            // create particle
            particles[index] = new ParticleShockwaveParticle(position, velocity, acceleration, count);
            particles[index].prevPosition = prevPosition;

            if (particlesUpperIndex == particles.Length - 1) Debug.LogError("The particles array is full. (array size: " + particles.Length + ") Particles will only be spawned if other particles are killed.");

            aliveParticlesCount++;
        }

        /// <summary>Spawns particles evenly distributed on the surface of a sphere of given radius.</summary>
        /// <param name="desiredCount">The desired number of particles to spawn. Note that the actual number of particles may differ due to rounding and symmetry.</param>
        /// <param name="radius">The radius of the sphere. Can be zero.</param>
        /// <param name="center">The center position (in world space) of the sphere.</param>
        void SpawnParticlesSphere(int desiredCount, float radius, Vector3 center)
        {
            int count = 0;

            // spherical coords calculated using: http://www.cmu.edu/biolphys/deserno/pdf/sphere_equi.pdf

            float a = (4.0F * Mathf.PI) / desiredCount;
            float d = Mathf.Sqrt(a);
            int MTh = (int)Mathf.Round(Mathf.PI / d);
            float dTh = Mathf.PI / MTh;
            float dPh = a / dTh;

            // count particles in batch
            for (int m = 0; m < MTh; m++)
            {
                float th = Mathf.PI * (m + 0.5F) / MTh;
                int MPh = (int)Mathf.Round(2 * Mathf.PI * Mathf.Sin(th) / dPh);
                count += MPh;
            }

            // spawn particles
            for (int m = 0; m < MTh; m++)
            {
                float th = Mathf.PI * (m + 0.5F) / MTh;
                int MPh = (int)Mathf.Round(2 * Mathf.PI * Mathf.Sin(th) / dPh);

                for (int n = 0; n < MPh; n++)
                {
                    float ph = (2 * Mathf.PI * n) / MPh;

                    float x = Mathf.Sin(th) * Mathf.Cos(ph);
                    float y = Mathf.Sin(th) * Mathf.Sin(ph);
                    float z = Mathf.Cos(th);

                    // make position and velocity vectors

                    Vector3 direction = new Vector3(x, y, z).normalized;
                    Vector3 position = direction * radius;
                    Vector3 velocity = direction * initialSpeed;
                    position += center;

                    SpawnParticle(position, position, velocity, constantAcceleration, count, visualisation);
                }
            }

            //Debug.Log("spawned particles. count: " + count);
        }
    }
}
