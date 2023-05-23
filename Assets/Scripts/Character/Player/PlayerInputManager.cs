using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Singleton;
    PlayerControls playerControls;
    [SerializeField] Vector2 movementInput;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        //When the scene changes, run this logic
        SceneManager.activeSceneChanged += OnSceneChange;

        Singleton.enabled = false;

    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        // If we are loading into our world scene, enable our player controls
        if (newScene.buildIndex == WorldSaveGameManager.Singleton.GetWorldSceneIndex())
        {
            Singleton.enabled = true;
        }
        // otherwise we must be at the main menu, disable our player controls
        // prevents character from moving during menu screens, etc.
        else
        {
            Singleton.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        //If we destroy this object, unsubscribe from the event
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
}
