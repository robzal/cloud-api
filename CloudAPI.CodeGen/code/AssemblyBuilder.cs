using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;

namespace CloudAPI.Server.Build
{
  public static class ViewModelGeneration
  {
    public static SyntaxNode GenerateViewModel(SyntaxNode node)
    {
      // Find the first class in the syntax node
      var classNode = node.DescendantNodes()
       .OfType<ClassDeclarationSyntax>().FirstOrDefault();
      if(classNode!=null)
      {
        // Get the name of the model class
        string modelClassName = classNode.Identifier.Text;
        // The name of the ViewModel class
        string viewModelClassName = $"{modelClassName}ViewModel";
        // Only for demo purposes, pluralizing an object is done by
        // simply adding the "s" letter. Consider proper algorithms
        string newImplementation =
          $@"public class {viewModelClassName} : INotifyPropertyChanged
                {{
                public event PropertyChangedEventHandler PropertyChanged;
                // Raise a property change notification
                protected virtual void OnPropertyChanged(string propname)
                {{
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
                }}
                private ObservableCollection<{modelClassName}> _{modelClassName}s;
                public ObservableCollection<{modelClassName}> {modelClassName}s
                {{
                  get {{ return _{modelClassName}s; }}
                  set
                  {{
                    _{modelClassName}s = value;
                    OnPropertyChanged(nameof({modelClassName}s));
                  }}
                }}
                public {viewModelClassName}() {{
                // Implement your logic to load a collection of items
                }}
                }}
                ";
          var newClassNode =
            CSharpSyntaxTree.ParseText(newImplementation).GetRoot()
            .DescendantNodes().OfType<ClassDeclarationSyntax>()
            .FirstOrDefault();
          // Retrieve the parent namespace declaration
          if(!(classNode.Parent is NamespaceDeclarationSyntax)) return null;
          var parentNamespace = (NamespaceDeclarationSyntax)classNode.Parent;
          // Add the new class to the namespace and adjust the white spaces
          var newParentNamespace =
            parentNamespace.AddMembers(newClassNode).NormalizeWhitespace();
          return newParentNamespace;
        //}
      }
      else
      {
        return null;
      }
    }
  }
}