#nullable enable

namespace TextRPG.Core.Models;

public class Location
{
    public string Id { get; }
    public string Name { get; }
    public string Description { get; }
    public string ImagePath { get; }
    public List<string> ConnectedLocations { get; }

    public Location(string id, string name, string description, string imagePath)
    {
        Id = id;
        Name = name;
        Description = description;
        ImagePath = imagePath;
        ConnectedLocations = new List<string>();
    }

    public void AddConnection(string locationId)
    {
        if (!ConnectedLocations.Contains(locationId))
        {
            ConnectedLocations.Add(locationId);
        }
    }

    public bool CanTravelTo(string locationId)
    {
        return ConnectedLocations.Contains(locationId);
    }
}