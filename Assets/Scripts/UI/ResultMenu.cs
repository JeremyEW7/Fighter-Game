using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ResultMenu : MonoBehaviour
{
    public GameObject resultPanel;
    public Text resultText;
    private List<Controller> playerController;

    private void OnEnable()
    {
        Controller.OnDeath += ListenCharacterDeath;
    }

    private void OnDisable()
    {
        Controller.OnDeath -= ListenCharacterDeath;
    }

    public void ListenCharacterDeath(int playerIndex)
    {
        string resultText = (playerIndex == 0) ? "Player 2 Wins!" : "Player 1 Wins!";
        SetResult(resultText);
    }

    public void InsertController(PlayerInput playerInput)
    {
        playerController.Add(playerInput.GetComponent<Controller>());
    }

    void SetResult(string result)
    {
        resultText.text = result;
        resultPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }


}
