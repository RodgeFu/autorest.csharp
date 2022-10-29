// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using AutoRest.CSharp.Input;
using AutoRest.CSharp.Input.Source;
using AutoRest.CSharp.Mgmt.AutoRest;
using AutoRest.CSharp.Mgmt.Models;
using AutoRest.CSharp.Mgmt.Output;
using AutoRest.CSharp.MgmtExplorer.Contract;
using AutoRest.CSharp.MgmtExplorer.Models;
using AutoRest.CSharp.MgmtTest.Models;
using AutoRest.CSharp.Output.Models.Types;
using AutoRest.CSharp.Utilities;

namespace AutoRest.CSharp.MgmtExplorer.AutoRest
{
    internal class MgmtExplorerOutputLibrary
    {
        private static string GetSdkPackageVersion()
        {
            string? file = Configuration.MgmtConfiguration.ExplorerGen?.SdkPackagesDataFile;
            if (!string.IsNullOrEmpty(file))
            {
                XmlReader reader = new XmlTextReader(file) { Namespaces = false };

                XmlDocument xd = new XmlDocument();
                xd.Load(reader);
                XmlNode node = xd.SelectSingleNode($"/Project/ItemGroup/PackageReference[@Update='{MgmtContext.Context.DefaultNamespace}']");
                if (node != null)
                {
                    return node.Attributes["Version"].Value;
                }
            }
            return "0.0.0";
        }

        public MgmtExplorerCodeGenInfo Info { get; init; }

        public MgmtExplorerOutputLibrary(CodeModel codeModel, SourceInputModel sourceInputModel)
        {
            MgmtContext.Initialize(new BuildContext<MgmtOutputLibrary>(codeModel, sourceInputModel));
            this.Info = new MgmtExplorerCodeGenInfo(
                MgmtContext.Context.DefaultNamespace,
                GetSdkPackageVersion(),
                DateTimeOffset.UtcNow,
                new List<string>()); // TODO: no extra nuget needed?
        }

        public CachedDictionary<Schema, TypeProvider> GetAllResourceSchemaMap()
        {
            return MgmtContext.Library.ResourceSchemaMap;
        }

        public CachedDictionary<Schema, TypeProvider> GetAllNonResourceSchemaMap()
        {
            return MgmtContext.Library.SchemaMap;
        }

        private IEnumerable<KeyValuePair<T, MgmtClientOperation>> EnumerateOperations<T>(IEnumerable<T> providerList) where T : MgmtTypeProvider
        {
            foreach (var provider in providerList)
            {
                foreach (var operations in provider.AllOperations)
                {
                    // TODO: DO WE NEED TO SKIP THIS for explorer?
                    if (operations.IsConvenientOperation)
                        continue;
                    // When will this not be 1?
                    Debug.Assert(operations.Count == 1);
                    yield return new KeyValuePair<T, MgmtClientOperation>(provider, operations);
                }
            }
        }

        private IEnumerable<KeyValuePair<Resource, MgmtClientOperation>> EnumerateOperationsOnResource() => EnumerateOperations<Resource>(MgmtContext.Library.ArmResources);
        private IEnumerable<KeyValuePair<ResourceCollection, MgmtClientOperation>> EnumerateOperationsOnResourceCollection() => EnumerateOperations<ResourceCollection>(MgmtContext.Library.ResourceCollections);
        private IEnumerable<KeyValuePair<MgmtExtensions, MgmtClientOperation>> EnumerateOperationsOnExtension() => EnumerateOperations<MgmtExtensions>(MgmtContext.Library.ExtensionWrapper.Extensions);

        public IEnumerable<MgmtExplorerApiDesc> EnumerateAllExplorerApis()
        {
            var exampleDict = MgmtContext.CodeModel.TestModel!.MockTest.ExampleGroups.ToDictionary(
                g => g.OperationId,
                g => g);

            foreach (var (p, o) in this.EnumerateOperationsOnResource())
            {
                exampleDict.TryGetValue(o.First().OperationId, out var exampleGroup);
                yield return new MgmtExplorerApiDesc(this.Info, p, o, exampleGroup);
            }
            foreach (var (p, o) in this.EnumerateOperationsOnResourceCollection())
            {
                exampleDict.TryGetValue(o.First().OperationId, out var exampleGroup);
                yield return new MgmtExplorerApiDesc(this.Info, p, o, exampleGroup);
            }
            foreach (var (p, o) in this.EnumerateOperationsOnExtension())
            {
                exampleDict.TryGetValue(o.First().OperationId, out var exampleGroup);
                yield return new MgmtExplorerApiDesc(this.Info, p, o, exampleGroup);
            }
        }

    }
}
