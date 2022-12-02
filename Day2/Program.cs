// Part One completed as a 1-line challenge... I'm so sorry.
Console.WriteLine($"Part One: {File.ReadAllLines("input.txt").Sum(line => line.Split(' ')[1][0] - 'W' + (((line.Split(' ')[0][0] - 'A' - (line.Split(' ')[1][0] - 'X')) + 5) % 3 * 12 % 10 % 3) * 3)}");

Console.WriteLine($"Part Two: {File.ReadAllLines("input.txt").Sum(GetPartTwoSum)}");

static int GetPartTwoSum(string line)
{
  var parts = line.Split(' ');
  return (parts[1][0] - 'X') * 3 + ChoiceScore(parts);

  static int ChoiceScore(string[] parts) => parts[1][0] switch
  {
    'X' => parts[0][0] - '@' == 1 ? 3 : parts[0][0] - '@' - 1,
    'Y' => parts[0][0] - '@',
    'Z' => (parts[0][0] - '@') % 3 + 1,
    _ => throw new NotImplementedException()
  };
}
