using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace EqMap.Gui.Views;

public partial class MapView : Canvas
{
    private MapLoader _mapLoader;
    private Map? _map;

    public MapView()
    {
        InitializeComponent();

        _mapLoader = new MapLoader(@"C:\Code\EqMap\Maps");
        LoadMap("airplane");
    }

    public void LoadMap(string mapName)
    {
        _map = _mapLoader.Load(mapName);
        if (_map == null)
        {
            Console.WriteLine($"Failed to load {mapName} map");
            return;
        }

        var geometryLayer = _map.Layers[0];
        RenderLayer(geometryLayer);
    }

    private void RenderLayer(Layer layer)
    {
        foreach (var lineElement in layer.Elements.OfType<Line>())
        {
            var color = Color.FromUInt32((uint)lineElement.Color.ToArgb());
            Children.Add(new Avalonia.Controls.Shapes.Line
            {
                StartPoint = new Point(lineElement.Start.X, lineElement.Start.Y),
                EndPoint = new Point(lineElement.End.X, lineElement.End.Y),
                Stroke = new SolidColorBrush(color),
            });
        }
    }
}
