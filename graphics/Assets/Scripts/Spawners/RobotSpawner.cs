/// <summary>
/// The RobotSpawner class is responsible for spawning the robots.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSpawner : MonoBehaviour
{
    private GameManager gameManager;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager component not found.");
        } else {
            SpawnRobots();
        }
    }

     /// <summary>
    /// The SpawnRobots method is responsible for spawning the robots and setting their colors.
    /// </summary>
    void SpawnRobots()
    {
        int width = gameManager.width;
        int height = gameManager.height;
        GameObject robotPrefab = gameManager.robotPrefab;
        string[] robots = gameManager.robots;
        Random.InitState(System.DateTime.Now.Millisecond);


        for (int i = 0; i < robots.Length; ++i)
        {
            GameObject go = Instantiate(robotPrefab, new Vector3(Random.Range(0, width), 0.1f, Random.Range(0, height)), Quaternion.identity);
            go.name = robots[i];
            go.transform.parent = transform;

            Transform sphere = go.transform.Find("Sphere");

            if (sphere != null)
            {
                Renderer renderer = sphere.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // Body
                    Material bodyMaterial = renderer.materials[0]; 
                    // Eyes
                    Material eyesMaterial = renderer.materials[1]; 
                    // Arms and antenna
                    Material extremitiesMaterial = renderer.materials[2]; 
                    
                    float r = Random.Range(0.0f, 1.0f);
                    float g = Random.Range(0.0f, 1.0f);
                    float b = Random.Range(0.0f, 1.0f);

                    bodyMaterial.color = new Color(r, g, b) * 0.7f;
                    eyesMaterial.color = new Color(r, g, b) * 0.3f;
                    extremitiesMaterial.color = new Color(r, g, b);                   
                }
                else
                {
                    Debug.LogError("Renderer component not found on Sphere object.");
                }
            }
            else
            {
                Debug.LogError("Sphere object not found within the robot prefab.");
            }

        }
    }
}
