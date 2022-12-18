var input = Array.ConvertAll(File.ReadAllLines("input.txt"), str => new Valve(str));
var namedLookup = input.ToDictionary(arr => arr.ValveName);
var possibleTargets = input.Where(val => val.FlowRate > 0).ToList();
possibleTargets.Sort((x, y) => x.FlowRate.CompareTo(y.FlowRate));
var fastestRoutes = GetAllFastestRoutesBetween();

List<(int released, HashSet<string> path)> p2Results = null;
Console.WriteLine($"Part One: {RunSimulation(30)}");

p2Results = new List<(int released, HashSet<string> path)>(possibleTargets.Count * possibleTargets.Count);
Console.WriteLine($"Part Two: {PartTwo()}");

int RunSimulation(int minutesRemaining, int scoreSoFar = 0, string nodeAt = "AA")
{
    Valve onValve = namedLookup[nodeAt];
    onValve.On = true;
    scoreSoFar += onValve.FlowRate * minutesRemaining;
    
    int newBestScore = scoreSoFar;
    foreach (var target in possibleTargets)
    {
        if (target.On) continue;
        
        int timeToTurnOn = fastestRoutes[(nodeAt, target.ValveName)] + 1;
        if (minutesRemaining < timeToTurnOn) continue;
        newBestScore = Math.Max(newBestScore,
            RunSimulation(minutesRemaining - timeToTurnOn, scoreSoFar, target.ValveName));
        target.On = false;
    }

    if (p2Results != null)
        p2Results.Add((scoreSoFar, new HashSet<string>(possibleTargets.Where(valve => valve.On && valve.ValveName != "AA").Select(valve => valve.ValveName))));
    
    return newBestScore;
}

int PartTwo()
{
    // It's much faster to generate every possible path, then find the top two which do no overlap.
    // Trying to do something similar to part one's solution but with two actors instead would take hours to compute.
    RunSimulation(26);
    p2Results.Sort((x, y) => y.released.CompareTo(x.released));
    
    int maxDistinct = 0;
    for (int i = 0; i < p2Results.Count; ++i)
    {
        for (int j = i + 1; j < p2Results.Count; ++j)
        {
            if (!p2Results[i].path.Overlaps(p2Results[j].path))
            {
                maxDistinct = Math.Max(maxDistinct, p2Results[i].released + p2Results[j].released);
                break;
            }
        }
    }
    
    return maxDistinct;
}

Dictionary<(string, string), int> GetAllFastestRoutesBetween()
{
    var routes = new Dictionary<(string, string), int>(possibleTargets.Count * possibleTargets.Count * 2 + possibleTargets.Count * 2);
    for (var i = 0; i < possibleTargets.Count; ++i)
    {
        var target = possibleTargets[i];
        int fromStart = FastestRouteBetween("AA", target.ValveName);
        routes.Add(("AA", target.ValveName), fromStart);
        routes.Add((target.ValveName, "AA"), fromStart);

        for (var j = i + 1; j < possibleTargets.Count; ++j)
        {
            var targetTwo = possibleTargets[j];
            int betweenCost = FastestRouteBetween(target.ValveName, targetTwo.ValveName);
            routes.Add((target.ValveName, targetTwo.ValveName), betweenCost);
            routes.Add((targetTwo.ValveName, target.ValveName), betweenCost);
        }
    }

    return routes;
}

int FastestRouteBetween(string start, string end)
{
    int movements = 1;
    HashSet<string> checkedValves = new HashSet<string>();
    HashSet<string> toCheck = new HashSet<string>() {start};
    HashSet<string> toCheckNext = new HashSet<string>();
    
    while (toCheck.Count > 0)
    {
        foreach (var checking in toCheck)
        {
            Valve valve = namedLookup[checking];
            if (valve.ConnectsTo.Contains(end)) return movements;

            foreach (var connected in valve.ConnectsTo)
            {
                if (!checkedValves.Contains(connected) && !toCheck.Contains(connected)) toCheckNext.Add(connected);
            }
            checkedValves.Add(checking);
        }

        (toCheck, toCheckNext) = (toCheckNext, toCheck);
        toCheckNext.Clear();
        ++movements;
    }
    
    return -1;
}

class Valve
{
    public int FlowRate { get; }
    public string ValveName { get; }
    public HashSet<string> ConnectsTo { get; }
    public bool On { get; set; } = false;

    public Valve(string line)
    {
        ValveName = line.Substring("Valve ".Length, 2);
        line = line.Remove(0, "Valve OQ has flow rate=".Length);
        FlowRate = int.Parse(line.Substring(0, line.IndexOf(';')));
        line = line.Substring(line.IndexOf("valve") + 6);
        ConnectsTo = new HashSet<string>(line.Replace(" ", "").Split(','));
    }
}