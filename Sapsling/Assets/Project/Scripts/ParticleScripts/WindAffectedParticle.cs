using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created by Ribhu Hooja

// A particle that is affected by wind
public class WindAffectedParticle : Particle2D {
    // how long the particle takes to fade
    [SerializeField] private float fadeTime;
    private bool fading = false;

    private SpriteRenderer spriteRenderer;
    
    // color
    private float redComponent;
    private float greenComponent;
    private float blueComponent;
    private float maxAlpha;
    private float alpha;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        redComponent = spriteRenderer.color.r;
        greenComponent = spriteRenderer.color.g;
        blueComponent = spriteRenderer.color.b;
        maxAlpha = Random.value;
        spriteRenderer.color = new(redComponent, greenComponent, blueComponent, maxAlpha);
        alpha = maxAlpha;
    }

    protected override void DestroySelf() {
        FadeOut();
    }

    private void FadeOut() {
        fading = true;
    }

    protected override void Update() {
        base.Update();      // do the normal particle stuff; i.e. call destroy self probabilistically, etc.
        // as it fades it gets lighter
        if (fading) {
            alpha -= maxAlpha * Time.deltaTime / fadeTime;
            if (alpha > 0) {
                spriteRenderer.color = new Color(redComponent, greenComponent, blueComponent, alpha);
            } else {
                Destroy(gameObject);
            }
        }
    }

    // If the particle leaves the wind it dies
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.TryGetComponent<Wind>(out Wind _)) {
            Destroy(gameObject);
        }
    }
}
