using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created by Ribhu Hooja
public class CottonSeed : Seed {
    public int WIND_AFFECTED = 6;       // layer number
    [SerializeField] private Transform wings;
    private bool specialUsed = false;

    //reduces gravity scale when ability is activated, and allows the seed to be affected by gravity 
    public override void SpecialAbility() {
        if (specialUsed) { return; }
        gameObject.layer = WIND_AFFECTED;
        GetComponent<Rigidbody2D>().gravityScale /= 2;
        wings.localScale = 2 * wings.localScale;
        specialUsed = true;

    }
}
