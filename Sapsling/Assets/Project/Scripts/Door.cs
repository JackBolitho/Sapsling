using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created by Jack Bolitho

public class Door : MonoBehaviour
{
    private bool activated = false;
    [SerializeField] private Vector3 activatedScale;
    private Vector3 originalScale;

    private void Start(){
        originalScale = transform.localScale;
    }

    //changes scale depending on activation; reliant on puzzle button 
    public void ChangeActivation(){
        activated = !activated;

        if(activated){
            transform.localScale = activatedScale;
        }
        else{
            transform.localScale = originalScale;
        }
    }
}
