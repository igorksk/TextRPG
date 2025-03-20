#nullable enable

using System.Collections.Generic;

namespace TextRPG.Core
{
    public class Location
    {
        public string Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string ImagePath { get; }
        public int? EventSceneId { get; }
        public List<string> ConnectedLocations { get; }
        public bool EventShown { get; set; }

        public Location(string id, string name, string description, string imagePath, int? eventSceneId)
        {
            Id = id;
            Name = name;
            Description = description;
            ImagePath = imagePath;
            EventSceneId = eventSceneId;
            EventShown = false;
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

        public void MarkEventAsShown()
        {
            EventShown = true;
        }
    }
} 