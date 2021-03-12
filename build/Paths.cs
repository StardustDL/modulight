using Cake.Core.IO;
using System.Collections.Generic;

namespace Build
{
    internal static class Paths
    {
        internal static class Dist
        {
            public static DirectoryPath Base => Paths.Base.Combine("dist");

            public static DirectoryPath Packages => Base.Combine("packages");

            public static DirectoryPath Test => Base.Combine("tests");

            public static DirectoryPath Documents => Base.Combine("docs");

            public static DirectoryPath CoverageDocuments => Documents.Combine("_site/reports/coverage");

            public static FilePath TestCoverageJsonResult => Test.CombineWithFilePath("./coverage.json");

            public static FilePath TestCoverageXmlResult => Test.CombineWithFilePath("./coverage.xml");

            public static DirectoryPath TestReport => Test.Combine("reports");
        }

        public static DirectoryPath Base => ".";

        public static DirectoryPath Test => Base.Combine("test");

        public static DirectoryPath Docs => Base.Combine("docs");

        public static FilePath DocFxConfigFile => Docs.CombineWithFilePath("docfx.json");

        public static FilePath TestBaseProject => Test.CombineWithFilePath("./Test.Base/Test.Base.csproj");

        public static FilePath UISolution => Base.CombineWithFilePath("./UI.sln");

        public static FilePath ModulightSolution => Base.CombineWithFilePath("./Modulight.sln");

        public static IEnumerable<FilePath> Solutions => new FilePath[] { ModulightSolution };
    }
}