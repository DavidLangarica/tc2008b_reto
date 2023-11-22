/// <summary>
/// The spawner class is responsible for spawning the robots and the paviment.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject pavimentPrefab;
    public GameObject robotPrefab;
    
    private int width = 20;
    private int height = 20;
    private int robotCount = 5;
    
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        SpawnPaviment();
        SpawnRobots();
    }

    /// <summary>
    /// The SpawnPaviment method is responsible for spawning the paviment.
    /// </summary>
    void SpawnPaviment()
    {
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject go = Instantiate(pavimentPrefab, new Vector3(i, 0, j), Quaternion.identity);
                go.transform.parent = transform;
            }
        }
    }

    /// <summary>
    /// The SpawnRobots method is responsible for spawning the robots and setting their colors.
    /// </summary>
    void SpawnRobots()
    {
        for (int i = 0; i < robotCount; ++i)
        {
            GameObject go = Instantiate(robotPrefab, new Vector3(Random.Range(0, width), 0.1f, Random.Range(0, height)), Quaternion.identity);
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
