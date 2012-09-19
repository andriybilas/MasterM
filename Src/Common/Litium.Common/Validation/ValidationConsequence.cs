using System;

namespace Litium.Common.Validation
{
    public sealed class ValidationConsequence
    {
        internal ValidationConsequence (Type targetType, String targetFullTypeName, String message)
        {
            TargetType = targetType;
            TargetFullTypeName = targetFullTypeName;
            Message = message;
        }
        public Type TargetType { get; private set; }
        public string TargetFullTypeName { get; private set; }
        public string Message { get; private set; }
    }
}