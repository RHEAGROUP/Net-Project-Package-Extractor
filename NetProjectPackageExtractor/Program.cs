// -------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="RHEA System S.A.">
//
//   Copyright 2022 RHEA System S.A.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace NetProjectPackageExtractor
{
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Main entry point for the command line application
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main entry point for the command line application
        /// </summary>
        /// <param name="args">
        /// command line arguments
        /// </param>
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("please provide the root folder where the csproj files are located and where the result will be written");
                Console.ReadLine();
                Environment.Exit(0);
            }

            var rootFolder = args[0];

            var resultFile = "result.xlsx";

            if (args.Length == 2)
            {
                resultFile = args[1];
            }

            var projectFileExtractor = new ProjectFileExtractor();
            var csprojFiles = projectFileExtractor.QueryProjectFiles(rootFolder);

            var projectFileParser = new ProjectFileParser();
            var packages = projectFileParser.RunParser(csprojFiles).ToList();

            var nuGetReader = new NuGetReader();
            nuGetReader.Update(packages);

            PackageToExcelWriter.WriteSRF(packages, Path.Combine(rootFolder, resultFile));
        }
    }
}