using Cake.Common.IO;
using Cake.DocFx;
using Cake.Frosting;

namespace Build
{
    [TaskName("Document")]
    [IsDependentOn(typeof(BuildTask))]
    [IsDependentOn(typeof(TestTask))]
    public class DocumentTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            context.CleanDirectory(Paths.Dist.Documents);

            context.DocFxMetadata(Paths.DocFxConfigFile);

            context.DocFxBuild(Paths.DocFxConfigFile, new Cake.DocFx.Build.DocFxBuildSettings
            {
                OutputPath = Paths.Dist.Documents,
            });

            context.CopyDirectory(Paths.Dist.TestReport, Paths.Dist.CoverageDocuments);
        }
    }
}