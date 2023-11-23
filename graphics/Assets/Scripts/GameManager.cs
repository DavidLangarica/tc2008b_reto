/// <summary>
/// The GameManager class is responsible for spawning the paviment, robots and trees.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject pavimentPrefab;
    public GameObject robotPrefab;
    public GameObject treeA;
    public GameObject treeB;
    public GameObject Restaurant;

    public int[] binPosition = { 0, 0 };

    public int width = 20;
    public int height = 20;
    public string[] robots = {"robot-0", "robot-1", "robot-2", "robot-3", "robot-4"};

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
    }
}
