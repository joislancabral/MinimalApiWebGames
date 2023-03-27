using System.Text.Json.Serialization;

namespace ApiWeb.Models;

public class Game
{
    public int GameId {get; set;}
    public string? Name {get; set;}
    public int Quantity {get; set;}
    public decimal Price {get; set;}

    public int ClassificationId {get; set;}
    [JsonIgnore]
    public Classification? Classification {get; set;}
}