using System;

namespace Modulight.Modules
{
    /// <summary>
    /// Exception in modulight
    /// </summary>
    [Serializable]
    public class ModulightException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public ModulightException() { }

        /// <inheritdoc/>
        public ModulightException(string message) : base(message) { }

        /// <inheritdoc/>
        public ModulightException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ModulightException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Incompatible type
    /// </summary>
    [Serializable]
    public class IncompatibleTypeException : ModulightException
    {
        /// <summary>
        /// 
        /// </summary>
        public Type ExpectedType { get; }

        /// <summary>
        /// 
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// 
        /// </summary>
        public IncompatibleTypeException(Type type, Type expected) : base($"{type.FullName} is not expected type {expected.FullName}.")
        {
            Type = type;
            ExpectedType = expected;
        }
    }
}
