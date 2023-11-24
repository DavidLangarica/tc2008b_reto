using System.Collections.Generic;

public class GameState
{
    public Bin Bin { get; set; }
    public Dictionary<string, Food> Food { get; set; }
    public Grid Grid { get; set; }
    public Dictionary<string, Waiter[]> Waiters { get; set; }
}