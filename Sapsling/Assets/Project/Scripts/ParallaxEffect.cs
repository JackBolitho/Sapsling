using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created by Jack Bolitho

public class ParallaxEffect : MonoBehaviour
{
    public List<GameObject> slowDecor = new List<GameObject>();
    public List<GameObject> midDecor = new List<GameObject>();
    public List<GameObject> fastDecor = new List<GameObject>();
    public float slowMove;
    public float midMove;
    public float fastMove;

    private List<Vector3> ogSlowPos = new List<Vector3>();
    private List<Vector3> ogMidPos = new List<Vector3>();
    private List<Vector3> ogFastPos = new List<Vector3>();


    private void Start(){
        foreach(GameObject obj in slowDecor){
            ogSlowPos.Add(obj.transform.position);
        }
        foreach(GameObject obj in midDecor){
            ogMidPos.Add(obj.transform.position);
        }
        foreach(GameObject obj in fastDecor){
            ogFastPos.Add(obj.transform.position);
        }
    }

    //moves background based on camera position, relative to their original position. Camera must start at transform position (0,0,0) to smoothly work
    public void MoveObjects(Vector3 camPos){
        for(int i = 0; i < slowDecor.Count; i++){
            slowDecor[i].transform.position = camPos * slowMove + ogSlowPos[i];
        }
        for(int i = 0; i < midDecor.Count; i++){
            midDecor[i].transform.position = camPos * midMove + ogMidPos[i];
        }
        for(int i = 0; i < fastDecor.Count; i++){
            fastDecor[i].transform.position = camPos * fastMove + ogFastPos[i];
        }
    }
    
}
