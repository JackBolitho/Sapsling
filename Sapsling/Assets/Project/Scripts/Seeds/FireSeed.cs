using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created by Ribhu Hooja

public class FireSeed : Seed {
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionRadius;


    public override void SpecialAbility() {
        Explode();
    }

    //instantiates explosion and destroys self
    private void Explode() {
        GameObject explosionObj = Instantiate(explosionPrefab);
        explosionObj.transform.position = transform.position;
        Explosion explosion = explosionObj.GetComponent<Explosion>();

        explosion.maxRadius = explosionRadius;

        DestroySelf();
    }

    protected override void HitNormalGround() {
        Explode();
    }

    protected override void HitSpawnableGround(Collision2D collision) {
        Explode();
    }
}

