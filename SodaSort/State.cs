/*var data = new string[]
{
    "yryr",
    "ryry",
    "____",
    "____"
};*/








// возвращает все успешные подпути
public record State(Bottle[] Bottles)
{
    public Transition[] Transitions;

    public bool IsFinal()
    {
        foreach(var bottle in Bottles)
        {
            if (bottle.IsEmpty() || bottle.IsFinished())
            {
                continue;
            }
            return false;
        }
        return true;
    }

    public Transition[] CalculateTransitions(bool fast = false)
    {
        List<Transition> transitions = new ();
        for (int i = 0; i < Bottles.Length; i++)
        {
            Bottle bottle = Bottles[i];
            if (bottle.IsEmpty() || bottle.IsFinished())
            {
                continue;
            }
            for (int j = 0; j < Bottles.Length; j++)
            {
                if (i == j)
                {
                    continue;
                }
                Bottle targetBottle = Bottles[j];
                if (targetBottle.IsEmpty() || targetBottle.Peek() == bottle.Peek())
                {
                    if (CanMove(i, j))
                    {
                        var path = $"{bottle.Peek()}>{targetBottle.Peek()}:{i}-{j}";
                        var nextState = this with { Bottles = Move(i, j)};
                        var priority = 0;
                        if (!fast)
                        {
                            priority = CalcPriority(nextState, i, j);
                        }
                        transitions.Add(new Transition(path, nextState, priority));
                    }
                }
            }
        }
        return transitions
            .OrderByDescending(t => t.Priority)
            .ToArray();
    }

    public int CalcPriority(State state, int from, int to)
    {
        int result = 0;
        var transitions = state.CalculateTransitions(fast:true);
        // приоритет перехода тем выше, чем больше доступных ходов после него
        result += transitions.Length;
        // если после перехода образуется полная бутылка, то вообще отлично
        if (state.Bottles[to].IsFinished())
        {
            result += 10;
        }
        return result;
    }

    public bool CanMove(int from, int to)
    {
        char[] topLayer = Bottles[from].GetTopLayer();
        Bottle emptiedBottle = Bottles[from].Pop();
        // не переливаем последний слой из одной бутылки в пустую другую, это бесполезно и ведет к зацикливаниям
        if (Bottles[to].IsEmpty() && emptiedBottle.IsEmpty())
        {
            return false;
        }
        // в другой цвет не переливаем
        if (!Bottles[to].IsEmpty() && Bottles[from].Peek() != Bottles[to].Peek())
        {
            return false;
        }
        // если не хватает места то не переливаем
        if (Bottles[to].Capacity() < topLayer.Length)
        {
            return false;
        }
        // если переливаем 3 слоя в 1 чтобы заполнить бутылку, то не переливаем, чтобы не дублировать
        if (topLayer.Length == 3 && Bottles[to].GetTopLayer().Length == 1)
        {
            return false;
        }
        // если переливаем 2 слоя в 2 то переливаем только слева направо чтобы не дублировать
        if (Bottles[to].GetTopLayer().Length == Bottles[from].GetTopLayer().Length && Bottles[to].Capacity() == 2 && Bottles[from].Capacity() == 2)
        {
            return from < to;
        }

        return true;
    }

    public Bottle[] Move(int from, int to)
    {
        var result = new List<Bottle>();
        var layer = Bottles[from].GetTopLayer();
        for (int i = 0; i < Bottles.Length; i++)
        {
            if (i == from)
            {
                result.Add(Bottles[i].Pop());
                continue;
            }
            if (i == to)
            {
                result.Add(Bottles[i].Push(layer));
                continue;
            }
            result.Add(Bottles[i]);
        }
        return result.ToArray();
    }

}
