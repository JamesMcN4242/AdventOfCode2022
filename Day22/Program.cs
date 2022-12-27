var input = File.ReadAllText("input.txt").Replace("\r", "").Split("\n\n");
var mapStrings = input[0].Split('\n');
var instructions = input[1].Trim();

var map = new char[mapStrings.Length][];
int maxX = mapStrings.Max(str => str.Length);
for (int i = 0; i < mapStrings.Length; ++i)
{
  map[i] = new char[maxX];
  for (int j = 0; j < maxX; ++j)
  {
    map[i][j] = mapStrings[i].Length > j ? mapStrings[i][j] : ' ';
  }
}

Console.WriteLine($"Part One: {PartOne()}");

long PartOne()
{
  var dir = Facing.RIGHT;
  var pos = (x: 0, y: 0);
  for (int i = 0; i < map[0].Length; ++i)
  {
    if (map[0][i] != ' ')
    {
      pos.x = i;
      break;
    }
  }

  int instructionIndex = -1;
  while(++instructionIndex < instructions.Length)
  {
    if (instructions[instructionIndex] == 'L')
    {
      dir = dir == Facing.RIGHT ? Facing.UP : dir - 1;
      continue;
    }
    else if (instructions[instructionIndex] == 'R')
    {
      dir = (Facing)((int)(dir + 1) % (int)Facing.COUNT);
      continue;
    }

    string numberToMove = instructions[instructionIndex].ToString();
    while (instructionIndex < instructions.Length - 1)
    {
      if (instructions[instructionIndex + 1] == 'L' || instructions[instructionIndex + 1] == 'R') break;
      numberToMove += instructions[++instructionIndex];
    }
    int moves = int.Parse(numberToMove);

    for (; moves > 0; --moves)
    {
      map[pos.y][pos.x] = dir == Facing.RIGHT ? '>' : dir == Facing.LEFT ? '<' : dir == Facing.DOWN ? 'V' : '^';
      switch (dir)
      {
        case Facing.UP:
          if (pos.y == 0 || map[pos.y - 1][pos.x] == ' ')
          {
            for(int i = 0; i < map.Length; ++i)
            {
              if (pos.y + i == map.Length - 1 || map[pos.y + i + 1][pos.x] == ' ')
              {
                if (map[pos.y + i][pos.x] == '#')
                {
                  moves = 0; break;
                }
                pos.y = pos.y + i;
                break;
              }
            }
            break;
          }

          if (map[pos.y - 1][pos.x] == '#')
          {
            moves = 0;
            break;
          }

          --pos.y;
          break;

        case Facing.RIGHT:
          if (pos.x + 1 == maxX || map[pos.y][pos.x + 1] == ' ')
          {
              for (int i = 0; i < map.Length; ++i)
              {
                if (pos.x - i == 0 || map[pos.y][pos.x - i - 1] == ' ')
                {
                  if (map[pos.y][pos.x - i] == '#')
                  {
                    moves = 0; break;
                  }
                  pos.x = pos.x - i;
                  break;
                }
              }
              break;
          }

          if (map[pos.y][pos.x + 1] == '#')
          {
            moves = 0;
            break;
          }

          ++pos.x;
          break;

        case Facing.DOWN:
          if (pos.y + 1 == map.Length || map[pos.y + 1][pos.x] == ' ')
          {
            for (int i = 0; i < map.Length; ++i)
            {
              if (pos.y - i == 0 || map[pos.y - i - 1][pos.x] == ' ')
              {
                if (map[pos.y - i][pos.x] == '#')
                {
                  moves = 0; break;
                }
                pos.y = pos.y - i;
                break;
              }
            }
            break;
          }

          if (map[pos.y + 1][pos.x] == '#')
          {
            moves = 0;
            break;
          }

          ++pos.y;
          break;

        case Facing.LEFT:
          if (pos.x == 0 || map[pos.y][pos.x - 1] == ' ')
          {
            for (int i = 0; i < map.Length; ++i)
            {
              if (pos.x + i == map[pos.y].Length - 1 || map[pos.y][pos.x + i + 1] == ' ')
              {
                if (map[pos.y][pos.x + i] == '#')
                {
                  moves = 0; break;
                }
                pos.x = pos.x + i;
                break;
              }
            }
            break;
          }

          if (map[pos.y][pos.x - 1] == '#')
          {
            moves = 0;
            break;
          }

          --pos.x;
          break;
      }
    }
  }

  PrintMap();
  return (pos.y + 1) * 1000 + (pos.x + 1) * 4 + (int)dir;
}

void PrintMap()
{
  File.WriteAllLines("directionMap.txt", Array.ConvertAll(map, charArr => string.Join("", charArr)));
}

enum Facing
{
  RIGHT = 0,
  DOWN,
  LEFT,
  UP,
  COUNT
}
