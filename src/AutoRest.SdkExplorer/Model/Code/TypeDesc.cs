// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using AutoRest.SdkExplorer.Model.Azure;
using AutoRest.SdkExplorer.Model.Example;
using AutoRest.SdkExplorer.Model.Schema;
using AutoRest.SdkExplorer.Model.Hint;
using Azure.Core;

namespace AutoRest.SdkExplorer.Model.Code
{
    public class TypeDesc
    {
        public string? Name { get; set; }
        public string? Namespace { get; set; }
        public bool IsValueType { get; set; }
        public bool IsEnum { get; set; }
        public bool IsNullable { get; set; }
        public bool IsGenericType { get; set; }
        public bool IsFrameworkType { get; set; }
        public bool IsList { get; set; }
        public bool IsDictionary { get; set; }
        public bool IsBinaryData { get; set; }
        public List<TypeDesc> Arguments { get; set; } = new List<TypeDesc>();
        public string? FullNameWithNamespace { get; set; }
        public string? FullNameWithoutNamespace { get; set; }

        public string? SchemaKey { get; set; }
        public string? SchemaType { get; set; }

        public TypeDesc()
        {
        }

        public void SetSchema(SchemaBase schema)
        {
            SchemaKey = schema.SchemaKey;
            SchemaType = schema.SchemaType;
        }

        public string GetFullName(bool includeNamespace, bool includeNullable = true)
        {
            string name = includeNamespace ? $"{Namespace ?? "__N/A__"}.{Name ?? "__N/A__"}" : Name ?? "__N/A__";
            if (includeNullable && IsNullable)
                name += "?";
            if (IsGenericType)
            {
                name += "<" + string.Join(", ", Arguments.Select(a => a.GetFullName(includeNamespace))) + ">";
            }
            return name;
        }

        public SchemaBase? GetSchema(SchemaStore? store = null)
        {
            store ??= SchemaStore.Instance;
            return store.GetSchemaFromStore(this);
        }

        public TypeDesc GetListElementType()
        {
            if (!this.IsList)
                throw new InvalidOperationException("Unable to get list element type when current type is not a list: " + this.FullNameWithNamespace);
            return this.Arguments[0];
        }

        public TypeDesc GetDictKeyType()
        {
            if (!this.IsDictionary)
                throw new InvalidOperationException("Unable to get key type when current type is not a dictionary: " + this.FullNameWithNamespace);
            return this.Arguments[0];
        }

        public TypeDesc GetDictValueType()
        {
            if (!this.IsDictionary)
                throw new InvalidOperationException("Unable to get value type when current type is not a dictionary: " + this.FullNameWithNamespace);
            return this.Arguments[1];
        }

        public IEnumerable<FieldHint> GetFieldHints(string curFieldName, string exampleName, ExampleValueDesc exampleValueDesc, SchemaStore? schemaStore)
        {
            Dictionary<string, FieldHint> dict = new Dictionary<string, FieldHint>();
            this.GetFieldHintsInternal(curFieldName, exampleName, exampleValueDesc, schemaStore, ref dict);
            return dict.Values.ToList();
        }

        private void GetFieldHintsInternal(string curFieldName, string exampleName, ExampleValueDesc exampleValueDesc, SchemaStore? schemaStore, ref Dictionary<string, FieldHint> result)
        {
            if (result == null)
                throw new ArgumentNullException("result");
            if (this.IsList)
            {
                if (!exampleValueDesc.HasArrayValues)
                    throw new InvalidOperationException("No array example value for list type: " + this.FullNameWithNamespace);
                var eleType = this.GetListElementType();
                foreach (var arrExample in exampleValueDesc.ArrayValues!)
                {
                    eleType.GetFieldHintsInternal(curFieldName, exampleName, arrExample, schemaStore, ref result);
                }
            }
            else if (this.IsDictionary)
            {
                if (!exampleValueDesc.HasPropertyValues)
                    throw new InvalidOperationException("No property value for Dictionary type: " + this.FullNameWithNamespace);
                var keyType = this.GetDictKeyType();
                var valueType = this.GetDictValueType();
                foreach (var kv in exampleValueDesc.PropertyValues!)
                {
                    keyType.GetFieldHintsInternal($"{curFieldName}.Key", exampleName, new ExampleValueDesc()
                    {
                        CSharpName = "Key",
                        ModelName = "Key",
                        SerializerName = "key",
                        SchemaType = $"{exampleValueDesc.SchemaType}.Key",
                        RawValue = kv.Key,
                    }, schemaStore, ref result);
                    valueType.GetFieldHintsInternal($"{curFieldName}.Value", exampleName, kv.Value, schemaStore, ref result);
                }
            }
            else if (exampleValueDesc.HasPropertyValues)
            {
                var schema = this.GetSchema(schemaStore);
                if (schema is not SchemaObject sob)
                    throw new InvalidOperationException("Object Schema is expected for non-value type: " + this.FullNameWithNamespace);

                foreach (var propExampleKV in exampleValueDesc.PropertyValues!)
                {
                    bool found = false;
                    foreach (var curProp in sob.GetAllProperties(schemaStore))
                    {
                        var matchExample = curProp.FindMatchingExample(propExampleKV.Value);
                        if (matchExample != null)
                        {
                            curProp.Type!.GetFieldHintsInternal($"{this.GetFullName(includeNamespace: true, includeNullable: false)}.{curProp.Name}", exampleName, matchExample, schemaStore, ref result);
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        // ignore examples no corresponding prop found. Most of them are because the example value is for readonly properties which is ignored in our schema
                        // TODO: include readonly property in our schema to do validation when needed
                        // TODO: add warning log?
                        var missing = $"{this.FullNameWithNamespace}.{propExampleKV.Key}";
                    }
                }
            }
            else if (exampleValueDesc.HasRawValue)
            {
                var schema = this.GetSchema(schemaStore);

                string raw = exampleValueDesc.RawValue!;
                if (!result.TryGetValue(curFieldName, out var hint))
                {
                    hint = new FieldHint(curFieldName);
                    result.Add(curFieldName, hint);
                }
                if (exampleValueDesc.RawValue!.Length > 0 && schema != null && schema.SchemaKey == typeof(ResourceIdentifier).FullName)
                {
                    AzureResourceIdentifier ari = new AzureResourceIdentifier(raw);
                    AzureResourceType? art = ari.GetResourceType();
                    if (art != null)
                    {
                         hint.AzureResourceTypes.Add(art);
                    }
                }

                hint.RawExampleValues.Add(new ExampleRawValueHint(exampleName, raw!));
            }
            else
            {
                if (!this.IsBinaryData)
                    throw new InvalidOperationException("unexpected example value for type: " + this.FullNameWithNamespace);
            }
        }

        public static TypeDesc CreateEnumType(string name, string @namespace, bool isNullable, SchemaEnum schema)
        {
            var o = new TypeDesc()
            {
                Name = name,
                Namespace = @namespace,
                IsValueType = true,
                IsEnum = true,
                IsNullable = isNullable,
                IsGenericType = false,
                IsFrameworkType = false,
                IsList = false,
                IsDictionary = false,
                IsBinaryData = false,
                Arguments = new List<TypeDesc>(),
            };
            o.FullNameWithNamespace = o.GetFullName(true);
            o.FullNameWithoutNamespace = o.GetFullName(false);
            o.SetSchema(schema);
            return o;
        }
    }
}
