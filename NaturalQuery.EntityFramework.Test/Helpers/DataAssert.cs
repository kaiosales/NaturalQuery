using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.EntityFramework.Test.Helpers
{
    public static class DataAssert
    {
        public static void AreEquals(dynamic expected, dynamic actual)
        {
            bool result = true;

            var listA = Enumerable.ToList(expected);
            var listB = Enumerable.ToList(actual);

            if (listA.Count != listB.Count)
            {
                result = false;
            }
            else
            {
                Type typeA = listA.GetType().GenericTypeArguments[0];
                Type typeB = listB.GetType().GenericTypeArguments[0];

                if (typeA.IsSimpleType() && typeB.IsSimpleType())
                {
                    for (int i = 0; i < listA.Count; i++)
                    {
                        var itemA = listA[i];
                        var itemB = listB[i];

                        if (itemA != itemB)
                        {
                            result = false;
                            break;
                        }
                    }
                }
                else
                {
                    PropertyInfo[] propertiesA = typeA.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    Dictionary<string, PropertyInfo> propertiesB = typeB.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).ToDictionary(p => p.Name);

                    if (propertiesA.Length == 0)
                    {
                        result = false;
                    }
                    else
                    {
                        for (int i = 0; i < listA.Count; i++)
                        {
                            var itemA = listA[i];
                            var itemB = listB[i];

                            foreach (PropertyInfo prop in propertiesA)
                            {
                                var valueA = prop.GetValue(itemA);
                                var valueB = propertiesB[prop.Name].GetValue(itemB);

                                if (valueA != valueB)
                                {
                                    result = false;
                                    break;
                                }
                            }

                            if (result == false)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            Assert.IsTrue(result);

        }
    }
}
