//Setup
var mapInput = Array.ConvertAll(File.ReadAllLines("input.txt"), str => str.ToCharArray());
var setLocations = Array.ConvertAll(mapInput, arr => Array.ConvertAll(arr, cr => cr == 'S' ? 0 : -1));

var startLocation = FindChar('S');
var endLocation = FindChar('E');
mapInput[startLocation.y][startLocation.x] = 'a';
mapInput[endLocation.y][endLocation.x] = 'z';

var toCheck = new HashSet<(int y, int x)>() { startLocation };
var toCheckNext = new HashSet<(int y, int x)>();

MoveToGoal();
Console.WriteLine($"Part One: {setLocations[endLocation.y][endLocation.x]}");

setLocations = Array.ConvertAll(mapInput, arr => Array.ConvertAll(arr, cr => cr == 'a' ? 0 : -1));
toCheck = new HashSet<(int y, int x)>(FindAllChars('a'));
toCheckNext.Clear();
MoveToGoal();
Console.WriteLine($"Part Two: {setLocations[endLocation.y][endLocation.x]}");

void MoveToGoal()
{
  foreach(var pos in toCheck)
  {
    int valueToSet = setLocations[pos.y][pos.x] + 1;
    char comingFrom = mapInput[pos.y][pos.x];

    TryUpdateTile(pos.y - 1, pos.x, valueToSet, comingFrom);
    TryUpdateTile(pos.y + 1, pos.x, valueToSet, comingFrom);
    TryUpdateTile(pos.y, pos.x - 1, valueToSet, comingFrom);
    TryUpdateTile(pos.y, pos.x + 1, valueToSet, comingFrom);
  }

  if (setLocations[endLocation.y][endLocation.x] != -1 || toCheckNext.Count == 0) return;
  SwitchHashSets();
  MoveToGoal();
}

void TryUpdateTile(int y, int x, int valueToSet, char comingFrom)
{
  if (y < 0 || y >= mapInput.Length || x < 0 || x >= mapInput[y].Length || setLocations[y][x] != -1 || mapInput[y][x] > comingFrom + 1) return;

  toCheckNext.Add((y, x));
  setLocations[y][x] = valueToSet;
}

void SwitchHashSets()
{
  var lastChecked = toCheck;
  toCheck = toCheckNext;
  toCheckNext = lastChecked;
  toCheckNext.Clear();
}

(int y, int x) FindChar(char toFind)
{
  for (int i = 0; i < mapInput.Length; ++i)
  {
    for (int j = 0; j < mapInput[0].Length; ++j)
    {
      if (mapInput[i][j] == toFind) return (i, j);
    }
  }
  throw new ArgumentException("The char was not found in the map.");
}

IEnumerable<(int y, int x)> FindAllChars(char toFind)
{
  var returnVals = new List<(int y, int x)>();

  for (int i = 0; i < mapInput.Length; ++i)
  {
    for (int j = 0; j < mapInput[0].Length; ++j)
    {
      if (mapInput[i][j] == toFind) returnVals.Add((i, j));
    }
  }

  return returnVals;
}
