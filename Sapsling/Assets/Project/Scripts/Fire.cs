using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created by Ribhu Hooja

public class Fire : MonoBehaviour {
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private Wind windPrefab;
    [SerializeField] private float windMaxHeight;
    public float width;
    public float height;
    [SerializeField] private float spreadProbabilityPerSecond;
    private bool leftCheckDone = false;
    private bool rightCheckDone = false;


    private void Start() {
        AttemptExplodingExplodables();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + Vector3.up * height, width * (0.4f));
        foreach (Collider2D collider in colliders) {
            if (collider.TryGetComponent(out Wind _)) {
                return;
            }
        }

        MakeWind();

    }

    // If the probability of fire spreading in time period T is q
    // and the probability of fire spreading in time period T/n is p
    // then (1-q) = (1-p)^n i.e. p = 1 - (1-q)^(1/n)
    private float SpreadProbability() {
        float spreadProbability = 1 - Mathf.Pow(1 - spreadProbabilityPerSecond, Time.deltaTime); // n = 1/Time.deltaTime
        return Mathf.Clamp(spreadProbability, 0, 1);
    }

    private void AttemptSpread() {
        if (!leftCheckDone && Random.value < SpreadProbability()) {
            leftCheckDone = true;
            if (SpawnValidAt(transform.position + width * Vector3.left)) {
                GameObject fireObj = Instantiate(firePrefab);
                fireObj.transform.position = transform.position + width * Vector3.left;
            }
        }

        if (!rightCheckDone && Random.value < SpreadProbability()) {
            rightCheckDone = true;
            if (SpawnValidAt(transform.position + width * Vector3.right)) {
                GameObject fireObj = Instantiate(firePrefab);
                fireObj.transform.position = transform.position + width * Vector3.right;
            }
        }
    }

    private bool SpawnValidAt(Vector3 pos) {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.6f * height);
        if (!hit) { return false; } // How the fuck is hit a bool???
        if (hit.collider.TryGetComponent(out FlammableGround _)) {
            return true;
        }
        return false;

    }

    private void Update() {
        AttemptSpread();
    }

    private void AttemptExplodingExplodables() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Mathf.Max(height, width));

        foreach (Collider2D collider in colliders) {
            if (collider.TryGetComponent(out SquareExploder exploder)) {
                exploder.Explode();
            }
        }
    }

    private void MakeWind() {
        Wind wind = Instantiate(windPrefab);
        wind.transform.position = transform.position + Vector3.up * height;
        wind.Initialize(width, windMaxHeight, Vector2.up, 1, 5);
    }
}
