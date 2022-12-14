var rockFormations = Array.ConvertAll(File.ReadAllLines("input.txt"), str => str.Split(" -> ").Select(sArr => sArr.Split(',').Select(sInt => int.Parse(sInt))));
var minCoordY = rockFormations.Max(arr => arr.Max(intArr => intArr.Last()));
var map = GetStartingMap();

int partOne = RunSandSimulation(false);
Console.WriteLine($"Part One: {partOne}");
Console.WriteLine($"Part Two: {RunSandSimulation(true) + partOne}");

int RunSandSimulation(bool floorExists)
{
  int sandUnitRested = 0;

  while (map[(0, 500)] != 'O')
  {
    var sandPos = (y: 0, x: 500);
    bool moved = true;

    while (moved && (floorExists || sandPos.y < minCoordY))
    {
      if (floorExists && sandPos.y == minCoordY + 1) moved = false; 
      else if (!map.ContainsKey((sandPos.y + 1, sandPos.x))) ++sandPos.y;
      else if (!map.ContainsKey((sandPos.y + 1, sandPos.x - 1)))
      {
        ++sandPos.y;
        --sandPos.x;
      }
      else if (!map.ContainsKey((sandPos.y + 1, sandPos.x + 1)))
      {
        ++sandPos.y;
        ++sandPos.x;
      }
      else moved = false;
    }

    if (moved) break;
    map[sandPos] = 'O';
    ++sandUnitRested;
  }

  return sandUnitRested;
}

Dictionary<(int y, int x), char> GetStartingMap()
{
  var createdMap = new Dictionary<(int y, int x), char>();

  // Add rocks and sand fall position
  createdMap.Add((0, 500), 'X');

  foreach (var rockFormation in rockFormations)
  {
    for (int i = 1; i < rockFormation.Count(); ++i)
    {
      var rockPosOne = (y: rockFormation.ElementAt(i - 1).Last(), x: rockFormation.ElementAt(i - 1).First());
      var rockPosTwo = (y: rockFormation.ElementAt(i).Last(), x: rockFormation.ElementAt(i ).First());
      var startPos = (y: rockPosOne.y >= rockPosTwo.y ? rockPosTwo.y : rockPosOne.y, x: rockPosOne.x >= rockPosTwo.x ? rockPosTwo.x : rockPosOne.x);
      var endPos = (y: rockPosOne.y < rockPosTwo.y ? rockPosTwo.y : rockPosOne.y, x: rockPosOne.x < rockPosTwo.x ? rockPosTwo.x : rockPosOne.x);

      for (int x = startPos.x; x <= endPos.x; ++x)
      {
        createdMap[(startPos.y, x)] = '#';
      }

      for (int y = startPos.y; y <= endPos.y; ++y)
      {
        createdMap[(y, startPos.x)] = '#';
      }
    }
  }

  return createdMap;
}
