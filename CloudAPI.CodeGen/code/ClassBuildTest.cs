using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CloudAPI.Server.Build 
{
public class ClassBuildTest
{
  static void Test(string[] args)
  {
   // GenerateSampleViewModel();
    AssemblyWriter.GenerateAssembly();
  }
  static void GenerateSampleViewModel()
  {
    const string models = @"namespace Models
        {
        public class Item
        {
            public string ItemName { get; set }
        }
        }
        ";
    var node = CSharpSyntaxTree.ParseText(models).GetRoot();
    var viewModel = ViewModelGeneration.GenerateViewModel(node);
    if(viewModel!=null)
      Console.WriteLine(viewModel.ToFullString());
    Console.ReadLine();
  }
}
}