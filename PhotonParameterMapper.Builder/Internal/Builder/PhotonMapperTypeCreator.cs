using PhotonParameterMapper.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace PhotonParameterMapper.Builder.Internal.Builder
{
    internal static class PhotonMapperTypeCreator
    {
        
        public static Type BuildMapper(ModuleBuilder moduleBuilder, string namespacePrefix, Dictionary<Type, Type> contractToContractMapperTypes)
        {
            TypeBuilder typeBuilder = moduleBuilder.DefineType(namespacePrefix + ".PhotonMapper", TypeAttributes.Public | TypeAttributes.Class, typeof(APhotonMapper));

            FieldInfo contractMapperInstances = GetMapperInstancesField(typeBuilder);
            CreateConstructor(typeBuilder, contractMapperInstances, contractToContractMapperTypes);

            return typeBuilder.CreateType();
        }

        private static FieldInfo GetMapperInstancesField(TypeBuilder typeBuilder)
        {
            return typeBuilder.BaseType.GetField("ContractMapperInstances", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        private static void CreateConstructor(TypeBuilder typeBuilder, FieldInfo contractMapperInstances, Dictionary<Type, Type> contractToContractMapperTypes)
        {
            ILGenerator il = CreateConstructorIlGenerator(typeBuilder);

            //TODO: REMOVEME:   I dont know why its going to fail there.
            //TODO:             contractMapperInstances.FieldType Should be defined by parten class :/
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Newobj, contractMapperInstances.FieldType.GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Stfld, contractMapperInstances);
            //TODO: REMOVEME - END

            foreach (KeyValuePair<Type, Type> contractMapperType in contractToContractMapperTypes)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, contractMapperInstances);
                il.EmitTypeOf(contractMapperType.Key);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Newobj, contractMapperType.Value.GetConstructor(new [] { typeof(ICustomTypeBiserializerRegistry) }));
                il.Emit(OpCodes.Callvirt, contractMapperInstances.FieldType.GetMethod("Add"));
            }
            
            il.Emit(OpCodes.Ret);
        }
        
        private static ILGenerator CreateConstructorIlGenerator(TypeBuilder typeBuilder)
        {
            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { typeof(ICustomTypeBiserializerRegistry) });
            constructorBuilder.DefineParameter(1, ParameterAttributes.None, "biserializerRegistry");

            return constructorBuilder.GetILGenerator();
        }
    }
}
