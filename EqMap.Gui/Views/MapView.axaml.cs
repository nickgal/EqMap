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
        LoadMap("airplane"); // two layers
        // LoadMap("bazaar"); // single layer mixed
    }

    public void LoadMap(string mapName)
    {
        _map = _mapLoader.Load(mapName);
        if (_map == null)
        {
            Console.WriteLine($"Failed to load {mapName} map");
            return;
        }

        Children.Clear();

        foreach (var layer in _map.Layers)
        {
            RenderLayer(layer);
        }
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

        foreach (var labelElement in layer.Elements.OfType<Label>())
        {
            var size = labelElement.Size + 10;
            var color = Color.FromUInt32((uint)labelElement.Color.ToArgb());
            var circle = new Avalonia.Controls.Shapes.Ellipse
            {
                Width = size,
                Height = size,
                Fill = new SolidColorBrush(color),
            };
            SetTop(circle, labelElement.Position.Y);
            SetLeft(circle, labelElement.Position.X);
            Children.Add(circle);

            var textBlock = new TextBlock
            {
                Text = labelElement.Text.Replace('_', ' '),
                Foreground = new SolidColorBrush(color),
                FontSize = size,
            };
            SetTop(textBlock, labelElement.Position.Y - 5);
            SetLeft(textBlock, labelElement.Position.X + 15);
            Children.Add(textBlock);
        }
    }
}
