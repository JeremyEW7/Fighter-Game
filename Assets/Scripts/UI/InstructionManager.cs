using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InstructionManager : MonoBehaviour
{
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
    public static InstructionManager Instance
    {
        get;private set;
    }
    public GameObject[] instructionPanels;



    public void CloseInstruction(PlayerInput playerInput)
    {
        instructionPanels[playerInput.playerIndex].SetActive(false);
    }

    public void HideInstruction()
    {
        instructionPanels[0].GetComponentInParent<CanvasGroup>().alpha = 0f;
    }

    public void ShowInstruction()
    {
        instructionPanels[0].GetComponentInParent<CanvasGroup>().alpha = 1f;
    }
}
