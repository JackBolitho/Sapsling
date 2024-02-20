using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created by Ribhu Hooja

public class Fan : MonoBehaviour {

    [SerializeField] private Wind windPrefab;
    [SerializeField] private float windHeight;
    [SerializeField] private float windWidth;


    // Start is called before the first frame update
    void Start() {
        Wind wind = Instantiate(windPrefab);
        wind.transform.position = transform.position + Vector3.up * 2;
        wind.Initialize(windWidth, windHeight, transform.up, 0, 5);
        wind.Strength = 15;
        wind.meanTimePerParticle = 0.2f;
    }
}
