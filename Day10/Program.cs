Console.WriteLine($"Part One: {Solve()}");
Console.WriteLine($"Please check the output above for Part Two solution.");

int Solve()
{
    var input = Array.ConvertAll(File.ReadAllLines("input.txt"), str => str.Split(' '));
    int registerVal = 1; int cycle = 1; int addedCycleValues = 0;
    var renders = new List<char>(240);

    foreach (var instruction in input)
    {
        RunCycleUpdate();
        if (instruction[0] == "noop") continue;
        RunCycleUpdate(int.Parse(instruction[1]));
    }
    
    for (var i = 0; i < 6; ++i) 
        Console.WriteLine($"{string.Join("", renders.GetRange(i * 40, 40))}");

    return addedCycleValues;

    void RunCycleUpdate(int toAdd = 0)
    {
        renders.Add(registerVal - 1 <= renders.Count % 40 && registerVal + 1 >= renders.Count % 40 ? '#' : '.');
        registerVal += toAdd;
        ++cycle;
        if (cycle == 20 || (cycle - 20) % 40 == 0 && cycle <= 220) addedCycleValues += cycle * registerVal;
    }
}