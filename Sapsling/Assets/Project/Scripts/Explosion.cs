using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created by Ribhu Hooja

public class Explosion : MonoBehaviour {

    // The explosion grows, then hovers at the same size, then fades away
    private enum ExplosionState {
        GROWING,
        HOVERING,
        FADING
    }

    public float maxRadius;
    private float radius = 0;
    public float growTime;
    public float hoverTime;
    public float fadeTime;
    private ExplosionState state = ExplosionState.GROWING;

    private SpriteRenderer spriteRenderer;
    private float redComponent;
    private float greenComponent;
    private float blueComponent;
    private float alpha;


    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        redComponent = spriteRenderer.color.r;
        greenComponent = spriteRenderer.color.g;
        blueComponent = spriteRenderer.color.b;
        alpha = spriteRenderer.color.a; // should be 1
    }

    private void Update() {
        if (state == ExplosionState.GROWING) {
            radius += maxRadius * (Time.deltaTime / growTime);
            if (radius >= maxRadius) {
                radius = maxRadius;
                state = ExplosionState.HOVERING;
                StartCoroutine(Hover());
            }
            gameObject.transform.localScale = Vector3.one * radius * 2;
        } else if (state == ExplosionState.FADING) {
            alpha -= Time.deltaTime / fadeTime;
            if (alpha > 0) {
                spriteRenderer.color = new Color(redComponent, greenComponent, blueComponent, alpha);
            } else {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator Hover() {
        DoExplosionChecks();
        yield return new WaitForSeconds(hoverTime);
        state = ExplosionState.FADING;
    }

    private void DoExplosionChecks() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);


        foreach (Collider2D collider in colliders) {
            // start fires and explode explodables
            if (collider.TryGetComponent(out FlammableGround flammableGround)) {
                flammableGround.SetOnFire(transform.position, radius);
            } else if (collider.TryGetComponent(out SquareExploder exploder)) {
                exploder.Explode();
            }
        }


    }

}
