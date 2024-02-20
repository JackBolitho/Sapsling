using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created by Ribhu Hooja

public class Particle2D : MonoBehaviour {
    protected float halfLife;
    protected float lambda;

    // each particle has a probabilistic life span
    // the half life is the amount of time in which half
    // of the particles decay
    private void Awake() {
        lambda = Mathf.Log(2) / halfLife;
    }
    protected virtual void Update() {
        if (Random.value < ProbabilityOfDecay(Time.deltaTime)) {
            DestroySelf();
        }
    }

    protected virtual void DestroySelf() {
        Destroy(gameObject);
    }

    // returns the probability that the particle decays
    // in a given interval of time t
    // https://en.wikipedia.org/wiki/Radioactive_decay#Mathematics - see "one decay processes"
    private float ProbabilityOfDecay(float time) {
        return 1 - Mathf.Exp(-lambda * time);   // see: radioactive decay
    }

    public void SetHalfLife(float value) {
        halfLife = value;
    }
}
