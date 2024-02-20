using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//created by Jack Bolitho

public class WinPanel : MonoBehaviour
{
    [SerializeField] private List<Image> stars = new List<Image>(); //first index is first star, last index is final star
    [SerializeField] private Sprite goldStar; 

    //sets the number of stars
    public void SetStars(int num){
        for(int i = 0; i < num; i++){
            stars[i].sprite = goldStar;
        }
    }
}
