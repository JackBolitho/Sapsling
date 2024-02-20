using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//created by Jack Bolitho

public class LevelButton : MonoBehaviour {
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private List<Image> stars = new List<Image>();
    [SerializeField] private Sprite goldStar;
    private int sceneNumber;
    private LevelTracker levelTracker;
    private bool unlocked;
    private int numOfStars;

    // Start is called before the first frame update
    void Start() {
        //gets the level number from the name
        string[] nameParts = gameObject.name.Split(" ");
        sceneNumber = int.Parse(nameParts[1]);

        levelTracker = GameObject.Find("CurrentLevelTracker").GetComponent<LevelTracker>();

        //records the number of stars and unlock status for a level
        unlocked = levelTracker.unlockedLevels[sceneNumber - 1];
        numOfStars = levelTracker.starsPerLevel[sceneNumber - 1];

        //sets the stars if the level is unlocked
        if (unlocked) {
            numberText.text = nameParts[1];
            for (int i = 0; i < numOfStars; i++) {
                stars[i].sprite = goldStar;
            }
        } 
        //otherwise shows lock icon and diables button
        else {
            lockIcon.SetActive(true);
            foreach (Image star in stars) {
                star.gameObject.SetActive(false);
            }
            button.enabled = false;
        }
    }

    //loads next level in build
    public void LoadLevel() {
        SceneManager.LoadScene(sceneNumber + 1);
    }
}