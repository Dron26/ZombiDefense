using System;
using System.Collections.Generic;
using System.Reflection;
using Humanoids.AbstractLevel;

namespace Humanoids
{
    public class HumanoidFinder
    {
        public static List<Type> FindAllHumanoidClasses()
        {
            Type humanoidType = typeof(Humanoid);
            List<Type> humanoidClasses = new List<Type>();
            FindAllDerivedTypes(AppDomain.CurrentDomain.GetAssemblies(), humanoidType, humanoidClasses);
            return humanoidClasses;
        }

        private static void FindAllDerivedTypes(Assembly[] assemblies, Type baseType, List<Type> result)
        {
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (type != baseType && baseType.IsAssignableFrom(type))
                    {
                        result.Add(type);
                    }
                }
            }
        }
    }
}