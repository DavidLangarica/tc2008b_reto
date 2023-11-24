/// <summary>
/// The GameManager class is responsible for spawning the paviment, robots and trees.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject pavimentPrefab;
    public GameObject waiterPrefab;
    public GameObject treeA;
    public GameObject treeB;
    public GameObject binPrefab;
    public GameObject food1;
    public GameObject food2;
    public GameObject food3;

    public int width = 20;
    public int height = 20;

    private int numWaiters = 5;

    [HideInInspector]
    public Waiter[] waiters;

    [HideInInspector]
    public int[] binPosition = {13, 0};

    /// <summary>
    /// initWaiters is responsible for initializing the waiters.
    /// </summary>
    public void initWaiters()
    {
        waiters = new Waiter[numWaiters];

        for (int i = 0; i < numWaiters; i++) {
            waiters[i] = new Waiter();
            waiters[i].id = "waiter-" + i;
            waiters[i].CarryingFood = 0;
            waiters[i].Step = 0;
            waiters[i].X = 0;
            waiters[i].Y = 0;
        }
        
        Waiter waiter0 = waiters[0];
        waiter0.CarryingFood = 0;
        waiter0.Step = 0;
        waiter0.X = 1;
        waiter0.Y = 0;

        Waiter waiter1 = waiters[1];
        waiter1.CarryingFood = 0;
        waiter1.Step = 0;
        waiter1.X = 5;
        waiter1.Y = 0;

        Waiter waiter2 = waiters[2];
        waiter2.CarryingFood = 0;
        waiter2.Step = 0;
        waiter2.X = 9;
        waiter2.Y = 0;

        Waiter waiter3 = waiters[3];
        waiter3.CarryingFood = 0;
        waiter3.Step = 0;
        waiter3.X = 13;
        waiter3.Y = 0;

        Waiter waiter4 = waiters[4];
        waiter4.CarryingFood = 0;
        waiter4.Step = 0;
        waiter4.X = 17;
        waiter4.Y = 0;
    }
}
