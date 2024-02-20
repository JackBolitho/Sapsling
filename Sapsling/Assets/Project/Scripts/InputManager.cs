using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

//created by Jack Bolitho and Ribhu Hooja

public class InputManager : MonoBehaviour {
    [SerializeField] private bool debugMode; //in debug mode, users can press x to destroy the seed currently being flung
    public GameObject treePrefab; //used to spawn tree
    public GameObject victoryTreePrefab; //used to spanw victory tree
    private GameObject currentSeed; //current seed being flung
    private PlayerControls controls;
    private InputAction dragTree;
    private TreeSling currentTree;
    public TreeSling startTree;
    private Vector2 originClick; //initial position of click, used to calculate sling
    private Vector2 currentClick; //current position of click, used to calculate sling
    private Camera mainCamera;
    public bool canChooseNewSeed; //determines whether or not a player can select a new seed

    //takes care of camera zooming
    private InputAction zoomCamera;
    private float targetZoom;
    private float zoomFactor = 3f;
    private float zoomLerpSpeed = 10f;
    [SerializeField] private float minZoom = 8f;
    [SerializeField] private float maxZoom = 15f;

    //takes care of parallax
    private ParallaxEffect parallaxEffect;

    //takes care of level management and stars
    private int totalSaplings;
    [SerializeField] private int threeStarSaplings; //the number of saplings used for three stars
    [SerializeField] private int twoStarSaplings; //the number of saplings used for two stars
    [SerializeField] private GameObject winPanel; //prefab of win panel
    [SerializeField] private GameObject thanksForPlayingPanel; //prefab of thanks for playing panel
    private Vector2 cameraOrigin; //intial 
    private bool draggingScreen = false;


    void Awake() {
        //instantiate controls
        mainCamera = Camera.main;
        controls = new PlayerControls();
        dragTree = controls.Game.DragClick;
        dragTree.started += StartDrag;
        zoomCamera = controls.Game.ZoomCamera;
        controls.Game.SeedSpecialAbility.performed += SeedSpecialAbility_performed;
        controls.Game.Restart.performed += RestartLevel;
        controls.Game.BackToLevelSelect.performed += GoBackToLevelSelect;

        //enable parallax effect
        GameObject backgroundObj = GameObject.Find("Background");
        if (backgroundObj != null) {
            parallaxEffect = backgroundObj.GetComponent<ParallaxEffect>();
        }

        //conditionally sets up seed destroying, if in debug mode
        if (debugMode) {
            controls.Debug.destroyCurrentSeed.performed += DebugDestroyCurrentSeed;
        }

        //gets total saplings for star count purposes
        totalSaplings = GameObject.FindGameObjectsWithTag("Sapling").Length;
    }

    //returns player to level select
    private void GoBackToLevelSelect(InputAction.CallbackContext obj) {
        SceneManager.LoadScene(1);
    }

    private void Start() {
        GoBackToStartTree();
    }

    void OnEnable() {
        controls.Enable();
    }

    void OnDisable() {
        controls.Disable();
    }

    //Jack Bolitho: controls dragging and screen scrolling
    void Update() {
        //takes care of slinging 
        if (currentTree != null) {
            if (dragTree.WasReleasedThisFrame()) {
                EndDrag();
            } else {
                SetPullForce();
            }
        }

        //takes care of scrolling camera
        float scrollData = zoomCamera.ReadValue<float>() * 0.005f;
        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
    }

     //Jack Bolitho: starts dragging screen
    private void StartDragScreen(Vector2 worldPos) {
        cameraOrigin = worldPos;
        draggingScreen = true;
    }

    //Jack Bolitho: ends dragging screen
    private void EndDragScreen() {
        draggingScreen = false;
    }

    //Jack Bolitho: allows for moving camera
    void LateUpdate() {
        //moves camera depending on how player drags screen
        if (draggingScreen && currentTree == null) {
            if (dragTree.WasReleasedThisFrame()) {
                EndDragScreen();
            } else {

                //moves the camera
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(dragTree.ReadValue<Vector2>());
                Vector2 difference = new Vector2(worldPos.x - mainCamera.transform.position.x, worldPos.y - mainCamera.transform.position.y);
                Vector3 camMove = new Vector3(cameraOrigin.x - difference.x, cameraOrigin.y - difference.y, mainCamera.transform.position.z);
                mainCamera.transform.position = camMove;

                //moves background objects with parallax effect
                if (parallaxEffect != null) {
                    parallaxEffect.MoveObjects(new Vector3(camMove.x, camMove.y, 0));
                }
            }
        }
    }

    //Jack Bolitho: intiates dragging on the tree 
    private void StartDrag(InputAction.CallbackContext context) {

        Vector2 mousePos = context.ReadValue<Vector2>();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        if (!currentSeed) {
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit) {
                GameObject hitObj = hit.transform.gameObject;
                if (hitObj.TryGetComponent<TreeSling>(out TreeSling tree) && tree.holdingSeed) {
                    currentTree = tree;
                    originClick = worldPos;
                    return;
                } else if (hitObj.TryGetComponent<StandingSeed>(out StandingSeed standingSeed)) {
                    if (canChooseNewSeed) {
                        standingSeed.SelectSelf(startTree);
                        return;
                    }
                }
            }
        }

        StartDragScreen(worldPos);
    }

    //Ribhu Hooja: determines pull force on seed, which will determine trajectory
    private void SetPullForce() {
        Vector2 mousePosOnScreen = dragTree.ReadValue<Vector2>();
        currentClick = Camera.main.ScreenToWorldPoint(mousePosOnScreen);

        Vector3 dragVector = currentClick - originClick;
        float dragVectorProjection = Vector3.Dot(dragVector, currentTree.OriginalUnitTangentVector);

        float pullForce = -dragVectorProjection / 2 * currentTree.pullGive;
        currentTree.SetPullForce(pullForce);
    }

    //Jack Bolitho: occurs when tree is released
    private void EndDrag() {
        currentTree.Release();
        currentTree = null;
    }

    //Ribhu Hooja: sets current seed
    public void SetCurrentSeed(GameObject s) {
        currentSeed = s;
    }

    //Ribhu Hooja: destroys active flying seed
    private void DebugDestroyCurrentSeed(InputAction.CallbackContext context) {
        Destroy(currentSeed);
        GoBackToStartTree();
    }

    //Ribhu Hooja: allows player to choose new seed
    public void GoBackToStartTree() {
        canChooseNewSeed = true;

    }

    //Jack Bolitho: displays win stars for the number of seeds it took to solve the puzzle once the level is completed
    public void WinLevel() {
        int saplingsUsed = totalSaplings - GameObject.FindGameObjectsWithTag("Sapling").Length;

        //determines number of stars
        int numOfStars = 1;
        if (saplingsUsed <= threeStarSaplings) {
            numOfStars = 3;
        } else if (saplingsUsed <= twoStarSaplings) {
            numOfStars = 2;
        }

        //sets the number of stars per level in the level tracker, which keeps track of how many stars there are per level
        GameObject levelTrackerObj = GameObject.Find("CurrentLevelTracker");
        if (levelTrackerObj != null) {
            int currentLevel = SceneManager.GetActiveScene().buildIndex - 2;
            LevelTracker levelTracker = levelTrackerObj.GetComponent<LevelTracker>();
            if (levelTracker.starsPerLevel[currentLevel] < numOfStars) {
                levelTracker.starsPerLevel[currentLevel] = numOfStars;
            }

            if (SceneManager.sceneCountInBuildSettings - 1 > currentLevel) {
                levelTracker.unlockedLevels[currentLevel + 1] = true;
            }
        }

        //shows the thanks for playing panel and sets the correct number of stars
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1) {
            GameObject thanksForPlayingPanelObj = Instantiate(thanksForPlayingPanel);
            thanksForPlayingPanelObj.GetComponent<WinPanel>().SetStars(numOfStars);
        } 
        //shows the win panel and sets the correct number of stars
        else {
            GameObject winPanelObj = Instantiate(winPanel);
            winPanelObj.GetComponent<WinPanel>().SetStars(numOfStars);
        }
    }

    //Ribhu Hooja: callback for seed special ability
    private void SeedSpecialAbility_performed(InputAction.CallbackContext obj) {
        if (currentSeed) {
            currentSeed.GetComponent<Seed>().SpecialAbility();
        }
    }

    //Jack Bolitho: reloads level
    private void RestartLevel(InputAction.CallbackContext context) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
