using System.Drawing;
using System.Numerics;

namespace EqMap;

public class MapParser
{
    public static List<IMapElement> Parse(string path)
    {
        // return ParseAsync(path).Result;
        return Task.Run(() => ParseAsync(path)).Result;
    }

    public static async Task<List<IMapElement>> ParseAsync(string path)
    {
        var elements = new List<IMapElement>();
        var lines = File.ReadLinesAsync(path);
        await Parallel.ForEachAsync(lines, (line, _token) =>
        {
            var element = ParseElement(line);
            if (element != null)
            {
                elements.Add(element);
            }

            return new ValueTask();
        });

        return elements;
    }

    private static IMapElement? ParseElement(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        return text[0] switch
        {
            'l' or 'L' => ParseLine(text),
            'p' or 'P' => ParseLabel(text),
            _ => null,
        };
    }

    private static IEnumerator<string> SplitLine(string text)
    {
        return text[1..].Trim().Split(',').Select(x => x.Trim()).GetEnumerator();
    }

    private static Line ParseLine(string text)
    {
        var line = new Line();
        using (var enumerator = SplitLine(text))
        {
            line.Start = ReadVector3(enumerator);
            line.End = ReadVector3(enumerator);
            line.Color = ReadColor(enumerator);
        }
        return line;
    }

    private static Label ParseLabel(string text)
    {
        var label = new Label();
        using (var enumerator = SplitLine(text))
        {
            label.Position = ReadVector3(enumerator);
            label.Color = ReadColor(enumerator);
            label.Size = ReadByte(enumerator);
            label.Text = ReadString(enumerator) ?? string.Empty;
        }
        return label;
    }

    private static string? ReadString(IEnumerator<string> enumerator)
    {
        enumerator.MoveNext();
        return enumerator.Current;
    }

    private static float ReadSingle(IEnumerator<string> enumerator)
    {
        var str = ReadString(enumerator);
        return Convert.ToSingle(str);
    }

    private static byte ReadByte(IEnumerator<string> enumerator)
    {
        var str = ReadString(enumerator);
        return Convert.ToByte(str);
    }

    private static Vector3 ReadVector3(IEnumerator<string> enumerator)
    {
        float x = ReadSingle(enumerator);
        float y = ReadSingle(enumerator);
        float z = ReadSingle(enumerator);
        return new Vector3(x, y, z);
    }

    private static Color ReadColor(IEnumerator<string> enumerator)
    {
        byte r = ReadByte(enumerator);
        byte g = ReadByte(enumerator);
        byte b = ReadByte(enumerator);
        return Color.FromArgb(r, g, b);
    }
}
