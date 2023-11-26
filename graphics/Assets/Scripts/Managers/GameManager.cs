/// <summary>
/// The GameManager class is responsible for spawning the paviment, robots and trees.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

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
    public TextMeshProUGUI stepCounter;
    public TextMeshProUGUI simulationComplete;

    [HideInInspector]
    public int width;

    [HideInInspector]
    public int height;

    [HideInInspector]
    public Dictionary<string, GameObject> spawnedWaiters = new Dictionary<string, GameObject>();

    [HideInInspector]
    public Dictionary<string, GameObject> spawnedFood = new Dictionary<string, GameObject>();

    [HideInInspector]
    public int steps = 0;

    [HideInInspector]
    public Waiter[] waiters;


    [HideInInspector]
    public bool isBinFound = false;

    private Bin bin;
    private float stepUpdateTime = 1f;
    private float foodUpdateTime = 5f;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        simulationComplete.gameObject.SetActive(false);
        StartCoroutine(InitializeGame());
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
            pavimentSpawner.SpawnPaviment(gameState.Bin);

            BinSpawner binSpawner = GetComponentInChildren<BinSpawner>();
            binSpawner.SpawnBin(gameState.Bin);

            steps = 0;

            InvokeRepeating(nameof(UpdateSteps), 0, stepUpdateTime);
            InvokeRepeating(nameof(GenerateFood), 0, foodUpdateTime);
        }
        else
        {
            Debug.LogError("Error loading data");
            EditorApplication.isPlaying = false;
        }
    }

    /// <summary>
    /// The UpdateUpdateTimes method is responsible for updating the step and food update time.
    /// </summary>
    public void UpdateUpdateTimes(float newStepTime, float newFoodTime)
    {
        stepUpdateTime = newStepTime;
        foodUpdateTime = newFoodTime;
        UpdateWaiterSpeed(newStepTime);
        RestartInvocations();
    }

    /// <summary>
    /// The UpdateWaiterSpeed method is responsible for updating the waiter speed.
    /// </summary>
    public void UpdateWaiterSpeed(float newStepTime)
    {
        foreach (var waiter in waiters)
        {
            Waiter waiterObject = waiter.GetComponent<Waiter>();
            waiterObject.AdjustInterpolationFrames(newStepTime);
        }
    }

    /// <summary>
    /// The RestartInvocations method is responsible for restarting the invocations.
    /// </summary>
    private void RestartInvocations()
    {
        CancelInvoke(nameof(UpdateSteps));
        CancelInvoke(nameof(GenerateFood));

        InvokeRepeating(nameof(UpdateSteps), 0, stepUpdateTime);
        InvokeRepeating(nameof(GenerateFood), 0, foodUpdateTime);
    }

    /// <summary>
    /// The UpdateSteps method is responsible for updating the steps.
    /// </summary>
    private void UpdateSteps()
    {
        StartCoroutine(UpdateStepsCoroutine());
    }

    /// <summary>
    /// The UpdateStepsCoroutine method is responsible for updating the steps.
    /// </summary>
    private IEnumerator UpdateStepsCoroutine()
    {
        Task<string> stepDataTask = APIHelper.FetchDataFromAPI("http://127.0.0.1:5000/step");
        yield return new WaitUntil(() => stepDataTask.IsCompleted);

        if (stepDataTask.Status == TaskStatus.RanToCompletion)
        {
            string stepData = stepDataTask.Result;
            GameState stepState = APIHelper.ParseJsonToGameState(stepData);

            if (stepState.modelIsRunning)
            {
                steps = stepState.currentStep;
                stepCounter.text = "Step: " + steps.ToString();

                UpdateBinFoundPosition(stepState.isBinFound);
                UpdateWaiters(stepState.Waiters);
                UpdateFood(stepState.Food);
            }
            else {
                simulationComplete.gameObject.SetActive(true);
                CancelInvoke(nameof(UpdateSteps));
                CancelInvoke(nameof(GenerateFood));
            }

        }
        else
        {
            Debug.LogError("Error loading step data");
            EditorApplication.isPlaying = false;
        }
    }

    /// <summary>
    /// The endSimulation method is responsible for ending the simulation.
    /// </summary>
    void endSimulation()
    {
        EditorApplication.isPlaying = false;
    }   

    /// <summary>
    /// The UpdateBinFoundPosition method is responsible for updating the bin found position state.
    /// </summary>
    private void UpdateBinFoundPosition(bool isBinFoundData)
    {
        if (isBinFoundData)
        {
            isBinFound = isBinFoundData;
        }
    }

    /// <summary>
    /// The UpdateWaiters method is responsible for updating the waiters.
    /// </summary>
    private void UpdateWaiters(Dictionary<string, WaiterModel[]> waiters)
    {
        foreach (var waiterGroup in waiters)
        {
            foreach (var waiter in waiterGroup.Value)
            {
                WaiterSpawner waiterSpawner = GetComponentInChildren<WaiterSpawner>();
                Vector3 newPosition = new Vector3(waiter.X, 0, waiter.Y);
                waiterSpawner.ManageWaiters(waiterGroup.Key, newPosition, waiter.CarryingFood);
            }
        }
    }

    /// <summary>
    /// The GenerateFood method is responsible for generating the food.
    /// </summary>
    private void GenerateFood()
    {
        StartCoroutine(GenerateFoodCoroutine());
    }

    /// <summary>
    /// The GenerateFoodCoroutine method is responsible for generating the food.
    /// </summary>
    private IEnumerator GenerateFoodCoroutine()
    {
        Task foodGenerationTask = TriggerFoodGeneration();
        yield return new WaitUntil(() => foodGenerationTask.IsCompleted);

        if (foodGenerationTask.Status != TaskStatus.RanToCompletion)
        {
            Debug.LogError("Error generating food");
            EditorApplication.isPlaying = false;
        }
    }

    /// <summary>
    /// The TriggerFoodGeneration method is responsible for triggering the food generation.
    /// </summary>
    private async Task TriggerFoodGeneration()
    {
        await APIHelper.FetchDataFromAPI("http://127.0.0.1:5000/food");
    }

    /// <summary>
    /// The UpdateFood method is responsible for updating the food.
    /// </summary>
    private void UpdateFood(Dictionary<string, FoodModel> foods)
    {
        foreach (var foodItem in foods)
        {
            Vector3 newPosition = new Vector3(foodItem.Value.X, 0, foodItem.Value.Y);
            FoodSpawner foodSpawner = GetComponentInChildren<FoodSpawner>();
            foodSpawner.AddFood(foodItem.Key, newPosition);
        }
    }
}
