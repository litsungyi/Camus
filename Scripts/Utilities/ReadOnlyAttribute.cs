using System;
using UnityEngine;

// Ref. https://gist.github.com/LotteMakesStuff/c0a3b404524be57574ffa5f8270268ea
namespace Camus.Utilities
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
}
