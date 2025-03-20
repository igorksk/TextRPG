using System.Collections.Generic;

namespace TextRPG.Core
{
    public class Location
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BackgroundImage { get; set; }
        public List<string> ConnectedLocations { get; set; }
        public int? EventSceneId { get; set; }
        public bool EventShown { get; set; }

        public Location(string id, string name, string description, string backgroundImage, int? eventSceneId = null)
        {
            Id = id;
            Name = name;
            Description = description;
            BackgroundImage = backgroundImage;
            EventSceneId = eventSceneId;
            EventShown = false;
            ConnectedLocations = [];
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