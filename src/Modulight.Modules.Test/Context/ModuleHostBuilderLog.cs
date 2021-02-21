using System;

namespace Modulight.Modules.Test.Context
{
    public record ModuleHostBuilderLog
    {
        public Type[] ModuleProcessingOrder { get; init; } = Array.Empty<Type>();
    }
}
