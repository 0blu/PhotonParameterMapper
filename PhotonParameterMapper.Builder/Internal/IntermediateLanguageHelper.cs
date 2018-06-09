using System;
using System.Reflection;
using System.Reflection.Emit;

namespace PhotonParameterMapper.Builder.Internal
{
    internal static class IntermediateLanguageHelper
    {
        private static readonly MethodInfo _getTypeFromHandleMethodInfo = typeof(Type).GetMethod("GetTypeFromHandle");

        public static void EmitTypeOf(this ILGenerator il, Type type)
        {
            il.Emit(OpCodes.Ldtoken, type);
            il.Emit(OpCodes.Call, _getTypeFromHandleMethodInfo);
        }
    }
}
