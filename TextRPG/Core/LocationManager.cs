#nullable enable

namespace TextRPG.Core;

public class LocationManager
{
    public Dictionary<string, Location> Locations { get; }

    public LocationManager()
    {
        Locations = new Dictionary<string, Location>();
        InitializeLocations();
    }

    private void InitializeLocations()
    {
        // Подземный комплекс
        var complex17 = new Location("complex17", "Комплекс 17", 
            "Подземный бункер, построенный для защиты от ядерной войны. Теперь здесь находится ваша база.", 
            "Images/complex_hall.jpeg");
        complex17.AddConnection("surface");

        // Поверхность
        var surface = new Location("surface", "Поверхность", 
            "Пустошь, оставшаяся после ядерной войны. Радиация и мутанты сделали её опасной.", 
            "Images/desert_ruins.jpeg");
        surface.AddConnection("complex17");
        surface.AddConnection("lasvegas");
        surface.AddConnection("sanfrancisco");
        surface.AddConnection("repair_station");
        surface.AddConnection("trading_post");
        surface.AddConnection("bunker42");

        // Города
        var lasVegas = new Location("lasvegas", "Лас-Вегас", 
            "Оазис в пустоши, город греха и развлечений. Здесь можно найти всё, что душе угодно.", 
            "Images/lasvegas.jpeg");
        lasVegas.AddConnection("surface");

        var sanFrancisco = new Location("sanfrancisco", "Сан-Франциско", 
            "Город, переживший ядерную войну. Здесь процветает торговля и технологические инновации.", 
            "Images/sanfrancisco.jpeg");
        sanFrancisco.AddConnection("surface");

        // Технические локации
        var repairStation = new Location("repair_station", "Ремонтная станция", 
            "Заброшенная станция технического обслуживания. Здесь можно починить снаряжение и купить запчасти.", 
            "Images/repair_station.jpeg");
        repairStation.AddConnection("surface");

        var tradingPost = new Location("trading_post", "Торговая застава", 
            "Караванная стоянка, где можно купить и продать товары. Здесь всегда много путешественников.", 
            "Images/trading_post.jpeg");
        tradingPost.AddConnection("surface");

        var bunker42 = new Location("bunker42", "Бункер 42", 
            "Заброшенный подземный бункер. Говорят, здесь проводились секретные эксперименты.", 
            "Images/bunker42.jpeg");
        bunker42.AddConnection("surface");

        Locations.Add(complex17.Id, complex17);
        Locations.Add(surface.Id, surface);
        Locations.Add(lasVegas.Id, lasVegas);
        Locations.Add(sanFrancisco.Id, sanFrancisco);
        Locations.Add(repairStation.Id, repairStation);
        Locations.Add(tradingPost.Id, tradingPost);
        Locations.Add(bunker42.Id, bunker42);
    }

    public Location? GetLocation(string locationId)
    {
        return Locations.TryGetValue(locationId, out var location) ? location : null;
    }

    public IEnumerable<Location> GetAvailableLocations(string currentLocationId)
    {
        if (currentLocationId == "surface")
        {
            return Locations.Values;
        }

        var currentLocation = GetLocation(currentLocationId);
        if (currentLocation == null)
        {
            return Enumerable.Empty<Location>();
        }

        return currentLocation.ConnectedLocations
            .Where(id => Locations.ContainsKey(id))
            .Select(id => Locations[id]);
    }

    public bool CanTravelTo(string fromLocationId, string toLocationId)
    {
        if (fromLocationId == "surface")
        {
            return true;
        }

        var currentLocation = GetLocation(fromLocationId);
        return currentLocation != null && currentLocation.ConnectedLocations.Contains(toLocationId);
    }

    public static int GetInitialSceneId(string locationId)
    {
        return locationId switch
        {
            "lasvegas" => 1,
            "sanfrancisco" => 5,
            "repair_station" => 9,
            "trading_post" => 12,
            "bunker42" => 15,
            _ => 0
        };
    }
} 