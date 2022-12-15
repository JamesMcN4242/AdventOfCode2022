var input = File.ReadAllLines("input.txt").Select(str => str.Split(':').Select(segment => (
  x: long.Parse(segment.Substring(segment.IndexOf("x=") + 2, segment.IndexOf(",") - segment.IndexOf("x=") - 2)),
  y: long.Parse(segment.Substring(segment.IndexOf("y=") + 2))))).Select(set => (coords: set, dist: Math.Abs(set.First().x - set.Last().x) + Math.Abs(set.First().y - set.Last().y)));

Console.WriteLine($"Part One: {PartOne()}");
Console.WriteLine($"Part Two: {PartTwo()}");

int PartOne()
{
  const long rowToCheck = 2000000;
  var xValues = new HashSet<long>();

  foreach (var sensorInfo in input)
  {
    var sensor = sensorInfo.coords.First();
    long startX = sensor.x - (sensorInfo.dist - Math.Abs(rowToCheck - sensor.y));
    long endX = sensor.x + (sensorInfo.dist - Math.Abs(rowToCheck - sensor.y));

    for (long x = startX; x <= endX; ++x)
    {
      xValues.Add(x);
    }
  }

  foreach (var sensorPairs in input)
  {
    if (sensorPairs.coords.Last().y == rowToCheck)
      xValues.Remove(sensorPairs.coords.Last().x);
  }

  return xValues.Count;
}

ulong PartTwo()
{
  throw new Exception("Not just completed");

  //const long maxBoundary = 4000000;
  //for(long x = 0; x <= maxBoundary; ++x)
  //{
  //  for (long y=0; y <= maxBoundary; )
  //  {
  //    var firstInteraction = input.FirstOrDefault(set => set.dist < Math.Abs(set.coords.First().x - x) + Math.Abs(set.coords.First().y - y));
  //    if (firstInteraction == default) return (ulong)x * 4000000ul + (ulong)y;
  //  }
  //}

  //positionsInMiddleOfCircles.RemoveWhere(element => element.x < 0 || element.x > maxBoundary || element.y < 0 || element.y > maxBoundary);
  //foreach(var position in positionsInMiddleOfCircles)
  //{
  //  if (!input.Any(set => set.dist >= Math.Abs(set.coords.First().x - position.x) + Math.Abs(set.coords.First().y - position.y)))
  //    return (ulong)position.x * 4000000ul + (ulong)position.y;
  //}

  //// Something went really wrong.
  //return 0;
}
