/// <summary>
/// The GameManager class is responsible for spawning the paviment, robots and trees.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

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

    private int numWaiters = 5;

    [HideInInspector]
    public int width;
    [HideInInspector]
    public int height;

    [HideInInspector]
    public Waiter[] waiters;

    [HideInInspector]
    public int[] binPosition = {13, 0};

    private float stepUpdateTime = 1.0f;
    private float foodUpdateTime = 5.0f;

    [HideInInspector]
    public Dictionary<string, GameObject> spawnedWaiters = new Dictionary<string, GameObject>();
    [HideInInspector]
    public Dictionary<string, GameObject> spawnedFood = new Dictionary<string, GameObject>();

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        StartCoroutine(InitializeGame());
        InvokeRepeating(nameof(UpdateSteps), 0, stepUpdateTime);
        InvokeRepeating(nameof(GenerateFood), 0, foodUpdateTime);
    }

    
    /// <summary>
    /// The InitializeGame method is responsible for initializing the game.
    /// </summary>
    private IEnumerator InitializeGame()
    {
        Task<GameState> loadDataTask = LoadDataAsync();
        yield return new WaitUntil(() => loadDataTask.IsCompleted);

        if (loadDataTask.Status == TaskStatus.RanToCompletion)
        {
            GameState gameState = loadDataTask.Result;
            setGridAttributes(gameState.Grid);

            PavimentSpawner pavimentSpawner = GetComponentInChildren<PavimentSpawner>();
            pavimentSpawner.SpawnPaviment();
        }
        else
        {
            Debug.LogError("Error loading data");
        }
    }

    /// <summary>
    /// The LoadDataAsync method is responsible for loading the data from the API.
    /// </summary>
    private async Task<GameState> LoadDataAsync()
    {
        string data = await APIHelper.FetchDataFromAPI("http://127.0.0.1:5000/init");
        return APIHelper.ParseJsonToGameState(data);
    }

    /// <summary>
    /// The setGridAttributes method is responsible for setting the grid attributes.
    /// </summary>
    private void setGridAttributes(Grid gridData)
    {
        width = gridData.Width;
        height = gridData.Height;
    }

    private void UpdateSteps()
    {
        StartCoroutine(UpdateStepsCoroutine());
    }

    private IEnumerator UpdateStepsCoroutine()
    {
        Task<string> stepDataTask = APIHelper.FetchDataFromAPI("http://127.0.0.1:5000/step");
        yield return new WaitUntil(() => stepDataTask.IsCompleted);

        if (stepDataTask.Status == TaskStatus.RanToCompletion)
        {
            string stepData = stepDataTask.Result;
            GameState stepState = APIHelper.ParseJsonToGameState(stepData);

            UpdateWaiters(stepState.Waiters);
            UpdateFood(stepState.Food);
        }
        else
        {
            Debug.LogError("Error loading step data");
        }
    }

    private void UpdateWaiters(Dictionary<string, Waiter[]> waiters)
    {
        foreach (var waiterGroup in waiters)
        {
            foreach (var waiter in waiterGroup.Value)
            {
                WaiterSpawner waiterSpawner = GetComponentInChildren<WaiterSpawner>();
                Vector3 newPosition = new Vector3(waiter.X, 0, waiter.Y);
                waiterSpawner.ManageWaiters(waiterGroup.Key, newPosition);
            }
        }
    }

    private void GenerateFood()
    {
        StartCoroutine(GenerateFoodCoroutine());
    }

    private IEnumerator GenerateFoodCoroutine()
    {
        Task foodGenerationTask = TriggerFoodGeneration();
        yield return new WaitUntil(() => foodGenerationTask.IsCompleted);
    }

    private async Task TriggerFoodGeneration()
    {
        await APIHelper.FetchDataFromAPI("http://127.0.0.1:5000/food");
    }

    private void UpdateFood(Dictionary<string, Food> foods)
    {
        foreach (var foodItem in foods)
        {
            Vector3 newPosition = new Vector3(foodItem.Value.X, 0, foodItem.Value.Y);
            FoodSpawner foodSpawner = GetComponentInChildren<FoodSpawner>();
            foodSpawner.AddFood(foodItem.Key, newPosition);
        }
    }
}
