// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AutoRest.CSharp.Input;
using AutoRest.CSharp.Input.Source;
using AutoRest.CSharp.Mgmt.AutoRest;
using AutoRest.CSharp.Mgmt.Models;
using AutoRest.CSharp.Mgmt.Output;
using AutoRest.CSharp.MgmtExplorer.Models;
using AutoRest.CSharp.Output.Models.Types;
using AutoRest.CSharp.Utilities;

namespace AutoRest.CSharp.MgmtExplorer.AutoRest
{
    internal class MgmtExplorerOutputLibrary
    {
        private static string GetSdkPackageVersion()
        {
            string? file = Configuration.MgmtConfiguration.ExplorerGen?.ChangeLogFile;
            // try to find changelog.md from outputFolder if it's not set explicitly
            if (string.IsNullOrEmpty(file))
            {
                DirectoryInfo di = Directory.GetParent(Configuration.OutputFolder);
                while (di != null)
                {
                    FileInfo[] fis = di.GetFiles("CHANGELOG.md", SearchOption.TopDirectoryOnly);
                    if (fis.Length > 0)
                    {
                        file = fis[0].FullName;
                        break;
                    }
                    di = di.Parent;
                }
            }

            if (string.IsNullOrEmpty(file))
                throw new InvalidOperationException("ChangeLogFile argument is missing for retriving the sdk package version");
            string changelog = File.ReadAllText(file);
            if (!string.IsNullOrEmpty(changelog))
            {
                // try to match released version like: ## 1.0.1 (2022-11-29)
                Regex reg = new Regex(@"^##\s+(?<version>\S+)\s+\(\d+-\d+-\d+\)\s*$", RegexOptions.Multiline);

                var m = reg.Match(changelog);
                if (m.Success)
                {
                    return m.Groups["version"].Value;
                }
            }
            throw new InvalidOperationException("Failed to get latest released sdk version from: " + file);
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
                    yield return new KeyValuePair<T, MgmtClientOperation>(provider, operations);
                }
            }
        }

        private IEnumerable<KeyValuePair<Resource, MgmtClientOperation>> EnumerateOperationsOnResource() => EnumerateOperations<Resource>(MgmtContext.Library.ArmResources);
        private IEnumerable<KeyValuePair<ResourceCollection, MgmtClientOperation>> EnumerateOperationsOnResourceCollection() => EnumerateOperations<ResourceCollection>(MgmtContext.Library.ResourceCollections);
        private IEnumerable<KeyValuePair<MgmtExtensions, MgmtClientOperation>> EnumerateOperationsOnExtension() => EnumerateOperations<MgmtExtensions>(MgmtContext.Library.ExtensionWrapper.Extensions);

        public IEnumerable<MgmtExplorerApiDesc> EnumerateAllExplorerApis()
        {
            // call SchemaMap to tirgger model initialize of MgmtContextLibrary first
            if (MgmtContext.Library.SchemaMap.Count + MgmtContext.Library.ResourceSchemaMap.Count == 0)
                throw new InvalidOperationException("SchemaMap and ResourceSchemaMap count == 0");

            var exampleDict = MgmtContext.CodeModel.TestModel!.MockTest.ExampleGroups.ToDictionary(
                g => g.OperationId,
                g => g);

            foreach (var (p, o) in this.EnumerateOperationsOnResource())
            {
                exampleDict.TryGetValue(o.First().OperationId, out var exampleGroup);
                int i = 0;
                foreach (var rop in o)
                    yield return new MgmtExplorerApiDesc(this.Info, p, o, exampleGroup, i++);
            }
            foreach (var (p, o) in this.EnumerateOperationsOnResourceCollection())
            {
                exampleDict.TryGetValue(o.First().OperationId, out var exampleGroup);
                int i = 0;
                foreach (var rop in o)
                    yield return new MgmtExplorerApiDesc(this.Info, p, o, exampleGroup, i++);
            }
            foreach (var (p, o) in this.EnumerateOperationsOnExtension())
            {
                exampleDict.TryGetValue(o.First().OperationId, out var exampleGroup);
                int i = 0;
                foreach (var rop in o)
                    yield return new MgmtExplorerApiDesc(this.Info, p, o, exampleGroup, i++);
            }
        }

    }
}
