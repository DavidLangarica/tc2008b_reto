using System.Collections.Generic;

public class GameState
{
    public Bin Bin { get; set; }
    public Dictionary<string, FoodModel> Food { get; set; }
    public Grid Grid { get; set; }
    public Dictionary<string, WaiterModel[]> Waiters { get; set; }
    public bool isBinFound { get; set; }
    public int currentStep { get; set; }
}