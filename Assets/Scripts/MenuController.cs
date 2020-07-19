using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    enum Screen
    {
        None,
        Main,
        Settings,
    }

    public CanvasGroup mainScreen;
    public CanvasGroup settingsScreen;

    void SetCurrentScreen(Screen screen)
    {
        Utility.SetCanvasGroupEnabled(mainScreen, screen == Screen.Main);
        Utility.SetCanvasGroupEnabled(settingsScreen, screen == Screen.Settings);
    }

    void Awake()
    {
        SetCurrentScreen(Screen.Main);
    }

    public void StartNewGame()
    {
        SetCurrentScreen(Screen.None);
        LoadingScreen.instance.LoadScene("SampleScene");
    }

    public void OpenSettings()
    {
        SetCurrentScreen(Screen.Settings);
    }

    public void CloseSettings()
    {
        SetCurrentScreen(Screen.Main);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
