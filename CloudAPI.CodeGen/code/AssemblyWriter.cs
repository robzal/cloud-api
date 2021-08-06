using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace CloudAPI.Server.Build
{
  public static class AssemblyWriter
  {
    public static void GenerateAssembly()
    {
      const string code = @"using System;
        using System.IO;
        namespace RoslynCore
        {
          public static class Helper
          {
            public static double CalculateCircleArea(double radius)
            {
              return radius * radius * Math.PI;
            }
          }
        }";
      var tree = SyntaxFactory.ParseSyntaxTree(code);
      string fileName="mylib.dll";
      // Detect the file location for the library that defines the object type
      var systemRefLocation=typeof(object).GetTypeInfo().Assembly.Location;
      // Create a reference to the library
      var systemReference = MetadataReference.CreateFromFile(systemRefLocation);
      // A single, immutable invocation to the compiler
      // to produce a library
      var compilation = CSharpCompilation.Create(fileName)
        .WithOptions(
          new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
        .AddReferences(systemReference)
        .AddSyntaxTrees(tree);
      string path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
      EmitResult compilationResult = compilation.Emit(path);
      if(compilationResult.Success)
      {
        // Load the assembly
        Assembly asm =
          AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
        // Invoke the RoslynCore.Helper.CalculateCircleArea method passing an argument
        double radius = 10;
        object result = 
          asm.GetType("RoslynCore.Helper").GetMethod("CalculateCircleArea").
          Invoke(null, new object[] { radius });
        Console.WriteLine($"Circle area with radius = {radius} is {result}");
      }
      else
      {
        foreach (Diagnostic codeIssue in compilationResult.Diagnostics)
        {
          string issue = $"ID: {codeIssue.Id}, Message: {codeIssue.GetMessage()}, Location: {codeIssue.Location.GetLineSpan()}, Severity: {codeIssue.Severity}";
          Console.WriteLine(issue);
        }
      }
    }
  }
}