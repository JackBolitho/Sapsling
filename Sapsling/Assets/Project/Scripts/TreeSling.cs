using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

//created by Jack Bolitho and Ribhu Hooja

public class TreeSling : MonoBehaviour {

    public float pullGive; //how fast the tree moves back based on how you drag 
    [SerializeField] private float treeRange; //how far back the tree can pull
    [SerializeField] private float treeReleaseForce; //the force on the ball
    [SerializeField] private GameObject treeTip;
    [SerializeField] private GameObject treeBase;
    [SerializeField] private GameObject leafGraphic;
    [SerializeField] private TrajectoryHelper trajectoryHelperPrefab;
    public HoldingSeed holdingSeed;
    public Vector3 OriginalUnitTangentVector { get; private set; }
    private float angle;
    [SerializeField] private float radius; //distance bewteen treebase and treetip
    private Vector3 circlePos = new Vector3(0, 1, 0);
    private InputManager inputManager;
    private bool returnToOriginalPos = false;
    private TrajectoryHelper trajectoryHelper;
    private float seedMass;

    private void Start() {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        OriginalUnitTangentVector = UnitTangentVector();
    }

    //Jack Bolitho: determines how far back the tree will move by converting force into the tree's angle
    public void SetPullForce(float force) {
        float maxForce = treeRange;
        float minForce = -treeRange;

        if (force > maxForce) {
            force = maxForce;
        } else if (force < minForce) {
            force = minForce;
        }

        angle = 90 - force;
        SetTreeTip();
    }


    private void SetTreeTip() {
        //Ribhu Hooja: instantiates the trajectory guide
        seedMass = holdingSeed.seedPrefab.GetComponent<Rigidbody2D>().mass; //TODO: This is awkward
        if (trajectoryHelper == null && !returnToOriginalPos) {
            trajectoryHelper = Instantiate(trajectoryHelperPrefab);
        }

        //Jack Bolitho: gets the position of the tip in the unit circle, with the base of the tree as the origin
        float rad = angle * Mathf.Deg2Rad;
        circlePos = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);

        //Jack Bolitho: controls the location of the tree tip, which allows the tree to effectively bend 
        treeTip.transform.localPosition = new Vector3(treeBase.transform.localPosition.x + radius * circlePos.x, treeBase.transform.localPosition.y + radius * circlePos.y, 0);
        leafGraphic.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        //Ribhu Hooja: computes trajectory
        if (trajectoryHelper != null) {
            trajectoryHelper.RecomputeTrajectory(treeTip.transform.position, UnitTangentVector() * treeReleaseForce * (90 - angle) / seedMass);
        }
    }

    public void Release() {
        //Ribhu Hooja: prevents player from immediately inputting new seed
        if (this == inputManager.startTree) {
            inputManager.canChooseNewSeed = false;
        }

        //Jack Bolitho: create seed, destory holding seed, and get necesary components
        GameObject seed = Instantiate(holdingSeed.seedPrefab);
        Destroy(holdingSeed.gameObject);
        seed.transform.position = treeTip.transform.position;
        Rigidbody2D seedrb = seed.GetComponent<Rigidbody2D>();
        Animator seedAnimator = seed.GetComponent<Animator>();
        seedAnimator.SetTrigger("Fly");

        //Ribhu Hooja: release with a force proportional to how much the tree has been pulled back
        seedrb.AddForce(UnitTangentVector() * treeReleaseForce * (90 - angle), ForceMode2D.Impulse);


        //Jack Bolitho: rotate the seed for graphical reasons
        if (angle < 90) {
            seed.transform.GetChild(0).transform.Rotate(new Vector3(0, 180, 0));
        }

        //Ribhu Hooja: resets tree and destroys trajectroy guide
        inputManager.SetCurrentSeed(seed);
        returnToOriginalPos = true;

        SetTreeTip();
        Destroy(trajectoryHelper.gameObject);
    }

    //Ribhu Hooja: returns the unit tangent vector
    public Vector3 UnitTangentVector() {
        Vector3 radiusVector = treeTip.transform.position - treeBase.transform.position;
        Vector3 tangentVector = new(-radiusVector.y, radiusVector.x, 0f);
        return tangentVector.normalized;
    }

    //Jack Bolitho: moves tree to original position after being flung
    private void FixedUpdate() {
        if (returnToOriginalPos) {
            if (angle > 101.5f) {
                angle -= 10;
                SetTreeTip();
            } else if (angle < 78.5f) {
                angle += 10;
                SetTreeTip();
            } else {
                angle = 90;
                SetTreeTip();
                returnToOriginalPos = false;
            }
        }
    }
}
