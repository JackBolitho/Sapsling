using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created by Ribhu Hooja

// Ground that spawns fire when touched by an explosion
public class FlammableGround : MonoBehaviour {
    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField] private GameObject firePrefab;
    private Vector2 center;
    private float fireWidth;

    private void Start() {
        // Assumes the ground is rectangular, then gets bounding coordinates for it (to spawn fire correctly)
        Bounds bounds = GetComponent<BoxCollider2D>().bounds;
        width = bounds.max.x - bounds.min.x;
        height = bounds.max.y - bounds.min.y;
        center = bounds.center;
        fireWidth = firePrefab.GetComponent<Fire>().width;
    }

    //
    //
    //      Tries to set the part of the ground intersected by the explosion on fire
    //      I don't think words explain the thing well. This does:      
    //
    //           _______
    //        ,-'       `-.
    //       /             \
    //      |               |
    //      |               |
    //      |       o <-----|--------- the explosion center. The circle is the explosion
    //      |       |       |
    //   ----\------o------/--------   <- The ground
    //        `._   ^  _.'
    //           `--|--'
    //              |
    //              |
    //              |
    //              The code starts at this point, direcly underneath the center
    //  Then it tries to spawn fires to the left and right, until it goes outside the explosion or goes off the edge of the ground
    //  moving by (widthOfFiresSprite) to the left or right each time


    public void SetOnFire(Vector2 explosionCoords, float blastRadius) {
        float surfaceY = center.y + height / 2;
        if (explosionCoords.y - surfaceY <= blastRadius) {
            if (explosionCoords.x > center.x + width / 2) {


                for (float fireX = center.x + width / 2 - fireWidth / 2; Vector2.Distance(new(fireX, surfaceY), explosionCoords) <= blastRadius; fireX -= fireWidth) {
                    MakeFireIfPossible(new(fireX, surfaceY));
                }

            } else if (explosionCoords.x < center.x - width / 2) {


                for (float fireX = center.x - width / 2 + fireWidth / 2; Vector2.Distance(new(fireX, surfaceY), explosionCoords) <= blastRadius; fireX += fireWidth) {
                    MakeFireIfPossible(new(fireX, surfaceY));
                }

            } else {

                for (float fireX = explosionCoords.x; Vector2.Distance(new(fireX, surfaceY), explosionCoords) <= blastRadius && fireX > center.x - width / 2; fireX -= fireWidth) {
                    MakeFireIfPossible(new(fireX, surfaceY));
                }

                for (float fireX = explosionCoords.x + fireWidth; Vector2.Distance(new(fireX, surfaceY), explosionCoords) <= blastRadius && fireX < center.x + width / 2; fireX += fireWidth) {
                    MakeFireIfPossible(new(fireX, surfaceY));
                }
            }
        }
    }

    // Tries to spawn a fire, if there is no fire overlapping
    private void MakeFireIfPossible(Vector2 position) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, fireWidth * (0.5f));
        foreach (Collider2D collider in colliders) {
            if (collider.TryGetComponent<Fire>(out Fire _)) {
                return;
            }
        }
        GameObject fire = Instantiate(firePrefab);
        fire.transform.position = position;
    }
}
