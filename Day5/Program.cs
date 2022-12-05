SolvePartOne();
SolvePartTwo();

void SolvePartOne()
{
  (var stacks, var instructionValues) = SetUpStacksAndInstructions();

  foreach (var instruction in instructionValues)
  {
    MoveBlocks(stacks[instruction[(int)Instruction.FROM_STACK] - 1], stacks[instruction[(int)Instruction.TO_STACK] - 1], instruction[(int)Instruction.MOVE_AMOUNT]);
  }

  OutputResults("Part One: {0}", stacks);
}

void SolvePartTwo()
{
  (var stacks, var instructionValues) = SetUpStacksAndInstructions();
  var movingChars = new Stack<char>();

  foreach (var instruction in instructionValues)
  {
    MoveBlocks(stacks[instruction[(int)Instruction.FROM_STACK] - 1], movingChars, instruction[(int)Instruction.MOVE_AMOUNT]);
    MoveBlocks(movingChars, stacks[instruction[(int)Instruction.TO_STACK] - 1], instruction[(int)Instruction.MOVE_AMOUNT]);
  }

  OutputResults("Part Two: {0}", stacks);
}

(Stack<char>[], List<int[]>) SetUpStacksAndInstructions()
{
  var input = File.ReadAllLines("input.txt").ToList();
  var stringLine = input.First(str => !str.Contains('['));
  int stackCount = int.Parse(stringLine.Substring(stringLine.TrimEnd().LastIndexOf(' ')));

  var stacks = new Stack<char>[stackCount];
  for (int i = input.IndexOf(stringLine) - 1; i >= 0; --i)
  {
    string cargoLine = input[i];
    for (int j = 0; j < stackCount; ++j)
    {
      stacks[j] ??= new Stack<char>();
      int indexOfColumn = stringLine.IndexOf((j + 1).ToString());
      if (cargoLine.Length > indexOfColumn && cargoLine[indexOfColumn] != ' ')
      {
        stacks[j].Push(cargoLine[indexOfColumn]);
      }
    }
  }

  // Leave only the instructions, and extract their numeric values
  input.RemoveRange(0, input.IndexOf(stringLine) + 2);
  return (stacks, input.ConvertAll(instruction => Array.ConvertAll(instruction.Replace("move ", "").Replace("from ", "").Replace("to ", "").Split(' '), strInt => int.Parse(strInt))));
}

void MoveBlocks(Stack<char> from, Stack<char> to, int blocksToMove)
{
  for (int i = 0; i < blocksToMove && from.Count > 0; ++i)
  {
    to.Push(from.Pop());
  }
}

void OutputResults(string outputFormat, Stack<char>[] stacks) => Console.WriteLine(outputFormat, string.Join("", stacks.Select(x => x.Pop().ToString())));

enum Instruction
{
  MOVE_AMOUNT = 0,
  FROM_STACK,
  TO_STACK
}
