using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;

    void Start()
    {
        mainMenu.SetActive(true);
    }

    public void PlayButtonClicked()
    {
        Debug.Log("Masuk");
        SceneManager.LoadSceneAsync("Map1", LoadSceneMode.Single);
    }

    public void QuitButtonClicked()
    {
        Application.Quit();
    }

}
