/*var data = new string[]
{
    "yryr",
    "ryry",
    "____",
    "____"
};*/
var data = new string[]
{
    "tsoy",
    "rvlc",
    "lptv",
    "yprb",
    "lnbv",
    "lsvy",
    "oscg",
    "rgcp",
    "nnct",
    "gbpo",
    "styo",
    "rbgn",
    "____",
    "____"
};

var InitialBottles = new List<Bottle>();
foreach (var bottle in data)
{
    InitialBottles.Add(new Bottle(bottle.ToCharArray()));
}

State InitialState = new (InitialBottles.ToArray<Bottle>());

List<string> successPaths = new();


FindPaths(InitialState, ">");

foreach (var path in successPaths.OrderBy(p=>p.Length).Take(1))
{
    Console.WriteLine(path);
}

var parts = successPaths.OrderBy(p => p.Length).First().Split(' ');
bool exit = false;
int index = 0;
while(!exit) {
    var key = Console.ReadKey().Key;
        
    if (key == ConsoleKey.Escape)
    {
        exit = true;
        break;
    }

    if (key == ConsoleKey.RightArrow)
    {
        index++;
    }

    if (key == ConsoleKey.LeftArrow)
    {
        index--;
    }
    Console.WriteLine(parts[index]);

}

// возвращает все успешные подпути
void FindPaths(State state, string path)
{
    if (successPaths.Count > 100) return;
    
    // если это состояние конечное, то возвращаем историю, ведущую к нему
    if (state.IsFinal())
    {
        path += " < success";
        successPaths.Add(path);
        return;
    }

    // находим все доступные переходы из этого состояния
    var transitions = state.CalculateTransitions();
    foreach (var transition in transitions)
    {
        // находим переходы от следующего состояния до победы
        FindPaths(transition.NextState, path + " " + transition.Path);
    } 
}
