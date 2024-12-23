using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance
    {
        get;private set;
    }

    public GameObject pauseMenu;
    public bool isPaused = false;
    public static Action<bool> OnPauseStatusChanged;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Pause()
    {
        Instance.isPaused = true;
        Time.timeScale = 0f;
        Instance.pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        InstructionManager.Instance.HideInstruction();

        OnPauseStatusChanged?.Invoke(true);
    }

    public void Resume()
    {
        Instance.isPaused = false;
        Time.timeScale = 1f;
        Instance.pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InstructionManager.Instance.ShowInstruction();

        OnPauseStatusChanged?.Invoke(false);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
