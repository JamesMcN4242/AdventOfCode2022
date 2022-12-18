var input = File.ReadAllLines("input.txt").Select(str => str.Split(':').Select(segment => (
  x: long.Parse(segment.Substring(segment.IndexOf("x=") + 2, segment.IndexOf(",") - segment.IndexOf("x=") - 2)),
  y: long.Parse(segment.Substring(segment.IndexOf("y=") + 2))))).Select(set => (coords: set, dist: Distance(set.First(), set.Last()))).ToList();

Console.WriteLine($"Part One: {PartOne()}");

const long maxBoundary = 4000000;
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
  var overlapPoints = new HashSet<(long x, long y)>();
  for (int i = 0; i < input.Count(); ++i)
  {
    var cornersI = GetCornerPoints(input[i].coords.First(), input[i].dist);
    
    for (int j = i + 1; j < input.Count() - 1; ++j)
    {
      if (input[i].dist + input[j].dist < Distance(input[i].coords.First(), input[j].coords.First())) continue;
      var cornersJ = GetCornerPoints(input[j].coords.First(), input[j].dist);
      TryAddOverlapPositions(cornersI, cornersJ, overlapPoints);
    }
  }

  // Since we're trying to hit exact edges we'll give it some safety margins.
  foreach (var overlapPoint in overlapPoints)
  {
    for (long i = -2; i <= 2; ++i)
    {
      for (long j = -2; j <= 2; ++j)
      {
        var position = (x: overlapPoint.x + i, y: overlapPoint.y + j);
        if (position.x is >= 0 and <= maxBoundary
            && position.y is >= 0 and <= maxBoundary
            && !input.Any(set => set.dist >= Distance(set.coords.First(), position)))
          return (ulong)position.x * 4000000ul + (ulong)position.y;
      }
    }
  }

  return 0;
}

void TryAddOverlapPositions((long x, long y)[] cornersOne, (long x, long y)[] cornersTwo,
  HashSet<(long x, long y)> toAddTo)
{
  // Intersect the line segments of each bounding box.
  // We do this because theoretically the point we're searching for needs to be right next to an intersection.
  for (int i = 0; i < cornersOne.Length; ++i)
  {
    var p1 = cornersOne[i];
    var p2 = cornersOne[(i + 1) % cornersOne.Length];
    
    for (int j = 0; j < cornersTwo.Length; ++j)
    {
      // Co-linear
      if(i == j) continue;
      
      var p3 = cornersTwo[j];
      var p4 = cornersTwo[(j + 1) % cornersTwo.Length];

      var divisor = (double)(p1.x - p2.x) * (p3.y - p4.y) - (p1.y - p2.y) * (p3.x - p4.x);
      if(divisor == 0) continue;
      
      var t = ((p1.x - p3.x) * (p3.y - p4.y) - (p1.y - p3.y) * (p3.x - p4.x)) / divisor;
      if (t is < 0.0 or > 1.0) continue;
      
      var point = (x: p1.x + (long)(t * (p2.x - p1.x)), y: p1.y + (long)(t * (p2.y - p1.y))); 
      
      if (point.x is >= 0 and <= maxBoundary && point.y is >= 0 and <= maxBoundary)
      {
        toAddTo.Add(point);
      }
    }
  }
}

long Distance((long x, long y) coordOne, (long x, long y) coordTwo)
{
  return Math.Abs(coordOne.x - coordTwo.x) + Math.Abs(coordOne.y - coordTwo.y);
}

(long x, long y)[] GetCornerPoints((long x, long y) centre, long dist) =>
  new[]
  {
    (centre.x, centre.y + dist),
    (centre.x + dist, centre.y),
    (centre.x, centre.y - dist),
    (centre.x - dist, centre.y)
  };