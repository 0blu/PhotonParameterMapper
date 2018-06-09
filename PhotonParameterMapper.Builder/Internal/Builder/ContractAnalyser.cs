using PhotonParameterMapper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PhotonParameterMapper.Builder.Internal.Builder
{
    internal static class ContractAnalyser
    {
        public struct ClassFieldInfo
        {
            public FieldInfo FieldInfo;
            public bool IsOptional;
        }

        public static IReadOnlyDictionary<byte, ClassFieldInfo> GetAllClassFieldInfos(Type contractType)
        {
            byte dataIndex = 0;

            Dictionary<byte, ClassFieldInfo> classFieldInfos = new Dictionary<byte, ClassFieldInfo>();

            foreach (var fieldInfo in contractType.GetFields().OrderBy(f => f.MetadataToken))
            {
                var attr = (DataFieldAttribute)fieldInfo.GetCustomAttribute(typeof(DataFieldAttribute));

                byte currentDataIndex;
                if (attr?.IsCustomFieldIndexSet ?? false)
                {
                    currentDataIndex = attr.FieldIndex;
                }
                else
                {
                    currentDataIndex = dataIndex++;
                }

                var currentIsOptional = attr?.IsOptional ?? true;

                if (classFieldInfos.ContainsKey(currentDataIndex))
                {
                    throw new Exception($"Field[{currentDataIndex}]({fieldInfo.Name}@{fieldInfo.DeclaringType.FullName}) has the same data index as another field");
                }

                classFieldInfos.Add(currentDataIndex, new ClassFieldInfo
                {
                    FieldInfo = fieldInfo,
                    IsOptional = currentIsOptional,
                });
            }

            return classFieldInfos;
        }
    }
}
