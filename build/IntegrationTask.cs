using Cake.Frosting;

namespace Build
{
    [TaskName("Integration")]
    [IsDependentOn(typeof(TestTask))]
    [IsDependentOn(typeof(PublishTask))]
    public class IntegrationTask : FrostingTask<BuildContext>
    {

    }
}