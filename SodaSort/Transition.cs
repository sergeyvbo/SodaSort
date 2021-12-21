/*var data = new string[]
{
    "yryr",
    "ryry",
    "____",
    "____"
};*/








// возвращает все успешные подпути
public record Transition(string Path, State NextState, int Priority);