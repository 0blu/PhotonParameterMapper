using PhotonParameterMapper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace PhotonParameterMapper.Builder.Internal.Builder
{
    internal static class ContractMapperBuilder
    {
        private static readonly ConstructorInfo _dictionaryConstructor = typeof(Dictionary<byte, object>).GetConstructor(Type.EmptyTypes);
        private static readonly MethodInfo _dictionaryGetItem = typeof(Dictionary<byte, object>).GetMethod("get_Item", BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo _dictionaryAddItem = typeof(Dictionary<byte, object>).GetMethod("set_Item", BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo _dictionaryContainsKey = typeof(Dictionary<byte, object>).GetMethod("ContainsKey", BindingFlags.Public | BindingFlags.Instance);

        public static Type BuildContractMapper(ModuleBuilder moduleBuilder, string namespacePrefix, Type contractType)
        {
            TypeBuilder typeBuilder = moduleBuilder.DefineType(namespacePrefix + ".Mappers.MapperFor" + contractType.Name, TypeAttributes.Class, null, new []{typeof(ISingleContractMapper)});

            var classFieldInfos = ContractAnalyser.GetAllClassFieldInfos(contractType);

            var biserializerInstaceFields = CreateConstructorAndGeteBiserializerFields(typeBuilder, classFieldInfos);

            CreateToDictionary(contractType, typeBuilder, classFieldInfos, biserializerInstaceFields);
            CreateFromDictionary(contractType, typeBuilder, classFieldInfos, biserializerInstaceFields);

            return typeBuilder.CreateType();
        }


        private static void CreateToDictionary(Type contractType, TypeBuilder typeBuilder, IReadOnlyDictionary<byte, ContractAnalyser.ClassFieldInfo> classFieldInfos, Dictionary<Type, FieldInfo> biserializerInstaceFields)
        {
            ILGenerator il = DefineMethod(typeBuilder, "ToDictionary");

            il.DeclareLocal(contractType);
            il.DeclareLocal(typeof(Dictionary<byte, object>));

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Castclass, contractType);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Newobj, _dictionaryConstructor);
            il.Emit(OpCodes.Stloc_1);

            foreach (var indexClassFieldInfo in classFieldInfos)
            {
                var info = indexClassFieldInfo.Value;
                var fieldInfo = indexClassFieldInfo.Value.FieldInfo;
                var fieldType = fieldInfo.FieldType;

                var skipLabel = new Label();
                var doesHaveSkip = info.IsOptional && (fieldType.IsPrimitive || fieldType.IsEnum || !fieldType.IsValueType);
                if (doesHaveSkip)
                {
                    skipLabel = il.DefineLabel();
                    il.Emit(OpCodes.Ldloc_0);
                    il.Emit(OpCodes.Ldfld, info.FieldInfo);

                    if (fieldType == typeof(float))
                    {
                        il.Emit(OpCodes.Ldc_R4, default(float));
                        il.Emit(OpCodes.Beq_S, skipLabel);
                    }
                    else if (fieldType == typeof(double))
                    {
                        il.Emit(OpCodes.Ldc_R8, default(double));
                        il.Emit(OpCodes.Beq_S, skipLabel);
                    }
                    else
                    {
                        il.Emit(OpCodes.Brfalse_S, skipLabel);
                    }
                }

                il.Emit(OpCodes.Ldloc_1);
                il.Emit(OpCodes.Ldc_I4, (int)indexClassFieldInfo.Key);

                var serializationMethodType = BiserializerMethodProvider.GetSerializerMethodType(fieldType);
                switch (serializationMethodType)
                {
                    case FieldSerializerMethodType.Noop:
                        il.Emit(OpCodes.Ldloc_0);//casted type
                        il.Emit(OpCodes.Ldfld, fieldInfo);
                        break;
                    case FieldSerializerMethodType.Box:
                        il.Emit(OpCodes.Ldloc_0);//casted type
                        il.Emit(OpCodes.Ldfld, fieldInfo);
                        il.Emit(OpCodes.Box, fieldType);
                        break;
                    case FieldSerializerMethodType.UseCustomBiserializerMethod:
                        il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Ldfld, biserializerInstaceFields[fieldType]);

                        il.Emit(OpCodes.Ldloc_0);//casted type
                        il.Emit(OpCodes.Ldfld, fieldInfo);

                        var customSerializer = BiserializerMethodProvider.GetCustomSerializerMethod(fieldType);
                        il.Emit(OpCodes.Callvirt, customSerializer);
                        break;
                    default:
                        throw new ArgumentException($"Unkown serializationMethodType: '{serializationMethodType}'");
                }
                
                il.Emit(OpCodes.Callvirt, _dictionaryAddItem);
                if (doesHaveSkip)
                    il.MarkLabel(skipLabel);
            }

            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Ret);
        }

        private static void CreateFromDictionary(Type contractType, TypeBuilder typeBuilder, IReadOnlyDictionary<byte, ContractAnalyser.ClassFieldInfo> classFieldInfos, Dictionary<Type, FieldInfo> biserializerInstaceFields)
        {
            var defaultConstructor = contractType.GetConstructor(Type.EmptyTypes);
            if (defaultConstructor == null)
                throw new ArgumentException($"Inaccessible default constructor of '{contractType.FullName}'");

            ILGenerator il = DefineMethod(typeBuilder, "FromDictionary");

            il.DeclareLocal(contractType);
            il.Emit(OpCodes.Newobj, defaultConstructor);
            il.Emit(OpCodes.Stloc_0);
            
            foreach (var indexClassFieldInfo in classFieldInfos)
            {
                var info = indexClassFieldInfo.Value;
                var fieldInfo = indexClassFieldInfo.Value.FieldInfo;
                var fieldType = fieldInfo.FieldType;
                var index = (int) indexClassFieldInfo.Key;

                il.BeginExceptionBlock(); // try {
                var afterContainsKeyCheck = new Label();
                if (info.IsOptional)
                {
                    // if(!arg1.ContainsKey(<i>))
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldc_I4_S, index);
                    il.Emit(OpCodes.Callvirt, _dictionaryContainsKey);
                    afterContainsKeyCheck = il.DefineLabel();
                    il.Emit(OpCodes.Brfalse_S, afterContainsKeyCheck);
                    // {
                }

                il.Emit(OpCodes.Ldloc_0);
                var deserializationMethodType = BiserializerMethodProvider.GetDeserializerMethodType(fieldType);
                switch (deserializationMethodType)
                {
                    case FieldSerializerMethodType.Noop:
                        il.Emit(OpCodes.Ldarg_1);
                        il.Emit(OpCodes.Ldc_I4_S, index);
                        il.Emit(OpCodes.Callvirt, _dictionaryGetItem); //arg1[<i>] ...
                        break;
                    case FieldSerializerMethodType.Box:
                        il.Emit(OpCodes.Ldarg_1);
                        il.Emit(OpCodes.Ldc_I4_S, index);
                        il.Emit(OpCodes.Callvirt, _dictionaryGetItem); //arg1[<i>] ...
                        il.Emit(OpCodes.Unbox_Any, fieldType); //(FieldType)arg1[<i>] ...
                        break;
                    case FieldSerializerMethodType.UseStaticMethod:
                        il.Emit(OpCodes.Ldarg_1);
                        il.Emit(OpCodes.Ldc_I4_S, index);
                        il.Emit(OpCodes.Callvirt, _dictionaryGetItem); //arg1[<i>] ...

                        var staticDeserializerMethod = BiserializerMethodProvider.GetStaticDeserializerMethod(fieldType);
                        il.Emit(OpCodes.Call, staticDeserializerMethod);
                        break;
                    case FieldSerializerMethodType.UseCustomBiserializerMethod:

                        il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Ldfld, biserializerInstaceFields[fieldType]);

                        il.Emit(OpCodes.Ldarg_1);
                        il.Emit(OpCodes.Ldc_I4_S, index);
                        il.Emit(OpCodes.Callvirt, _dictionaryGetItem); //arg1[<i>] ...

                        var customDeserializerMethod = BiserializerMethodProvider.GetCustomDeserializerMethod(fieldType);
                        il.Emit(OpCodes.Callvirt, customDeserializerMethod);
                        break;
                    default:
                        throw new ArgumentException($"Unkown deserializationMethodType: '{deserializationMethodType}'");

                }

                il.Emit(OpCodes.Stfld, fieldInfo); //arg0.<fieldInfo> = deserialzer(arg1[<i>]) or (FieldType)arg1[<i>]
                if (info.IsOptional)
                    il.MarkLabel(afterContainsKeyCheck);
                // } catch(Exception e) {
                il.BeginCatchBlock(typeof(Exception));
                il.Emit(OpCodes.Ldc_I4_S, index);
                il.Emit(OpCodes.Newobj, typeof(FieldDeserializationException).GetConstructor(new []{typeof(Exception), typeof(byte) }));
                il.Emit(OpCodes.Throw);
                // }
                il.EndExceptionBlock();
            }

            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);
        }

        private static ILGenerator DefineMethod(TypeBuilder typeBuilder, string baseMethodName)
        {
            MethodInfo baseMethod = typeof(ISingleContractMapper).GetMethod(baseMethodName);
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(baseMethod.Name, MethodAttributes.Public | MethodAttributes.Virtual, CallingConventions.Standard, baseMethod.ReturnType, baseMethod.GetParameters().Select(p => p.ParameterType).ToArray());

            foreach (ParameterInfo parameter in baseMethod.GetParameters())
            {
                methodBuilder.DefineParameter(parameter.Position + 1, ParameterAttributes.None, parameter.Name);
            }

            return methodBuilder.GetILGenerator();
        }

        private static Dictionary<Type, FieldInfo> CreateConstructorAndGeteBiserializerFields(TypeBuilder typeBuilder, IReadOnlyDictionary<byte, ContractAnalyser.ClassFieldInfo> classFieldInfos)
        {
            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { typeof(ICustomTypeBiserializerRegistry) });
            constructorBuilder.DefineParameter(1, ParameterAttributes.None, "biserializerRegistry");

            Dictionary<Type, FieldInfo> biserializerFields = new Dictionary<Type, FieldInfo>();

            ILGenerator il = constructorBuilder.GetILGenerator();

            int i = 0;
            foreach (var indexClassFieldInfo in classFieldInfos)
            {
                var fieldType = indexClassFieldInfo.Value.FieldInfo.FieldType;

                if (biserializerFields.ContainsKey(fieldType))
                    continue;

                var methodTyp = BiserializerMethodProvider.GetSerializerMethodType(fieldType);
                var genricType = typeof(ICustomTypeBiserializer<>).MakeGenericType(fieldType);

                if (methodTyp == FieldSerializerMethodType.UseCustomBiserializerMethod)
                {
                    var field = typeBuilder.DefineField($"_{i++}", genricType, FieldAttributes.Private);
                    biserializerFields.Add(fieldType, field);
                    
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Callvirt, typeof(ICustomTypeBiserializerRegistry).GetMethod("GetCustomTypeBiserializer").MakeGenericMethod(fieldType));
                    il.Emit(OpCodes.Stfld, field);
                }
            }

            il.Emit(OpCodes.Ret);

            return biserializerFields;
        }
    }
}
