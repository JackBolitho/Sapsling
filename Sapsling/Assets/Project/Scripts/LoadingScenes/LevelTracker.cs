using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created by Jack Bolitho

public class LevelTracker : MonoBehaviour
{
    public List<bool> unlockedLevels = new List<bool>();
    public List<int> starsPerLevel = new List<int>();

    private static LevelTracker instance;

    //keeps track of all of the levels and whether or not they have stars, does not destroy on load
    void Awake(){
        if(instance == null){
            instance = this;
            gameObject.name = "CurrentLevelTracker";
            DontDestroyOnLoad(gameObject);
        }else{
            GameObject otherLevelTracker = GameObject.Find("LevelTracker");
            if(otherLevelTracker != null){
                Destroy(otherLevelTracker);
            }
        }
    }

}
