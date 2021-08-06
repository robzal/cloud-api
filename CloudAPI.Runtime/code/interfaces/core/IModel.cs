using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public interface IModel 
    {
        Scope Scope { get; set; }
        string ScopeField {get; set;}
        string ScopeFilter {get; set;}
        string PublisherNamespace {get; set;}
        string Namespace { get; set; }
        string Name {get; set;}
        string VerboseName {get; set;}
        string PluralName {get; set;}
        string Description {get; set;}
        bool IsManaged {get; set;}
        string DBTableName {get; set;}
        string DBSchemaName {get; set;}
        string SortOrder {get; set;}
        string CustomGetSQL {get; set;}
        string CustomInsertSQL {get; set;}
        string CustomUpdateSQL {get; set;}
        string CustomDeleteSQL {get; set;}
        List<IModelField> Fields { get; set; }
        List<IModelRelationship> Relationships { get; set; }
        List<IModelFieldMapping> Mappings {get;}
        List<string> KeyFields {get;}
        List<IModelRule> Rules { get; set; }
        List<IService> Services { get; set; }
        List<ISetting> Settings { get; set; }
        List<ILibrary> Libraries { get; set; }
        List<IModel> RelatedModels {get; set;}
        IModelOptions Options {get; set;}
        string GetSQL ();
        string GetSQL (List<string> ids);
        string GetSQL (IModelOptions options);
        string InsertSQL (IObject obj);
        string UpdateSQL(IObject obj);
        string DeleteSQL();
        string DeleteSQL (List<string> ids);
    }

    public interface IModelField 
    {

        string Name {get; set;}
        string VerboseName {get; set;}
        string Description {get; set;}
        int DisplayIndex {get; set;}
        string DBFieldName {get; set;}
        FieldType DataType {get; set;}
        Boolean IsPrimaryKey {get; set;}
        Boolean IsUnique {get; set;}
        Boolean IsAutoCounter {get; set;}
        Boolean IsRequired {get; set;}
        string DefaultValue {get; set;}
        float MinLength {get; set;}
        float MaxLength {get; set;}
        Boolean IsCalculated {get;}
        string ValueFormula {get; set;}
        string RuleFormula {get; set;}
    }

    public interface IModelFieldMapping
    {
        string PropertyName {get; set;}
        string FieldName {get; set;}
        FieldType FieldType {get; set;}
        Boolean IsRequired {get; set;}
    }

    public interface IModelRelationship
    {

    }

    public interface IModelRule
    {
        string Name {get; set;}
        string FieldName {get; set;}
        string Description {get; set;}
        RuleType RuleType {get; set;}
        string RuleValue {get; set;}
    }
}
