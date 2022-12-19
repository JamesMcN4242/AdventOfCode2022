// WIP
using System.Text;

var inputString = File.ReadAllText("exampleInput.txt").Trim();
var rockArrangements = new string[][]
{
  new []{"####"},
  new []{".#.", "###", ".#." },
  new []{"..#", "..#", "###" },
  new []{"#", "#", "#", "#" },
  new []{"##", "##" }
};

Console.WriteLine($"Part One: {PartOne()}");

int PartOne()
{
  int inputIndex = 0;
  int rockIndex = 0;
  List<string> rows = new List<string>(5000) { "_______" };
  for (int rocksFallen = 0; rocksFallen < 2022; ++rocksFallen, ++rockIndex)
  {
  //  rows.Reverse();
  //  File.AppendAllText("output.txt", "\n\n\n");
  //  File.AppendAllLines("output.txt", rows);
  //  rows.Reverse();

    var rockToFall = rockArrangements[rockIndex % rockArrangements.Length];

    //Position is relative to the top left point
    var rockPos = (x: 2, y: rows.Count + rockToFall.Length + 2);
    while(true)
    {
      int index = (inputIndex++ % inputString.Length);
      int xChange = inputString[index] == '<' ? -1 : 1;
      bool shouldMove = rockPos.x + xChange >= 0 && rockPos.x + xChange + rockToFall[0].Length <= 7;
      for (int row = 0; row < rockToFall.Length && shouldMove; ++row)
      {
        for (int column = 0; column < rockToFall[row].Length && shouldMove; ++column)
        {
          int xToCheck = xChange == -1 ? rockPos.x - 1 : rockPos.x + rockToFall[0].Length;
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

  // 3039 is too low??
  // Remove the floor
  return rows.Count - 1;
}

string AddToString(string present, string toAdd, int startPosition)
{
  StringBuilder strB = new StringBuilder(present);
  for (int i = 0; i < toAdd.Length; ++i)
  {
    if (toAdd[i] == '#')
      strB[startPosition + i] = '#';
  }
  return strB.ToString();
}
