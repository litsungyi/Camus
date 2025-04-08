using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class PostBuild
{
    public static unsafe void Replace(this MethodInfo methodToReplace, MethodInfo methodToInject)
    {
        RuntimeHelpers.PrepareMethod(methodToReplace.MethodHandle);
        RuntimeHelpers.PrepareMethod(methodToInject.MethodHandle);

        IntPtr tar = methodToReplace.MethodHandle.Value;
        if (!methodToReplace.IsVirtual)
        {
            tar += 8;
        }
        else
        {
            var index = (int) (((*(long*) tar) >> 32) & 0xFF);
            var classStart = *(IntPtr*) (methodToReplace.DeclaringType.TypeHandle.Value + (IntPtr.Size == 4 ? 40 : 64));
            tar = classStart + IntPtr.Size * index;
        }
        var inj = methodToInject.MethodHandle.Value + 8;
        tar = *(IntPtr*) tar + 1;
        inj = *(IntPtr*) inj + 1;

        *(int*) tar = *(int*) inj + (int) (long) inj - (int) (long) tar;
    }
}
