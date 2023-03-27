using System.Text.Json.Serialization;

namespace ApiWeb.Models;

public class Classification
{
    public int ClassificationId {get; set;}
    public string? Type {get; set;}
    public int AgeRating {get; set;}
    
    [JsonIgnore]
    public List<Game> Game = new List<Game>();
}