var input = File.ReadLines("input.txt")
    .Select(line => line.Split(',').Select(range => range.Split('-').Select(iVal => int.Parse(iVal))));

// Part One - Find pairs with fully overlapping regions
int partOneCount = input.Count(ranges =>
{
    var firstSet = ranges.First().ToArray();
    var secondSet = ranges.Last().ToArray();
    
    return (firstSet[0] <= secondSet[0] && firstSet[1] >= secondSet[1])
        || (secondSet[0] <= firstSet[0] && secondSet[1] >= firstSet[1]);
});

Console.WriteLine($"Part One: {partOneCount}");

// Part Two - Find pairs with any overlapping regions
int partTwoCount = input.Count(ranges =>
{
    var firstSet = ranges.First().ToArray();
    var secondSet = ranges.Last().ToArray();
    
    return (firstSet[0] <= secondSet[0] && firstSet[1] >= secondSet[0])
           || (firstSet[0] <= secondSet[1] && firstSet[1] >= secondSet[1])
           || (secondSet[0] <= firstSet[0] && secondSet[1] >= firstSet[0])
           || (secondSet[0] <= firstSet[1] && secondSet[1] >= firstSet[1]);
});

Console.WriteLine($"Part Two: {partTwoCount}");