using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Created by Jack Bolitho

public class LoadNextSceneButton : MonoBehaviour {
    [SerializeField] private int sceneIndex; //loads the next scene if scene manager is -1, otherwise loads specified scene

    public void LoadNextScene() {
        if (sceneIndex == -1 && SceneManager.GetActiveScene().buildIndex <= SceneManager.sceneCountInBuildSettings - 1) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
