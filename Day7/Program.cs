var input = SetupDirectories(File.ReadAllLines("input.txt"));
Console.WriteLine($"Part One: {input.allNodes.Sum(dir => dir.Size <= 100000 ? dir.Size : 0)}");

long valueToRemove = 30000000 - (70000000 - input.rootNode.Size);
Console.WriteLine($"Part Two: {input.allNodes.Min(dir => dir.Size >= valueToRemove ? dir.Size : long.MaxValue)}");

(DirectoryNode rootNode, List<DirectoryNode> allNodes) SetupDirectories(string[] input)
{
  DirectoryNode rootDir = new DirectoryNode { Name = "ThrowAwayNode" };
  rootDir.Directories.Add(new DirectoryNode { Name = "/", Container = rootDir });
  List<DirectoryNode> allNodes = new List<DirectoryNode>(200) { rootDir.Directories[0] };

  DirectoryNode currentDir = rootDir;

  foreach (var line in input)
  {
    if (line.StartsWith('$'))
    {
      if (line.StartsWith("$ cd"))
      {
        string posCommand = line.Replace("$ cd ", "");
        if (posCommand == "..") currentDir = currentDir.Container;
        else currentDir = currentDir.Directories.First(node => node.Name == posCommand);
      }
    }
    else
    {
      if (line.StartsWith("dir"))
      {
        var newDir = new DirectoryNode { Name = line.Replace("dir ", ""), Container = currentDir };
        currentDir.Directories.Add(newDir);
        allNodes.Add(newDir);
      }
      else
      {
        string[] sections = line.Split(' ');
        currentDir.Files.Add(new FileReference { Name = sections[1], Size = long.Parse(sections[0]) });
      }
    }
  }

  return (rootDir.Directories[0], allNodes);
}

class DirectoryNode
{
  public string Name { get; set; }
  public DirectoryNode Container { get; set; }
  public List<FileReference> Files { get; set; } = new List<FileReference>();
  public List<DirectoryNode> Directories { get; set; } = new List<DirectoryNode>();

  public long Size => Files.Sum(file => file.Size) + Directories.Sum(dir => dir.Size);
}

class FileReference
{
  public string Name { get; set; }
  public long Size { get; set; }
}
