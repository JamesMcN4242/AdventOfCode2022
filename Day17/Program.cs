using System.Text;

var inputString = File.ReadAllText("input.txt").Trim();
var rockArrangements = new[]
{
  new []{"####"},
  new []{".#.", "###", ".#." },
  new []{"..#", "..#", "###" },
  new []{"#", "#", "#", "#" },
  new []{"##", "##" }
};

Console.WriteLine($"Part One: {Run(2022)}");
Console.WriteLine($"Part Two: {Run(1000000000000)}");

long Run(long rocksToDrop)
{
  int inputIndex = 0;
  int rockIndex = 0;

  var lastRockToStartingIndexDict = new Dictionary<(int rockIndex, int inputIndex), (List<string> lastFifteenRows, long firstHit, long rockCountOnOccurence)>(rockArrangements.Length * inputString.Length);
  List<string> rows = new List<string>(5000) { "_______" };
  long skippedRowCount = 0;

  for (long rocksFallen = 0; rocksFallen < rocksToDrop; ++rocksFallen, ++rockIndex)
  {
    rockIndex %= rockArrangements.Length;
    var rockToFall = rockArrangements[rockIndex];

    // Try to see if we've been in this configuration before. If so lets back skip as many rocks as possible.
    const int rowsToBacktrack = 15;
    if (rows.Count > rowsToBacktrack)
    {
      var rowRange = rows.GetRange(rows.Count - rowsToBacktrack - 1, rowsToBacktrack);
      if (lastRockToStartingIndexDict.ContainsKey((rockIndex, inputIndex)))
      {
        if (lastRockToStartingIndexDict[(rockIndex, inputIndex)].lastFifteenRows.SequenceEqual(rowRange))
        {
          Console.WriteLine($"Found Repeating Sequence.\nFirst hit: {lastRockToStartingIndexDict[(rockIndex, inputIndex)].firstHit}, now hit on tower height: {rows.Count}." +
            $"\nRock  Occurrence: {lastRockToStartingIndexDict[(rockIndex, inputIndex)].rockCountOnOccurence}, now on rock number {rocksFallen}\n\n");

          var skippedRocks = rocksFallen - lastRockToStartingIndexDict[(rockIndex, inputIndex)].rockCountOnOccurence;
          long skippedLoops = (rocksToDrop - rocksFallen) / skippedRocks;

          skippedRowCount = skippedLoops * (rows.Count - lastRockToStartingIndexDict[(rockIndex, inputIndex)].firstHit);
          rocksFallen += skippedRocks * skippedLoops;
          lastRockToStartingIndexDict.Clear();
        }
      }
      else
      {
        lastRockToStartingIndexDict.Add((rockIndex, inputIndex), (rowRange, rows.Count, rocksFallen));
      }
    }

    // Position is relative to the top left point
    var rockPos = (x: 2, y: rows.Count + rockToFall.Length + 2);

    // Move the rock
    while (true)
    {
      int index = inputIndex++;
      inputIndex %= inputString.Length;

      // X Movement
      int xChange = inputString[index] == '<' ? -1 : 1;
      bool shouldMove = rockPos.x + xChange >= 0 && rockPos.x + xChange + rockToFall[0].Length <= 7;
      for (int row = 0; row < rockToFall.Length && shouldMove; ++row)
      {
        for (int column = 0; column < rockToFall[row].Length && shouldMove; ++column)
        {
          int xToCheck = rockPos.x + column + xChange;
          shouldMove = rows.Count <= rockPos.y - row || rows[rockPos.y - row][xToCheck] == '.' || rockToFall[row][column] == '.';
        }
      }

      if (shouldMove) rockPos.x += xChange;

      // Y Movement
      shouldMove = rockPos.y - rockToFall.Length > 0;
      for (int row = 0; row < rockToFall.Length && shouldMove; ++row)
      {
        for (int column = 0; column < rockToFall[row].Length && shouldMove; ++column)
        {
          shouldMove = rows.Count <= rockPos.y - row - 1 || rows[rockPos.y - row - 1][column + rockPos.x] == '.' || rockToFall[row][column] == '.';
        }
      }

      if (shouldMove)
      {
        rockPos.y -= 1;
        continue;
      }

      // Otherwise we're at rest
      for (int row = rockToFall.Length - 1; row >= 0; --row)
      {
        if (rockPos.y - row >= rows.Count)
        {
          rows.Add(AddToString(".......", rockToFall[row], rockPos.x));
        }
        else
        {
          rows[rockPos.y - row] = AddToString(rows[rockPos.y - row], rockToFall[row], rockPos.x);
        }
      }
      break;
    }
  }

  // Remove the floor and apply any skips we managed to do
  return rows.Count + skippedRowCount - 1;
}

string AddToString(string present, string toAdd, int startPosition, char toSet = '#')
{
  StringBuilder strB = new StringBuilder(present);
  for (int i = 0; i < toAdd.Length; ++i)
  {
    if (toAdd[i] == '#')
      strB[startPosition + i] = toSet;
  }

  return strB.ToString();
}
