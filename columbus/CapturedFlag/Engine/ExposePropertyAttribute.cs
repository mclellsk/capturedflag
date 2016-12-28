using System;

/// <summary>
/// Used to expose property fields to the editor.
/// </summary>
namespace CapturedFlag.Engine
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExposePropertyAttribute : Attribute { }
}