using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//created by Jack Bolitho

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    //allows for consistent music playing, so mulitple music players do noit overlap one another
    void Awake(){
        if(instance == null){
            instance = this;
            gameObject.name = "CurrentMusicPlayer";
            DontDestroyOnLoad(gameObject);
        }else{
            GameObject otherMusicPlayer = GameObject.Find("MusicPlayer");
            if(otherMusicPlayer != null){
                Destroy(otherMusicPlayer);
            }
        }
    }
}
