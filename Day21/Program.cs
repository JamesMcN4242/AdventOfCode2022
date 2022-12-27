using System.Text.RegularExpressions;

Console.WriteLine($"Part One: {PartOne()}");
Console.WriteLine($"Part One: {PartTwo()}");

long PartOne()
{
  var monkeys = Array.ConvertAll(File.ReadAllLines("input.txt"), str => new Monkey(str));
  var monkeyDict = monkeys.Where(monkey => monkey.Value.HasValue).ToDictionary(monkey => monkey.Name, monkey => monkey.Value.Value);

  while (!monkeyDict.ContainsKey("root"))
  {
    RunMonkeyOperations(monkeys, monkeyDict);
  }

  return monkeyDict["root"];
}

long PartTwo()
{
  var monkeys = Array.ConvertAll(File.ReadAllLines("input.txt"), str => new Monkey(str));
  var monkeyDict = monkeys.Where(monkey => monkey.Value.HasValue).ToDictionary(monkey => monkey.Name, monkey => monkey.Value.Value);

  var myMonkey = monkeys.First(monkey => monkey.Name == "humn");
  myMonkey.Value = null;
  myMonkey.DependantOn = new[] { "fakeName", "fakeName" };
  monkeyDict.Remove(myMonkey.Name);

  var rootMonkey = monkeys.First(monkey => monkey.Name == "root");
  bool changes = true;
  while (changes)
  {
    int monkeyDictCount = monkeyDict.Count;
    RunMonkeyOperations(monkeys, monkeyDict);
    changes = monkeyDict.Count != monkeyDictCount;
  }

  // Backtrack in order to find exactly what value we need. Thankfully we only need to contribute to one branch at anyone time.
  long valueToMatch = monkeyDict.ContainsKey(rootMonkey.DependantOn[0]) ? monkeyDict[rootMonkey.DependantOn[0]] : monkeyDict[rootMonkey.DependantOn[1]];
  string monkeyToCheck = !monkeyDict.ContainsKey(rootMonkey.DependantOn[0]) ? rootMonkey.DependantOn[0] : rootMonkey.DependantOn[1];
  while(monkeyToCheck != "humn")
  {
    var monkeyBeingChecked = monkeys.First(monkey => monkey.Name == monkeyToCheck);
    
    monkeyToCheck = !monkeyDict.ContainsKey(monkeyBeingChecked.DependantOn[0]) ? monkeyBeingChecked.DependantOn[0] : monkeyBeingChecked.DependantOn[1];
    valueToMatch = monkeyBeingChecked.Operation.Value switch
    {
      '+' => valueToMatch - monkeyDict[monkeyBeingChecked.DependantOn[monkeyToCheck != monkeyBeingChecked.DependantOn[0] ? 0 : 1]],
      '*' => valueToMatch / monkeyDict[monkeyBeingChecked.DependantOn[monkeyToCheck != monkeyBeingChecked.DependantOn[0] ? 0 : 1]],
      '-' => monkeyToCheck != monkeyBeingChecked.DependantOn[0] ? monkeyDict[monkeyBeingChecked.DependantOn[0]] - valueToMatch : valueToMatch + monkeyDict[monkeyBeingChecked.DependantOn[1]],
      '/' => monkeyToCheck != monkeyBeingChecked.DependantOn[0] ? monkeyDict[monkeyBeingChecked.DependantOn[0]] / valueToMatch : valueToMatch * monkeyDict[monkeyBeingChecked.DependantOn[1]],
      _ => throw new NotImplementedException(),
    };
  }

  return valueToMatch;
}

void RunMonkeyOperations(Monkey[] monkeys, Dictionary<string, long> monkeyDict)
{
  foreach (Monkey monkey in monkeys)
  {
    if (monkey.Value.HasValue || !monkeyDict.ContainsKey(monkey.DependantOn[0]) || !monkeyDict.ContainsKey(monkey.DependantOn[1])) continue;

    monkey.Value = monkey.Operation.Value switch
    {
      '+' => monkeyDict[monkey.DependantOn[0]] + monkeyDict[monkey.DependantOn[1]],
      '-' => monkeyDict[monkey.DependantOn[0]] - monkeyDict[monkey.DependantOn[1]],
      '*' => monkeyDict[monkey.DependantOn[0]] * monkeyDict[monkey.DependantOn[1]],
      '/' => monkeyDict[monkey.DependantOn[0]] / monkeyDict[monkey.DependantOn[1]],
      _ => throw new NotImplementedException(),
    };

    monkeyDict.Add(monkey.Name, monkey.Value.Value);
  }
}

class Monkey
{
  public string Name { get;}
  public string[] DependantOn { get; set; }
  public long? Value { get; set; } = null;
  public char? Operation { get; } = null;

  public Monkey(string line)
  {
    Name = line.Substring(0, line.IndexOf(':'));

    var match = Regex.Match(line, " [0-9]+");
    if (match.Success)
    {
      Value = long.Parse(match.Value);
      DependantOn = Array.Empty<string>();
      return;
    }

    var segments = line.Substring(line.IndexOf(":") + 1).Trim().Split(' ');
    DependantOn = new[] { segments[0], segments[2] };
    Operation = segments[1][0];
  }
}
