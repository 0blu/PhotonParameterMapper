using PhotonParameterMapper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PhotonParameterMapper.Builder.Internal.Builder
{
    internal enum FieldSerializerMethodType
    {
        UseStaticMethod,
        UseCustomBiserializerMethod,
        Box,
        Noop
    }

    internal static class BiserializerMethodProvider
    {
        public static FieldSerializerMethodType GetDeserializerMethodType(Type type)
        {
            var methodInfo = FindMethodByReturnType(typeof(Convert), type);
            if (methodInfo != null)
                return FieldSerializerMethodType.UseStaticMethod;

            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                while (elementType.IsArray)
                    elementType = elementType.GetElementType();
                if (elementType.IsPrimitive || elementType.IsEnum)
                    return FieldSerializerMethodType.Box;
            }

            if (type.IsEnum)
                return FieldSerializerMethodType.Box;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                return FieldSerializerMethodType.Noop;

            // Use custom biserializer as default
            return FieldSerializerMethodType.UseCustomBiserializerMethod;
        }

        public static FieldSerializerMethodType GetSerializerMethodType(Type type)
        {
            if (type.IsArray)
            {
                var elementType = type;
                while (elementType.IsArray)
                    elementType = elementType.GetElementType();
                if (elementType.IsPrimitive || elementType.IsEnum)
                    return FieldSerializerMethodType.Noop;
                if (elementType == typeof(string))
                    return FieldSerializerMethodType.Noop;
            }

            if (type.IsPrimitive || type.IsEnum)
                return FieldSerializerMethodType.Box;
            if (type == typeof(string))
                return FieldSerializerMethodType.Noop;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                return FieldSerializerMethodType.Noop;

            // Use custom biserializer as default
            return FieldSerializerMethodType.UseCustomBiserializerMethod;
        }
        
        private static MethodInfo FindMethodByReturnType(Type declaringType, Type returnType)
        {
            return declaringType.GetMethods().FirstOrDefault(f => f.ReturnType == returnType && f.GetParameters().Length == 1 && f.GetParameters()[0].ParameterType == typeof(object));
        }

        public static MethodInfo GetStaticDeserializerMethod(Type fieldType)
        {
            return FindMethodByReturnType(typeof(Convert), fieldType);
        }

        public static MethodInfo GetCustomSerializerMethod(Type contractType)
        {
            return typeof(ICustomTypeBiserializer<>).MakeGenericType(contractType).GetMethod("Serialize");
        }

        public static MethodInfo GetCustomDeserializerMethod(Type contractType)
        {
            return typeof(ICustomTypeBiserializer<>).MakeGenericType(contractType).GetMethod("Deserialize");
        }
    }
}
