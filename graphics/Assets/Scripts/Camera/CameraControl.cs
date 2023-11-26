/// <summary>
/// The CameraControl class is responsible for controlling the camera.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float lerpTime = 0.1f;

    private GameManager gameManager;
    private int gridWidth;
    private int gridHeight;
    private int currentSide = 0;
    private Vector3 targetPosition;
    
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager component not found.");
        } else {
            gridWidth = gameManager.width;
            gridHeight = gameManager.height;
            CalculateCameraPositions();
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentSide = (currentSide + 1) % 5;
            CalculateCameraPositions();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentSide = (currentSide + 3) % 5;
            CalculateCameraPositions();
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / lerpTime);
        transform.LookAt(new Vector3(gridWidth / 2f, 0, gridHeight / 2f));
    }

    /// <summary>
    /// The CalculateCameraPositions method is responsible for calculating the camera positions.
    /// </summary>
    private void CalculateCameraPositions()
    {
        float cameraDistance = Mathf.Max(gridWidth, gridHeight);
        Vector3 gridCenter = new Vector3(gridWidth / 2f, 0, gridHeight / 2f);

        Vector3[] cameraPositions = new Vector3[5];
        cameraPositions[0] = gridCenter + new Vector3(-cameraDistance, 6.5f, 0); 
        cameraPositions[1] = gridCenter + new Vector3(0, 6.5f, cameraDistance); 
        cameraPositions[2] = gridCenter + new Vector3(cameraDistance, 6.5f, 0);
        cameraPositions[3] = gridCenter + new Vector3(0, 6.5f, -cameraDistance);

        cameraPositions[4] = gridCenter + new Vector3(0, cameraDistance, -cameraDistance); 

        targetPosition = cameraPositions[currentSide];
    }
}
