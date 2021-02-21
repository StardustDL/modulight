using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Frosting;
using Cake.Coverlet;
using Cake.Common.Tools.DotNetCore.Test;
using Cake.Core.IO.Arguments;

namespace Build
{
    [TaskName("Test")]
    [IsDependentOn(typeof(BuildTask))]
    public sealed class TestTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            context.CleanDirectory(Paths.Dist.Test);

            foreach (var solution in context.SolutionFiles)
            {
                context.DotNetCoreTest(solution, new DotNetCoreTestSettings
                {
                    Configuration = context.BuildConfiguration,
                }, new CoverletSettings
                {
                    CollectCoverage = true,
                    CoverletOutputDirectory = Paths.Dist.TestCoverageJsonResult.GetDirectory().FullPath,
                    CoverletOutputName = Paths.Dist.TestCoverageJsonResult.GetFilename().FullPath,
                    MergeWithFile = context.MakeAbsolute(Paths.Dist.TestCoverageJsonResult),
                });
            }

            context.DotNetCoreTest(Paths.TestBaseProject, new DotNetCoreTestSettings
            {
                Configuration = context.BuildConfiguration,
            }, new CoverletSettings
            {
                CollectCoverage = true,
                CoverletOutputDirectory = Paths.Dist.TestCoverageXmlResult.GetDirectory().FullPath,
                CoverletOutputName = Paths.Dist.TestCoverageXmlResult.GetFilename().FullPath,
                MergeWithFile = context.MakeAbsolute(Paths.Dist.TestCoverageJsonResult),
                CoverletOutputFormat = CoverletOutputFormat.opencover,
            });

            context.DotNetCoreTool("reportgenerator", new Cake.Common.Tools.DotNetCore.Tool.DotNetCoreToolSettings
            {
                ArgumentCustomization = builder =>
                {
                    builder.Append(new TextArgument($"-reports:{Paths.Dist.TestCoverageXmlResult.FullPath}"));
                    builder.Append(new TextArgument($"-targetdir:{Paths.Dist.TestReport.FullPath}"));
                    return builder;
                }
            });
        }
    }
}