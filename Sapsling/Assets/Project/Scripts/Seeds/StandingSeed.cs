using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created by Jack Bolitho

public class StandingSeed : MonoBehaviour {
    public GameObject holdingSeedPrefab;

    public void SelectSelf(TreeSling startTree) {
        //replaces the holding seed with a standing seed
        if (startTree.holdingSeed) {
            GameObject newStandingSeed = Instantiate(startTree.holdingSeed.standingSeedPrefab);
            newStandingSeed.transform.position = transform.position;
            Destroy(startTree.holdingSeed.gameObject);
        }

        //creates the new holding seed
        GameObject holdingSeedObj = Instantiate(holdingSeedPrefab, startTree.transform.GetChild(2));
        startTree.holdingSeed = holdingSeedObj.GetComponent<HoldingSeed>();
        Destroy(gameObject);
    }
}
