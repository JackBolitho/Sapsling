using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created by Jack Bolitho

public class PuzzleButton : MonoBehaviour
{
    public List<Door> doors = new List<Door>();

    //turns on or off doors when button is collided with
    void OnCollisionEnter2D(Collision2D collision){
        foreach(Door door in doors){
            door.ChangeActivation();
        }
    }
}
