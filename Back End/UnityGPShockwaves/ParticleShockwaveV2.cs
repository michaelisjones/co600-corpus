using UnityEngine;
using System.Collections.Generic;

namespace UnityGPShockwaves
{
    /// <summary>A particle shock wave that uses the Unity particle system.</summary>
    /// <author>Liam Ireland, Michael Jones, Tomasz Mackow</author>
    class ParticleShockwaveV2 : MonoBehaviour
    {
        /// <summary>The constant acceleration of the wave, usually negative.</summary>
        public float constantDampening = 0.1F; // TODO: calculate wave acceleration ?
        /// <summary>The starting speed of the particles.</summary>
        public float startSpeed = 69F;
        /// <summary>The number of partiles in the shockwave.</summary>
        public int NumberOfParticles = 40000;
        /// <summary>How long the particles last until they die in seconds.</summary>
        public float startLifetime = 500F;
        /// variables for calculating accurate particle forces
        public ShockwaveMedium medium;

        /// <summary>The number of partiles in one particle burst.</summary>
        short NoOfParticlesInEachBurst = 20000;
        /// <summary>All of the colliders without rigidbodys in the scene.</summary>
        Collider[] staticColliders;
        /// <summary>The particle system.</summary>
        new ParticleSystem particleEmitter;

		/// <summary>Awake message.</summary>
        /// <author>Liam Ireland</author>
        void Awake()
        {
            if (constantDampening < 0) constantDampening *= -1.0F;
            if (!medium) medium = ScriptableObject.CreateInstance<ShockwaveMedium>();

            particleEmitter = gameObject.AddComponent <ParticleSystem>() as ParticleSystem;
            particleEmitter.name = "Particle Emitter";
            particleEmitter.playOnAwake = true;
            particleEmitter.loop = false;
            particleEmitter.startLifetime = startLifetime;
            particleEmitter.startSpeed = startSpeed;
            particleEmitter.startSize = 0.1F;
            particleEmitter.maxParticles = 1000000;
            particleEmitter.simulationSpace = ParticleSystemSimulationSpace.World;

            //setup the curve something module
            //tom did this
            ParticleSystem.LimitVelocityOverLifetimeModule limitVelocityModule = particleEmitter.limitVelocityOverLifetime;
            limitVelocityModule.enabled = true;
            limitVelocityModule.limit = 0F;
            limitVelocityModule.dampen = constantDampening;

            //setup the shape module
            ParticleSystem.ShapeModule shapeModule = particleEmitter.shape;
            shapeModule.enabled = true;
            shapeModule.shapeType = ParticleSystemShapeType.Sphere;
            shapeModule.radius = 0.01F;
            shapeModule.randomDirection = true;

            //setup the Emission module
            ParticleSystem.EmissionModule emissionModule = particleEmitter.emission;
            emissionModule.rate = 0;
            // if the number of particles is bigger than the number of particles in each burst, split them up into seperate bursts
            // (the reason for this is the maximum number of particles in one burst can only be a short (16 bits), using multiple bursts we can get around this (although unity does only limit us to 4 bursts))
            if (NumberOfParticles > NoOfParticlesInEachBurst)
            {
                int numberOfBursts = 0;
                if(NumberOfParticles % NoOfParticlesInEachBurst == 0)
                {
                    numberOfBursts = NumberOfParticles / NoOfParticlesInEachBurst;
                }
                else
                {
                    numberOfBursts = (NumberOfParticles / NoOfParticlesInEachBurst) + 1;
                }

                ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[numberOfBursts];
                for (int i=0; i<numberOfBursts; i++)
                {
                    bursts[i] = new ParticleSystem.Burst(0, NoOfParticlesInEachBurst);
                }
                emissionModule.SetBursts(bursts);
            }
            else
            {
                emissionModule.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0, NoOfParticlesInEachBurst) }); // make the bursts array and create the initiale burst of particles
            }

            // setup the collision module
            ParticleSystem.CollisionModule collisionModule = particleEmitter.collision;
            collisionModule.enabled = true;
            collisionModule.type = ParticleSystemCollisionType.World;
            collisionModule.mode = ParticleSystemCollisionMode.Collision3D;
            collisionModule.enableDynamicColliders = true;
            collisionModule.enableInteriorCollisions = true;
            collisionModule.quality = ParticleSystemCollisionQuality.High; // could perhaps change to help performance
            collisionModule.sendCollisionMessages = true;
            collisionModule.maxCollisionShapes = 256; // default value

            // get the static objects in the scene and set the particle triggers for diffraction - this is no longer used
            /*
            Collider[] allColldiers = FindObjectsOfType<Collider>();
            staticColliders = new Collider[allColldiers.Length - FindObjectsOfType<Rigidbody>().Length]; // initialise the staticColliders array to the size of the number of static colliders in the scene
            int counter = 0;
            foreach (Collider collider in allColldiers) // loop through every collider in the scene
            {
                if (!collider.GetComponent<Rigidbody>()) // if the collider does not have a rigidbody
                {
                    staticColliders[counter++] = collider; // set the collider to the counter element on the staticColliders array and increment the counter
                }
            }
            */

            // set up the triggers for diffraction - this is no longer used
            /*
            ParticleSystem.TriggerModule triggers = particleEmitter.trigger; // initialise the same amount of triggers as static colliders
            triggers.enabled = true;
            triggers.exit = ParticleSystemOverlapAction.Callback; // when a particle exits the trigger, call OnParticleTrigger()
            triggers.inside = ParticleSystemOverlapAction.Ignore;
            for (int i=0; i<counter; i++)
            {
                GameObject trigger = new GameObject();
                trigger.name = "Particle Trigger";
                trigger.transform.position = staticColliders[i].transform.position;
                trigger.transform.localScale = new Vector3(staticColliders[i].transform.localScale.x + 0.5F, staticColliders[i].transform.localScale.y + 0.5F, staticColliders[i].transform.localScale.z + 0.5F);
                trigger.AddComponent<BoxCollider>(); // I'm assuming the static colliders have box colliders - they may not - need to fix
                trigger.GetComponent<BoxCollider>().isTrigger = true;
                triggers.SetCollider(i, trigger.GetComponent<BoxCollider>());
            }
            */

        }

        /// <summary>Called when a particle collides with a collider.</summary>
        /// <param name="other">The GameObject the particle is colliding with.</param>
        /// <author>Michael Jones, Tomasz Mackow</author>
        private void OnParticleCollision(GameObject other) // when a particle hits a collider
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb) // if the rigidbody exists
            {
                //rigidbody.AddExplosionForce(1000, transform.position, Vector3.Distance(transform.position, other.transform.position), 0, ForceMode.Impulse);  // Legacy

                List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
                ParticlePhysicsExtensions.GetCollisionEvents(particleEmitter, other, collisionEvents);

                foreach (ParticleCollisionEvent collisionEvent in collisionEvents)
                {
                    if (collisionEvent.velocity != Vector3.zero)
                    {
                        Vector3 velocity = collisionEvent.velocity;
                        float radius = particleEmitter.time * startSpeed; // radius assumes constant speed. TODO: use average speed (after acceleration is impleemted)
                        float shockPressure = medium.GetShockPressure(velocity.magnitude);
                        float surfaceArea = (4.0F * Mathf.PI * Mathf.Pow(radius, 2)) / NumberOfParticles;
                        Vector3 force = velocity.normalized * shockPressure * surfaceArea;
                        rb.AddForce(force, ForceMode.Force);
                    }
                }
            }
         /* else // if colliding with a collider without a rigidbody - no longer used
            {
                List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
                ParticlePhysicsExtensions.GetCollisionEvents(particleEmitter, other, collisionEvents);

                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[NumberOfParticles];
                particleEmitter.GetParticles(particles);

                for (int i = 0; i < particles.Length; i++)
                {
                    foreach (ParticleCollisionEvent collisionEvent in collisionEvents)
                    {
                        if (particles[i].velocity == collisionEvent.velocity && particles[i].position == collisionEvent.intersection)
                            particles[i].lifetime = 0F;
                    }
                }
                particleEmitter.SetParticles(particles, particles.Length);
            } */
        }


        /// <summary>Called when a particle leaves the trigger.</summary>
        /// <author>Liam Ireland</author>
        private void OnParticleTrigger()
        {
            List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
            List<ParticleSystem.Particle> newParticles = new List<ParticleSystem.Particle>();  // an arraycontaining the particles to be added
            foreach (ParticleSystem.Particle particle in particles) // for every particle that just left a trigger
            {
                Collider closestCollider = closestStaticCollider(particle.position);
                Vector3 closestPointOnStaticCollider = closestCollider.bounds.ClosestPoint(particle.position);
                // get the distance from the particle to the shock wave origin
                float distanceFromParticleToOrigin = Vector3.Distance(particle.position, particleEmitter.transform.position);
                // get the distance from the closest point on the collider the particle nearlly missed to the shock wave origin
                float distanceFromColliderToOrigin = Vector3.Distance(closestPointOnStaticCollider, particleEmitter.transform.position);
                // if the distance from the particle to the shockwave origin is greater than the closest point to the particle on the collider we're checking to the shockwave origin - it makes sense in my mind, I don't know how to explain it easisly
                if (distanceFromParticleToOrigin > distanceFromColliderToOrigin)
                {
                    // doesn't quite work as intended yet - need to find a better check
                    // TODO - find a better check above
                    if(particle.velocity.y <= 0) // pretty dumb, need more robust solution
                    for (int i=0; i<4; i++) { // create multiple new particles for every near miss of a static collider
                        ParticleSystem.Particle newParticle = new ParticleSystem.Particle();
                        newParticle = particle;
                        newParticle.velocity = new Vector3(newParticle.velocity.x + UnityEngine.Random.Range(-1, 1), newParticle.velocity.y - (i*0.75F), newParticle.velocity.z + UnityEngine.Random.Range(-1, 1));
                        newParticle.startColor = new Color32(255, 0, 0, 255);
                        newParticles.Add(newParticle);
                    }
                }
            }
            ParticleSystem.Particle[] oldParticles = new ParticleSystem.Particle[particleEmitter.particleCount];
            particleEmitter.GetParticles(oldParticles);
            ParticleSystem.Particle[] finalParticles = new ParticleSystem.Particle[oldParticles.Length + newParticles.Count];
            oldParticles.CopyTo(finalParticles, 0);
            newParticles.ToArray().CopyTo(finalParticles, oldParticles.Length);
            particleEmitter.SetParticles(finalParticles, finalParticles.Length);
        }

        /// <summary>Finds and returns the closest static collider to the point provided.</summary>
        /// <param name="point">The point to check</param>
        /// <returns>the closest static collider to the point</returns>
        /// <author>Liam Ireland</author>
        private Collider closestStaticCollider(Vector3 point)
        {
            Collider closestCollider = staticColliders[0] ? staticColliders[0] : null;
            Vector3 closestDistance = new Vector3(9999999, 9999999, 9999999);
            foreach (Collider collider in staticColliders)
            {
                if (collider.bounds.ClosestPoint(point).magnitude < closestDistance.magnitude)
                {
                    closestCollider = collider;
                    closestDistance = collider.bounds.ClosestPoint(point);
                }
            }
            return closestCollider;
        }

        /// <summary>kills old particle and replaces it with a new particle with a new velocity</summary>
        /// <param name="oldParticle">The old particle</param>
        /// <param name="newVelocity">The new velocity of the new particle</param>
        /// <returns>The new particle</returns>
        /// <author>Liam Ireland</author>
        private ParticleSystem.Particle ParticleVelocityChange(ParticleSystem.Particle oldParticle, Vector3 newVelocity)
        {
            ParticleSystem.Particle newParticle = new ParticleSystem.Particle();
            newParticle = oldParticle;
            newParticle.velocity = newVelocity;
            newParticle.startColor = new Color32(0, 255, 0, 255);
            return newParticle;
        }

        /// <summary>Called every fixed update.</summary>
        /// <author>Liam Ireland</author>
        void FixedUpdate() 
        {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[NumberOfParticles];
            particleEmitter.GetParticles(particles);


            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].velocity.magnitude < medium.speedOfSound)
                    particles[i].lifetime = 0F;
            }

            particleEmitter.SetParticles(particles, particles.Length);
        }
    }
}
