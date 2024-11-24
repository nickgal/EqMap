namespace EqMap;

public class Map
{
    public string Name;
    public List<Layer> Layers = [];

    public Map(string name)
    {
        Name = name;
    }

    public Layer AddLayer(string path)
    {
        var layer = Layer.FromPath(path);
        Layers.Add(layer);
        return layer;
    }
}
