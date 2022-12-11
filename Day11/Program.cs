var monkeys = Array.ConvertAll(File.ReadAllText("input.txt").Split("\nMonkey"), str => new Monkey(str));
RunMonkeyPassing(20, true, "Part One: {0}");

// Reset state for part two
monkeys = Array.ConvertAll(File.ReadAllText("input.txt").Split("\nMonkey"), str => new Monkey(str));
RunMonkeyPassing(10000, false, "Part Two: {0}");

void RunMonkeyPassing(int loops, bool divideAfterInspection, string outputFormat)
{
    long allMonkeyProducts = monkeys.Aggregate(1, (i, monkey) => i * monkey.TestDivisionValue);
    for (int i = 0; i < loops; ++i)
    {
        foreach (var monkey in monkeys)
        {
            while (monkey.ItemsHeld.Count > 0)
            {
                ++monkey.Inspected;
                var operatedItem = divideAfterInspection ? 
                    monkey.Operation(monkey.ItemsHeld.Dequeue()) / 3 :
                    monkey.Operation(monkey.ItemsHeld.Dequeue()) % allMonkeyProducts;
                monkeys[monkey.Test(operatedItem)].ItemsHeld.Enqueue(operatedItem);
            }
        }
    }
    
    long maxInspected = monkeys.Max(m => m.Inspected);
    Console.WriteLine(outputFormat, maxInspected * monkeys.Max(m => m.Inspected == maxInspected ? int.MinValue : m.Inspected));
}

class Monkey
{
    public long Inspected { get; set; } = 0;
    public Queue<long> ItemsHeld;
    public Func<long, long> Operation;
    public Func<long, int> Test;
    public int TestDivisionValue;
    
    public Monkey(string textBlock)
    {
        var lines = textBlock.Split('\n');
        
        var startingItems = Array.ConvertAll(lines[1].Replace("Starting items: ", "").Split(", "), str => long.Parse(str));
        ItemsHeld = new Queue<long>(startingItems);

        var opTxt = lines[2].Replace("Operation: new = old ", "").Trim().Split(' ');
        Operation = (long old) =>
        {
            switch (opTxt[0])
            {
                case "*": return old * (opTxt[1] == "old" ? old : long.Parse(opTxt[1]));
                case "+": return old + (opTxt[1] == "old" ? old : long.Parse(opTxt[1]));
                default: throw new NotImplementedException($"Need to implement a {opTxt[0]} operator to use it.");
            }
        };

        TestDivisionValue = int.Parse(lines[3].Replace("Test: divisible by ", ""));
        int monkeyWhenTrue = int.Parse(lines[4].Remove(0, lines[4].LastIndexOf(' ')));
        int monkeyWhenFalse = int.Parse(lines[5].Remove(0, lines[5].LastIndexOf(' ')));
        Test = (long score) => score % TestDivisionValue == 0 ? monkeyWhenTrue : monkeyWhenFalse;
    }
}