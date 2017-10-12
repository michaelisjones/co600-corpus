using UnityEngine;
using System;

namespace UnityGPShockwaves
{
    /// <summary>A shock wave consisting of spherical rigid particles.</summary>
    /// <author>Tomasz Mackow, Liam Ireland</author>
    public class ParticleShockwaveV1 : MonoBehaviour
    {
        /// <summary>The number of particles to be emitted from the inner sphere</summary>
        public int numberOfParticles = 1000;
        /// <summary>The scale of the inner sphere</summary>
        private float scale = 1.0f;
        /// <summary>An array of the particles</summary>
        private GameObject[] particles;

        /// <summary>Called when the scene is started</summary>
        /// <author>Liam Ireland</author>
        void Awake()
        {
            GameObject innerSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            innerSphere.transform.position = this.transform.position;
            innerSphere.transform.localScale = innerSphere.transform.localScale * (scale * 2);
            innerSphere.GetComponent<Renderer>().enabled = false;

            Vector3[] myPoints = GetPointsOnSphere(numberOfParticles);
            particles = new GameObject[numberOfParticles];

            foreach (Vector3 point in myPoints)
            {
                GameObject particle = GameObject.CreatePrimitive(PrimitiveType.Sphere); // create the particle
                particle.transform.position = innerSphere.transform.position + point * scale;
                //particle.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F); // reduce the size of the particles - for future use (maybe)
                particle.AddComponent<Rigidbody>(); // Add a Rigidbody to the particle to enable us to set the velocity
                particle.GetComponent<Rigidbody>().velocity = transform.forward;
                particle.GetComponent<Rigidbody>().useGravity = false;
                particles[Array.IndexOf(myPoints, point)] = particle; // Add the particle to the array of particles (not used currently)
            }
        }

        /// <summary>Called every fixed update.</summary>
        /// <author>Liam Ireland</author>
        void FixedUpdate()
        {
            /*foreach (GameObject particle in particles) // not used yet
            {

            }*/
        }

        /// <summary>Returns an array of Vector3s representing the points on a sphere.</summary>
        /// <param name="nPoints">Number of points on the sphere</param>
        /// <returns>An array of Vector3s representing the points on a sphere</returns>
        /// <author>Algorithm taken from http://web.archive.org/web/20120421191837/http://cgafaq.info/wiki/Evenly_distributed_points_on_sphere </author>
        Vector3[] GetPointsOnSphere(int nPoints)
        {
            float fPoints = (float)nPoints;

            Vector3[] points = new Vector3[nPoints];

            float inc = Mathf.PI * (3 - Mathf.Sqrt(5)); // ~2.39996323
            float off = 2 / fPoints;

            for (int k = 0; k < nPoints; k++)
            {
                float y = k * off - 1 + (off / 2);
                float r = Mathf.Sqrt(1 - y * y);
                float phi = k * inc;

                points[k] = new Vector3(Mathf.Cos(phi) * r, y, Mathf.Sin(phi) * r);
            }

            return points;
        }
    }
}