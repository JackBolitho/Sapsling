using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created by Ribhu Hooja

// A wind a is a rectangular zone in which particles are created at one end
// and float to the other end
public class Wind : MonoBehaviour {
    [SerializeField] private WindAffectedParticle[] particles; // probably have a dict alongside for later
    private WindAffectedParticle preferredParticle;

    public float meanTimePerParticle;   // mean time between new particles spawned
    private float particleHalfLife;     // amount of time taken by half of the particles to decay

    // The height of the wind rectangle
    public float Height {
        get { return transform.localScale.y; }
        private set {
            float diff = value - Height;
            transform.localScale = new(transform.localScale.x, value, transform.localScale.z);
            transform.position += Direction * diff / 2;
        }
    }

    // The width of the wind rectangle
    public float Width {
        get { return transform.localScale.x; }
        private set { transform.localScale = new(value, transform.localScale.y, transform.localScale.z); }
    }

    // the direction vector of the wind i.e. the direction the particles float in
    // and the direction game objects experience a wind force
    public Vector3 Direction {
        get { return transform.up; }
        private set { transform.up = value; }
    }

    private AreaEffector2D effector;

    public float Strength {
        get { return effector.forceMagnitude; }
        set { effector.forceMagnitude = value; }
    }

    private void Awake() {
        effector = GetComponent<AreaEffector2D>();
    }

    public void Initialize(float width, float maxHeight, Vector2 direction, int preferredParticleIndex, float particleHalfLife) {
        Width = width;
        Direction = direction;

        // The wind tries to make a rectangle of maxHeight
        // But if an object is in the way the rectangle stops midway
        Height = 0;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, maxHeight);
        foreach (RaycastHit2D hit in hits) {
            if (!hit.collider.isTrigger) {
                Height = hit.distance;
            }
        }
        if (Height == 0) { Height = maxHeight; }
        this.particleHalfLife = particleHalfLife;


        // some ugly code to set up which particle is spawned by the wind
        // if the index is not give then we default to 0
        if (preferredParticleIndex < 0 || preferredParticleIndex > particles.Length - 1) {
            preferredParticle = particles[0];
        } else {
            preferredParticle = particles[preferredParticleIndex];
        }
    }

    private void Update() {
        // This is very similar to the radioactive decay code (uses the same math/ differential equation)
        // The 'lambda' for this equation is 1/meanTime (integrate to find out why that's the case :) )
        // So the probability that a particle gets spawned in a time period of t is 1 - e^-(lambda)t
        // (this is technically slightly incorrect because of discrete timesteps but whatever)
        if (Random.value < (1 - Mathf.Exp(-(1 / meanTimePerParticle) * Time.deltaTime))) {
            Particle2D particle = Instantiate(preferredParticle);
            particle.transform.position = transform.position + Vector3.down * Height / 2 + Vector3.right * Width * (Random.value - 0.5f) + Vector3.up;
            particle.SetHalfLife(particleHalfLife);
        }
    }
}
