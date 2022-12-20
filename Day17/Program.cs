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

List<string> outputLines = new List<string>(500);
Console.WriteLine($"Part One: {Run(2022)}");

//Part two, will try to find repeated hashes for the top X tower sections dependant on the rock and input indexes. 
//Presumably it has to eventually loop around when we have so many rocks.
//Console.WriteLine($"Part Two: {Run(1000000000000)}");

long Run(long rocksToDrop, long? outputRock = null)
{
  int inputIndex = 0;
  int rockIndex = 0;
  List<string> rows = new List<string>(5000) { "_______" };
  for (long rocksFallen = 0; rocksFallen < rocksToDrop; ++rocksFallen, ++rockIndex)
  {
    rockIndex %= rockArrangements.Length;
    var rockToFall = rockArrangements[rockIndex];

    //Position is relative to the top left point
    var rockPos = (x: 2, y: rows.Count + rockToFall.Length + 2);

    while(true)
    {
      int index = inputIndex++;
      inputIndex %= inputString.Length;
      
      int xChange = inputString[index] == '<' ? -1 : 1;
      if (outputRock.HasValue && outputRock.Value == rocksFallen + 1)
        OutputView(rows, rockToFall, rockPos, inputString[index]);

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

      shouldMove = rockPos.y - rockToFall.Length > 0;
      for (int row = 0; row < rockToFall.Length && shouldMove; ++row)
      {
        for(int column = 0; column < rockToFall[row].Length && shouldMove; ++column)
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

  // Remove the floor
  return rows.Count - 1;
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

void OutputView(List<string> rows, string[] rockFormation, (int, int) rockPosition, char nextOperation)
{
  outputLines.Clear();
  outputLines.Add($"X Move applied after this: {nextOperation}\n\n");
  
  for (int i = rockFormation.Length; i > 0 && rows.Count < rockPosition.Item2 - rockFormation.Length + i + 1; --i)
  {
    outputLines.Add("|" + AddToString(".......", rockFormation[^i], rockPosition.Item1, '@') + "|");
  }

  for (int i = rockPosition.Item2 - rockFormation.Length; i >= rows.Count; --i )
  {
    outputLines.Add("|.......|");
  }
  
  for (int i = rows.Count - 1; i >= 0 && i > rows.Count - 15; --i)
  {
    if (rockPosition.Item2 >= i && rockPosition.Item2 - rockFormation.Length < i)
    {
      int jIndex = rockPosition.Item2 - i;
      outputLines.Add("|" + AddToString(rows[i], rockFormation[jIndex], rockPosition.Item1, '@') + "|");
    }
    else
    {
      outputLines.Add($"|{rows[i]}|");
    }
  }
  
  outputLines.Add("\n\n");
  File.AppendAllLines("outputFile.txt", outputLines);
}
