﻿// -------------------------------------------------------------------------------------------------
// <copyright file="NuGetReader.cs" company="RHEA System S.A.">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;

    using NuGet.Packaging;

    public class NuGetReader
    {
        /// <summary>
        /// Update the <see cref="Package"/> objects with information from the local nuget cache
        /// </summary>
        /// <param name="packages">
        /// The <see cref="Packages"/> for which the information needs to be retrieved from the local nuget cache
        /// </param>
        /// <returns>
        /// The <see cref="List{Package}"/>
        /// </returns>
        public void Update(List<Package> packages)
        {
            foreach (var package in packages)
            {
                var separator = Path.DirectorySeparatorChar;
                var path = $@"%USERPROFILE%{separator}.nuget{separator}packages{separator}{package.Name}{separator}{package.Version}{separator}{package.Name}.nuspec";
                var filePath = Environment.ExpandEnvironmentVariables(path);
                if (File.Exists(filePath))
                {
                    var nuspecDoc = XDocument.Load(filePath);
                    var reader = new NuspecReader(nuspecDoc);
                    package.Description = reader.GetDescription();
                    package.ProjectUrl = reader.GetProjectUrl();
                    package.LicenseUrl = reader.GetLicenseUrl();
                    package.License = reader.GetLicenseMetadata()?.License;
                }
                else
                {
                    Console.WriteLine($"One of the referenced package files ({package.Name}) doesn't exist, please consider running a dotnet restore command");
                }
            }
        }
    }
}
