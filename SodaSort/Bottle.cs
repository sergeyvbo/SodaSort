public record Bottle(char[] Layers)
{
    public char this[int index] => Layers[index];

    public bool IsEmpty() => Peek() == '_';

    public bool IsFinished() => !IsEmpty() && Layers.Distinct().Count() == 1;

    public int Capacity()
    {
        int result = 0;
        foreach (var layer in Layers)
        {
            if (layer != '_')
            {
                break;
            }
            result++;
        }
        return result;
    }

    public char Peek()
    {
        foreach (var layer in Layers)
        {
            if (layer == '_') continue;
            return layer;
        }
        return '_';
    }

    public char[] GetTopLayer()
    {
        List<char> result = new();
        var color = Peek();
        foreach (var layer in Layers)
        {
            if (layer == '_') continue;
            if (layer != color) break;
            result.Add(layer);
        }
        return result.ToArray();
    }

    // возвращает копию бутылки но без верхнего слоя
    public Bottle Pop()
    {
        var color = Peek();
        List<char> newLayers = new();
        var i = 0;
        // добавляем пустоту вместо пустых слоев или слоев изымаемого цвета
        while (i< Layers.Length && (Layers[i] == '_' || Layers[i] == color))
        {
            newLayers.Add('_');
            i++;
        }
        // остальные слои добавляем без изменений
        while (i < Layers.Length)
        {
            newLayers.Add(Layers[i]);
            i++;
        }
        return new Bottle(newLayers.ToArray());
    }
    // возвращает копию бутылки с добавленным слоем
    public Bottle Push(char[] layer)
    {
        List<char> newLayers = new();
        var i = 0;
        // добавляем (емкость-размер добавляемого слоя) пустых слоев
        while (i < Capacity() - layer.Length)
        {
            newLayers.Add('_');
            i++;
        }
        // добавляем новый слой
        foreach (var l in layer)
        {
            newLayers.Add(l);
            i++;
        }
        // добавляем старые слои
        while (i < Layers.Length)
        {
            newLayers.Add(Layers[i]);
            i++;
        }
        return new Bottle(newLayers.ToArray());
    }

    public override string ToString()
    {
        return new string(Layers);
    }
}