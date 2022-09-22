// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using AutoRest.CSharp.Input;
using AutoRest.CSharp.Input.Source;
using AutoRest.CSharp.Mgmt.Models;
using AutoRest.CSharp.Mgmt.Output;
using AutoRest.CSharp.MgmtExplorer.AutoRest;
using AutoRest.CSharp.MgmtExplorer.Generation;
using AutoRest.CSharp.MgmtTest.AutoRest;
using Azure;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoRest.CSharp.AutoRest.Plugins
{
    internal class MgmtExplorerTarget
    {
        public static async Task<Dictionary<string, string>> ExecuteAsync(CodeModel codeModel)
        {
            Debug.Assert(codeModel.TestModel is not null);
            Debug.Assert(Configuration.MgmtConfiguration.TestGen is not null);

            try
            {
                string outputPath = string.IsNullOrEmpty(Configuration.OutputFolder) ? Directory.GetCurrentDirectory() : Configuration.OutputFolder;

                var sourceCodePath = Path.Combine(Configuration.OutputFolder, "../../src");
                var sourceCodeProject = new SourceCodeProject(sourceCodePath, Configuration.SharedSourceFolders);
                var sourceInputModel = new SourceInputModel(await sourceCodeProject.GetCompilationAsync());

                // construct the MgmtTestOutputLibrary
                var library = new MgmtExplorerOutputLibrary(codeModel, sourceInputModel);

                //GenerateSchema(library);
                Dictionary<string, string> r = new Dictionary<string, string>();
                GenerateOperations(library, r);

                return r;
            }
            catch
            {
                throw;
            }
        }

        private static void GenerateOperations(MgmtExplorerOutputLibrary library, Dictionary<string, string> output)
        {
            Dictionary<string, string> r = new Dictionary<string, string>();
            foreach (var desc in library.EnumerateAllExplorerApis())
            {
                MgmtExplorerCodeGenBase? writer = desc.Provider switch
                {
                    ResourceCollection rc => new MgmtExplorerCodeGenForResourceCollectionApi(desc),
                    Resource res => new MgmtExplorerCodeGenForResourceApi(desc),
                    MgmtExtensions ex => new MgmtExplorerCodeGenForExtensionsApi(desc),
                    // TODO: throw exception after we add all support
                    _ => null,
                };
                if (writer != null)
                {
                    string v = writer.WriteExplorerApi();
                    // exception will be thrown if the name already exists, is it possible? let's see
                    output.Add($"Explorer/{desc.UniqueName}.cs", v);
                }
            }
        }
    }
}
