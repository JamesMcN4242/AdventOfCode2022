var blueprints = Array.ConvertAll(File.ReadAllLines("input.txt"), str => new Blueprint(str));

Console.WriteLine($"Part One: {blueprints.Sum(bp => RunSimulation(bp) * bp.Id, 24)}");
Console.WriteLine($"Part Two: {RunSimulation(blueprints[0], 32) * RunSimulation(blueprints[1], 32) * RunSimulation(blueprints[2], 32)}");

int RunSimulation(Blueprint blueprint, int minutesTotal)
{
  // This value needs to be so high because my branch pruning is sub-par.
  // Otherwise the examples don't work well enough.
  const int branchesConsidered = 300_000;
  var keptBranches = new List<Branch>(branchesConsidered * (int)ResourceType.COUNT) { new() };
  var nextBranches = new List<Branch>(branchesConsidered * (int)ResourceType.COUNT);

  int minuteOn = 0;
  while (++minuteOn <= minutesTotal)
  {
    Console.WriteLine($"Minute: {minuteOn}. Branches to action: {keptBranches.Count}.");
    if (keptBranches.Count > branchesConsidered)
    {
      // Find where it goes wrong. Something is in the wrong order.
      var found = keptBranches.Find(branch =>
        branch.Resources[0] == 1 && branch.Resources[1] == 1 && branch.Resources[2] == 1 && branch.Resources[3] == 1 && 
                  branch.Robots[0] == 1 && branch.Robots[1] == 1 && branch.Robots[2] == 1 && branch.Robots[3] == 1);
      
      keptBranches.Sort((x, y) => y.ScoreCurrentBranch(blueprint).CompareTo(x.ScoreCurrentBranch(blueprint)));
      keptBranches.RemoveRange(branchesConsidered,  keptBranches.Count - branchesConsidered);
    }

    foreach (var branch in keptBranches)
    {
      for (int i = 0; i < (int)ResourceType.COUNT; ++i)
      {
        if (branch.CantMakeRobot.Contains(i)) continue;
        
        bool all = blueprint.Options[i].costOfBuild.All(item => item.cost <= branch.Resources[(int)item.type]);

        if (!all) continue;

        Branch newBranch = new Branch(blueprint, (ResourceType)i, branch);
        nextBranches.Add(newBranch);

        // Always chose this option when it is available.
        if (i == (int)ResourceType.GEODE) break;
      }
      
      // Case where we do nothing.
      var nothingHappened = new Branch(blueprint, ResourceType.COUNT, branch);
      for (int i = 0; i < (int)ResourceType.COUNT; ++i)
      {
        if (branch.CantMakeRobot.Contains(i)) continue;
        
        if(blueprint.Options[i].costOfBuild.All(item => item.cost <= branch.Resources[(int)item.type]))
          nothingHappened.CantMakeRobot.Add(i);
      }

      nextBranches.Add(nothingHappened);
    }

    (keptBranches, nextBranches) = (nextBranches, keptBranches);
    nextBranches.Clear();
  }

  var max = keptBranches.Max(branch => branch.Resources[(int)ResourceType.GEODE]);
  keptBranches.Sort((x,y) => x.Resources[(int)ResourceType.GEODE].CompareTo(y.Resources[(int)ResourceType.GEODE]));
  return max;
}

enum ResourceType {
  ORE,
  CLAY,
  OBSIDIAN,
  GEODE,
  COUNT
}

class Blueprint
{
  public int Id { get; }
  public List<(ResourceType typeBuilt, List<(ResourceType type, int cost)> costOfBuild)> Options { get; }
  
  public Blueprint(string input)
  {
    Id = int.Parse(input.Substring(10, input.IndexOf(':') - 10));
    Options = new List<(ResourceType typeBuilt, List<(ResourceType type, int cost)> costOfBuild)>(); 

    var sections = input.Split("Each ");
    for (int i = 1; i < sections.Length; ++i)
    {
      var type = Enum.Parse<ResourceType>(sections[i].Substring(0, sections[i].IndexOf(' ')), true);
      var cost = new List<(ResourceType type, int cost)>();
      var costSection = sections[i].Substring(sections[i].IndexOf("costs ") + 6).Split("and ");

      foreach(var costStr in costSection)
      {
        var amountAndType = costStr.Split(' ');
        cost.Add((Enum.Parse<ResourceType>(amountAndType[1].Replace(".", ""), true), int.Parse(amountAndType[0])));
      }

      Options.Add((type, cost));
    }

    Options.Sort((x, y) => x.typeBuilt.CompareTo(y.typeBuilt));
  }
}

class Branch
{
  public int[] Resources { get; }
  public int[] Robots { get; }

  public HashSet<int> CantMakeRobot { get; } = new((int)ResourceType.COUNT);

  public Branch()
  {
    Resources = new [] { 0, 0, 0, 0 };
    Robots = new [] { 1, 0, 0, 0 };
  }
  
  public Branch(Blueprint bluePrint, ResourceType chosen, Branch from)
  {
    Resources = new int[(int)ResourceType.COUNT];
    Robots = new int[(int)ResourceType.COUNT];
    
    Array.Copy(from.Resources, Resources, (int)ResourceType.COUNT);
    Array.Copy(from.Robots, Robots, (int)ResourceType.COUNT);

    for (int i = 0; i < Robots.Length; ++i)
    {
      Resources[i] += Robots[i];
    }

    if (chosen == ResourceType.COUNT) return;
    ++Robots[(int)chosen];
    foreach (var costToTake in bluePrint.Options[(int)chosen].costOfBuild)
    {
      Resources[(int)costToTake.type] -= costToTake.cost;
    }
  }

  public int ScoreCurrentBranch(Blueprint blueprint) =>
    Resources[(int)ResourceType.GEODE] + Robots[0] + Robots[1] * 2 + Robots[2] * 3 + Robots[3] * 6;
}