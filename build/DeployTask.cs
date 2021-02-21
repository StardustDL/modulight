using Cake.Frosting;

namespace Build
{
    [TaskName("Deploy")]
    [IsDependentOn(typeof(PublishTask))]
    [IsDependentOn(typeof(DeployPackageTask))]
    public class DeployTask : FrostingTask<BuildContext>
    {

    }
}