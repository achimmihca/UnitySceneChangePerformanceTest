using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class SceneChangeTestControl : MonoBehaviour
{
    private static Stopwatch stopwatch = new();

    private bool wasAnyTouch;

    private void OnEnable()
    {
        if (!EnhancedTouchSupport.enabled)
        {
            EnhancedTouchSupport.Enable();
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        stopwatch.Stop();
        Debug.Log($"OnSceneLoaded - Loading scene took {stopwatch.ElapsedMilliseconds} ms");
    }


    private void Update()
    {
        if ((Keyboard.current != null && Keyboard.current.anyKey.wasReleasedThisFrame)
            || (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame))
        {
            ChangeScene();
        }

        wasAnyTouch |= Touch.activeTouches.Count > 0;
        if (wasAnyTouch
            && Touch.activeTouches.Count == 0)
        {
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        var nextSceneName = currentSceneName == "FirstScene"
            ? "SecondScene"
            : "FirstScene";

        stopwatch.Reset();
        stopwatch.Start();
        SceneManager.LoadSceneAsync(nextSceneName, new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.None));
    }
}