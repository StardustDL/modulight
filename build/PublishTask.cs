using Cake.Frosting;

namespace Build
{
    [TaskName("Publish")]
    [IsDependentOn(typeof(BuildTask))]
    [IsDependentOn(typeof(PackTask))]
    [IsDependentOn(typeof(DocumentTask))]
    public class PublishTask : FrostingTask<BuildContext>
    {

    }
}