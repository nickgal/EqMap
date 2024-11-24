namespace EqMap;

public class MapLoader
{
    private string _mapDirectory;
    private Dictionary<string, List<string>> _mapRegistry = [];
    private Dictionary<string, Map> _maps = [];

    public MapLoader(string mapDirectory)
    {
        _mapDirectory = mapDirectory;
        PopulateMapRegistry();
    }

    public Map? Load(string mapName)
    {
        mapName = mapName.ToLower();

        if (_maps.TryGetValue(mapName, out var map))
        {
            return map;
        }

        if (!_mapRegistry.TryGetValue(mapName, out var mapFiles))
        {
            return null;
        }

        map = new Map(mapName);
        foreach (var mapFile in mapFiles)
        {
            map.AddLayer(Path.Join(_mapDirectory, mapFile));
        }

        _maps.Add(mapName, map);

        return map;
    }

    private void PopulateMapRegistry()
    {
        var txtFiles = Directory.EnumerateFiles(_mapDirectory, "*.txt", SearchOption.TopDirectoryOnly);
        foreach (var txtFile in txtFiles)
        {
            var fileName = Path.GetFileName(txtFile);
            var mapName = Path.GetFileNameWithoutExtension(fileName.Split('_')[0].ToLower());
            if (_mapRegistry.TryGetValue(mapName, out var mapFiles))
            {
                mapFiles ??= [];
                mapFiles.Add(fileName);
            }
            else
            {
                _mapRegistry.Add(mapName, [fileName]);
            }
        }
    }
}
