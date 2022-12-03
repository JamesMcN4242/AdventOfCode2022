static int GetCharValue(char toGet) => char.IsUpper(toGet) ? toGet - 64 + 26 : toGet - 96;
string[] lines = File.ReadAllLines("input.txt");

// Part one - a-z is worth 1-26, and A-Z is 27-52.
// We're searching for repeated chars in the first and second half of the strings.
int repeatedValues = 0;
foreach (string line in lines)
{
    var compartmentItems = new HashSet<char>(line.ToCharArray(0, line.Length / 2));
    
    for (int i = line.Length / 2; i < line.Length; ++i)
        if (compartmentItems.Remove(line[i]))
            repeatedValues += GetCharValue(line[i]);
}

Console.WriteLine($"Part One: {repeatedValues}");

// Part Two - Get the elf badges for each group of 3.
int badgeValues = 0;
for (int i = 0; i < lines.Length; i += 3)
{
    var groupedChars =  lines[i].Distinct().Concat(
        lines[i + 1].Distinct().Concat(lines[i + 2].Distinct())).GroupBy(ch => ch);
    
    badgeValues += GetCharValue(groupedChars.First(group => group.Count() == 3).Key);
}

Console.WriteLine($"Part Two: {badgeValues}");