using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public List <Transform> playerTransforms;
    public float yOffset = 2.0f;
    public float minDistance = 7.5f;
    public float maxDistance = 20f;
    public float zoomSpeed = 5f;
    public Camera camera;
    public float minX = -10f;
    public float maxX = -30f;

    private void Start()
    {
        // Set the initial position and orientation
        transform.position = new Vector3(10f, 2.5f, -0.5f);
        transform.rotation = Quaternion.Euler(-1f, -90f, 0f);

        // Find all players
        /*GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        if (allPlayers.Length == 0)
        {
            Debug.LogError("No players found in the scene!");
            return;
        }

        playerTransforms = new List<Transform>();
        for (int i = 0; i < allPlayers.Length; i++)
        {

            playerTransforms[i] = allPlayers[i].transform;
        }*/

        if (camera == null)
        {
            camera = Camera.main;
        }

        if (camera == null)
        {
            Debug.LogError("No camera found! Make sure the Camera field is set.");
        }
    }

    private void LateUpdate()
    {
        if (playerTransforms == null || playerTransforms.Count < 2)
        {
            //Debug.LogWarning("Not enough players to calculate camera bounds.");
            return;
        }

        // Calculate bounds for players
        float zMin = playerTransforms.Min(p => p.position.z);
        float zMax = playerTransforms.Max(p => p.position.z);
        float yMin = playerTransforms.Min(p => p.position.y);
        float yMax = playerTransforms.Max(p => p.position.y);

        float zMiddle = (zMin + zMax) / 2; // Middle point on the Z-axis
        float yMiddle = (yMin + yMax) / 2; // Middle point on the Y-axis

        float zDistance = zMax - zMin;
        float yDistance = yMax - yMin;
        float maxPlayerDistance = Mathf.Max(zDistance, yDistance);

        // Calculate the desired X position for the camera
        float clampedDistance = Mathf.Clamp(maxPlayerDistance, minDistance, maxDistance);
        float targetX = Mathf.Lerp(minX, maxX, (clampedDistance - minDistance) / (maxDistance - minDistance));

        // Set the target position for the camera
        Vector3 targetPosition = new Vector3(targetX, yMiddle + yOffset, zMiddle);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * zoomSpeed);

        //Debug.Log($"Camera Position: {transform.position}, Max Distance: {maxPlayerDistance}, Target X: {targetX}");
    }
}
