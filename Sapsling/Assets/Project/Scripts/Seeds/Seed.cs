using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created by Jack Bolitho and Ribhu Hooja 

public class Seed : MonoBehaviour {
    private InputManager inputManager;
    private bool hasCollided = false;
    private bool hasExitedParentTree;
    public GameObject prefab;
    public GameObject holdingSeedPrefab;
    private Rigidbody2D seedRigidbody;

    // Start is called before the first frame update
    void Start() {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        seedRigidbody = GetComponent<Rigidbody2D>();
        hasExitedParentTree = false;
    }

    //Jack Bolitho: Determines seed behavior when hitting ground. Spawnable ground allows trees to grow. Winning ground causes player to win.
    private void OnCollisionEnter2D(Collision2D collision) {
        if (!hasCollided && collision.gameObject.name.Contains("Ground")) {
            hasCollided = true;
            if (collision.gameObject.name.Contains("Spawnable")) {
                HitSpawnableGround(collision);
            } else if (collision.gameObject.name.Contains("Win")) {
                HitWinGround(collision);
            } else {
                HitNormalGround();
            }

        } else if (collision.gameObject.name.Contains("Bound")) {
            DestroySelf();
        }
    }

    //Ribhu Hooja: when a seed enters another tree, switch to that tree
    private void OnTriggerEnter2D(Collider2D collider) {
        if (hasExitedParentTree && collider.TryGetComponent<TreeSling>(out TreeSling tree)) {
            SwitchTree(tree);
        }
    }

    //Jack Bolitho: indicates that a tree has been exited
    private void OnTriggerExit2D(Collider2D collider) {
        if (!hasExitedParentTree) {
            hasExitedParentTree = true;
        }
    }

    //Jack Bolitho: instantiates a new tree at the position where the seed hits spawnable ground
    //tree direction depends on normal vector of ground
    private void SproutNewTree(Collision2D collision) {
        Vector3 savedPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        GameObject newTree = Instantiate(inputManager.treePrefab);

        newTree.transform.up = collision.GetContact(0).normal;
        newTree.transform.position = savedPosition;
    }

    //Jack Bolitho: creates the victory tree depending on normal vector
    private void SproutVictoryTree(Collision2D collision) {
        Vector3 savedPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        GameObject newTree = Instantiate(inputManager.victoryTreePrefab);

        newTree.transform.eulerAngles = collision.transform.eulerAngles;
        newTree.transform.position = savedPosition;
    }

    //Jack Bolitho: sets the correct seed to child of tip of tree, and destroys the flying seed
    private void SwitchTree(TreeSling tree) {
        GameObject holdingSeedObj = Instantiate(holdingSeedPrefab, tree.transform.GetChild(2));
        tree.holdingSeed = holdingSeedObj.GetComponent<HoldingSeed>();

        Destroy(gameObject);
    }

    //Ribhu Hooja: destroys object when hitting ground
    protected virtual void HitSpawnableGround(Collision2D collision) {
        SproutNewTree(collision);
        DestroySelf();
    }

    //Jack Bolitho: causes death timer to start once the ground has been hhit
    protected virtual void HitNormalGround() {
        hasCollided = false;
        StartCoroutine(DeathTimer());
    }

    //Jack Bolitho: destroys seed if it does not move for more than 0.6 seconds
    private IEnumerator DeathTimer() {
        while (true) {
            while (Mathf.Abs(seedRigidbody.velocity.x) > 0.1 || Mathf.Abs(seedRigidbody.velocity.y) > 0.1) {
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.5f);
            if (Mathf.Abs(seedRigidbody.velocity.x) < 0.1 && Mathf.Abs(seedRigidbody.velocity.y) < 0.1) {
                DestroySelf();
            }
        }
    }


    protected virtual void HitWinGround(Collision2D collision) {
        SproutVictoryTree(collision);
        inputManager.WinLevel();
        DestroySelf();
    }

    public virtual void SpecialAbility() { }

    protected void DestroySelf() {
        Destroy(gameObject);
        inputManager.GoBackToStartTree();
    }
}
