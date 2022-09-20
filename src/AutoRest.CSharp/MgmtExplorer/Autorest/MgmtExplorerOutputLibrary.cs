// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public MgmtExplorerOutputLibrary(CodeModel codeModel, SourceInputModel sourceInputModel)
        {
            MgmtContext.Initialize(new BuildContext<MgmtOutputLibrary>(codeModel, sourceInputModel));
        }

        public CachedDictionary<Schema, TypeProvider> GetAllResourceSchemaMap()
        {
            return MgmtContext.Library.ResourceSchemaMap;
        }

        public CachedDictionary<Schema, TypeProvider> GetAllNonResourceSchemaMap()
        {
            return MgmtContext.Library.SchemaMap;
        }

        private IEnumerable<KeyValuePair<T, MgmtRestOperation>> EnumerateOperations<T>(IEnumerable<T> providerList) where T : MgmtTypeProvider
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
                    yield return new KeyValuePair<T, MgmtRestOperation>(provider, operations.First());
                }
            }
        }

        private IEnumerable<KeyValuePair<Resource, MgmtRestOperation>> EnumerateOperationOnResource() => EnumerateOperations<Resource>(MgmtContext.Library.ArmResources);
        private IEnumerable<KeyValuePair<ResourceCollection, MgmtRestOperation>> EnumerateOperationsOnResourceCollection() => EnumerateOperations<ResourceCollection>(MgmtContext.Library.ResourceCollections);
        private IEnumerable<KeyValuePair<MgmtExtensions, MgmtRestOperation>> EnumerateOperationsOnExtension() => EnumerateOperations<MgmtExtensions>(MgmtContext.Library.ExtensionWrapper.Extensions);

        public IEnumerable<MgmtExplorerApiDesc> EnumerateAllExplorerApis()
        {
            foreach (var (p, o) in this.EnumerateOperationOnResource())
            {
                yield return new MgmtExplorerApiDesc(p, o);
            }
            foreach (var (p, o) in this.EnumerateOperationsOnResourceCollection())
            {
                yield return new MgmtExplorerApiDesc (p, o);
            }
            foreach (var (p, o) in this.EnumerateOperationsOnExtension())
            {
                yield return new MgmtExplorerApiDesc(p, o);
            }
        }

    }
}
