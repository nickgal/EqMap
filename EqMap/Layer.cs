namespace EqMap;

public class Layer
{
    public List<IMapElement> Elements;

    public Layer()
    {
        Elements = [];
    }

    public Layer(List<IMapElement> elements)
    {
        Elements = elements;
    }

    public static Layer FromPath(string path)
    {
        var elements = MapParser.Parse(path);
        return new Layer(elements);
    }
}
