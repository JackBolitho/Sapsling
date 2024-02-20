using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

//created by Jack Bolitho

public class LevelButtonMaker : MonoBehaviour
{
    [SerializeField] private GameObject levelButton;
    private GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        //instantiates all the levels in a grid
        canvas = GameObject.Find("Canvas");
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings-2; i++){
            GameObject newButton = Instantiate(levelButton, canvas.transform);
            newButton.transform.localPosition = new Vector3(-300 + i % 9 * 75, 150 - (int)i/(int)9 * 75);
            newButton.name = "LevelButton " + (i+1);
        }
    }
}
