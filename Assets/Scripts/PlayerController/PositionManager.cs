using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PositionManager : MonoBehaviour
{
    public static PositionManager Instance { get; private set; }
    public Transform[] spawnPositions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetPosition(PlayerInput playerInput)
    {
        playerInput.transform.position = spawnPositions[playerInput.playerIndex].transform.position;
        playerInput.transform.rotation = spawnPositions[playerInput.playerIndex].transform.rotation;
    }
}
