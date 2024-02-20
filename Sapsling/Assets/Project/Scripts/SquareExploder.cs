using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created by Ribhu Hooja

// This is a component that you can attach to ANY object with a collider
// It will make that object explode when touched by an Explosion
// The square exploder assumes that the object is square-like
// If you use it for, say, a long line-like object you'll get a hilariously big explosion
// Other exploders are in the works
public class SquareExploder : MonoBehaviour {
    [SerializeField] private Explosion explosionPrefab;
    [SerializeField] private float collisionMultiplier;
    private bool hasExploded = false;

    private void Awake() {
        if (collisionMultiplier < 1) {
            collisionMultiplier = 1;
        }
    }

    public void Explode() {
        if (hasExploded) { return; }
        hasExploded = true;
        Bounds bounds = GetComponent<Collider2D>().bounds;
        float radius = Mathf.Max(bounds.max.x - bounds.center.x, bounds.max.y - bounds.center.y);

        Explosion explosion = Instantiate(explosionPrefab);
        explosion.transform.position = bounds.center;
        explosion.maxRadius = radius * collisionMultiplier;
        StartCoroutine(DestroySelf());

    }

    private IEnumerator DestroySelf() {
        yield return new WaitForSeconds(explosionPrefab.growTime);
        Destroy(gameObject);
    }
}
