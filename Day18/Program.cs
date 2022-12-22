var positions = Array.ConvertAll(File.ReadAllLines("input.txt").Select(str => str.Split(',').Select(str => int.Parse(str))).ToArray(), line => (x: line.ElementAt(0), y: line.ElementAt(1), z: line.ElementAt(2))).ToHashSet();
(int minX, int maxX) = (positions.Min(input => input.x), positions.Max(input => input.x));
(int minY, int maxY) = (positions.Min(input => input.y), positions.Max(input => input.y));
(int minZ, int maxZ) = (positions.Min(input => input.z), positions.Max(input => input.z));
var airPocketCache = new HashSet<(int x, int y, int z)>();
var airPocketToCheck = new HashSet<(int x, int y, int z)>();
var airPocketToCheckNext = new HashSet<(int x, int y, int z)>();
var discoveredAirPockets = new HashSet<(int x, int y, int z)>();

Console.WriteLine($"Part One: {GetSurfaceArea()}");
Console.WriteLine($"Part Two: {GetSurfaceArea(true)}");

int GetSurfaceArea(bool externallyExposedOnly = false)
{
  int openSides = 0;

  foreach (var pos in positions)
  {
    if (!positions.Contains((pos.x - 1, pos.y, pos.z)) && (!externallyExposedOnly || !IsAirPocket((pos.x - 1, pos.y, pos.z)))) ++openSides;
    if (!positions.Contains((pos.x + 1, pos.y, pos.z)) && (!externallyExposedOnly || !IsAirPocket((pos.x + 1, pos.y, pos.z)))) ++openSides;
    if (!positions.Contains((pos.x, pos.y - 1, pos.z)) && (!externallyExposedOnly || !IsAirPocket((pos.x, pos.y - 1, pos.z)))) ++openSides;
    if (!positions.Contains((pos.x, pos.y + 1, pos.z)) && (!externallyExposedOnly || !IsAirPocket((pos.x, pos.y + 1, pos.z)))) ++openSides;
    if (!positions.Contains((pos.x, pos.y, pos.z - 1)) && (!externallyExposedOnly || !IsAirPocket((pos.x, pos.y, pos.z - 1)))) ++openSides;
    if (!positions.Contains((pos.x, pos.y, pos.z + 1)) && (!externallyExposedOnly || !IsAirPocket((pos.x, pos.y, pos.z + 1)))) ++openSides;
  }

  return openSides;
}

bool IsAirPocket((int x, int y, int z) pos)
{
  if (discoveredAirPockets.Contains(pos)) return true;

  airPocketCache.Clear();
  airPocketToCheck.Clear();
  airPocketToCheckNext.Clear();
  airPocketToCheck.Add(pos);

  while (airPocketToCheck.Count > 0)
  {
    foreach(var airPocket in airPocketToCheck)
    {
      if (airPocket.x < minX || airPocket.x > maxX || airPocket.y < minY || airPocket.y > maxY || airPocket.z < minZ || airPocket.z > maxZ) return false;

      airPocketCache.Add(airPocket);
      if (discoveredAirPockets.Contains(airPocket)) return true;
      if (positions.Contains(airPocket)) continue;

      if (!airPocketCache.Contains((airPocket.x - 1, airPocket.y, airPocket.z))) airPocketToCheckNext.Add((airPocket.x - 1, airPocket.y, airPocket.z));
      if (!airPocketCache.Contains((airPocket.x + 1, airPocket.y, airPocket.z))) airPocketToCheckNext.Add((airPocket.x + 1, airPocket.y, airPocket.z));
      if (!airPocketCache.Contains((airPocket.x, airPocket.y - 1, airPocket.z))) airPocketToCheckNext.Add((airPocket.x, airPocket.y - 1, airPocket.z));
      if (!airPocketCache.Contains((airPocket.x, airPocket.y + 1, airPocket.z))) airPocketToCheckNext.Add((airPocket.x, airPocket.y + 1, airPocket.z));
      if (!airPocketCache.Contains((airPocket.x, airPocket.y, airPocket.z - 1))) airPocketToCheckNext.Add((airPocket.x, airPocket.y, airPocket.z - 1));
      if (!airPocketCache.Contains((airPocket.x, airPocket.y, airPocket.z + 1))) airPocketToCheckNext.Add((airPocket.x, airPocket.y, airPocket.z + 1));
    }

    var temp = airPocketToCheck;
    airPocketToCheck = airPocketToCheckNext;
    airPocketToCheckNext = temp;
    airPocketToCheckNext.Clear();
  }

  foreach(var airPocket in airPocketCache)
  {
    if (!positions.Contains(airPocket)) discoveredAirPockets.Add(airPocket);
  }
  return true;
}
