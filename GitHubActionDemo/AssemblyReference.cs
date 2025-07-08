using System.Reflection;

namespace GitHubActionDemo;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}

