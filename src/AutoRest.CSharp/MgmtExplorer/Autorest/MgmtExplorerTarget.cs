// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoRest.CSharp.Input;
using AutoRest.CSharp.Input.Source;
using AutoRest.CSharp.MgmtExplorer.Autorest;
using AutoRest.CSharp.MgmtExplorer.AutoRest;
using AutoRest.CSharp.MgmtExplorer.Contract;
using AutoRest.CSharp.MgmtExplorer.Generation;
using AutoRest.CSharp.MgmtTest.AutoRest;

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
            HashSet<string> exampleFileName = new HashSet<string>();
            foreach (var desc in library.EnumerateAllExplorerApis())
            {
                MgmtExplorerCodeGenBase writer = MgmtExplorerCodeGenBase.Create(desc);
                MgmtExplorerCodeDesc v = writer.WriteExplorerApi();
                List<MgmtExplorerExampleDesc> examples = MgmtExplorerCodeGenExampleHelper.GenerateExampleDescs(desc, v);

                List<string> outputFormat = Configuration.MgmtConfiguration.ExplorerGen?.OutputFormat?.ToLower().Split(",").ToList() ?? new List<string>();
                if (outputFormat.Contains("yaml"))
                {
                    output.Add($"Explorer/{desc.FullUniqueName}.yaml", v.ToYaml());
                    foreach (var ex in examples)
                    {
                        string filename = $"{ex.OriginalFileNameWithoutExtension}";
                        for (int i = 1; ; i++)
                        {
                            if (!exampleFileName.Contains(filename))
                            {
                                exampleFileName.Add(filename);
                                break;
                            }
                            filename = $"{ex.OriginalFileNameWithoutExtension}_{i}";
                        }

                        output.Add($"Explorer/Example/{filename}.yaml", ex.ToYaml());
                    }
                }
                if (outputFormat.Contains("cs"))
                {
                    output.Add($"Explorer/{desc.FullUniqueName}.cs", v.ToCode(true));
                }
            }
        }
    }
}
