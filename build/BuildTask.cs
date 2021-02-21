using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Build;
using Cake.Frosting;

namespace Build
{
    [TaskName("Build")]
    [IsDependentOn(typeof(RestoreTask))]
    public sealed class BuildTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            foreach (var solution in context.SolutionFiles)
            {
                try
                {
                    context.DotNetCoreBuild(solution.FullPath, new DotNetCoreBuildSettings
                    {
                        MSBuildSettings = context.GetMSBuildSettings(),
                    });
                }
                catch
                {
                    context.DotNetCoreBuild(solution.FullPath, new DotNetCoreBuildSettings
                    {
                        MSBuildSettings = context.GetMSBuildSettings(),
                    });
                }
            }
        }
    }
}