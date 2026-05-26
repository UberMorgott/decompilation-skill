using System;

namespace AGInterfaces;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class TranslatableAttribute : Attribute
{
}
