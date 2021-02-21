using Cake.Frosting;

namespace Build
{
    [TaskName("Integration")]
    [IsDependentOn(typeof(BuildTask))]
    [IsDependentOn(typeof(TestTask))]
    [IsDependentOn(typeof(PackTask))]
    [IsDependentOn(typeof(DocumentTask))]
    public class IntegrationTask : FrostingTask<BuildContext>
    {

    }
}