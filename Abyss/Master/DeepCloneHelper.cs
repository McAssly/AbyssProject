using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Master
{
    internal class DeepCloneHelper
    {

        /// <summary>
        /// Clones the given array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T[] CloneArray<T>(T[] array)
        {
            List<T> cloned_list = new List<T>(array.Length);
            foreach (T t in array)
            {
                T cloned = DeepClone(t);
                cloned_list.Add(cloned);
            }
            return cloned_list.ToArray();
        }

        /// <summary>
        /// deep clones the given object, ONLY if the object has a non-argumentive constructor and its sub properties also have non-argumentative constructors
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T DeepClone<T>(T src)
        {
            if (src == null) return default;

            Type type = src.GetType();
            if (type.IsValueType || type == typeof(string)) return src;

            object cloned_obj = Activator.CreateInstance(type);

            // Get all fields, including private and non-public ones
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field_info in fields)
            {
                object field = field_info.GetValue(src);
                object cloned_field = DeepClone(field);
                field_info.SetValue(cloned_obj, cloned_field);
            }

            // Get all properties with a public getter and setter
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property_info in properties)
            {
                if (property_info.CanRead && property_info.CanWrite)
                {
                    object property = property_info.GetValue(src);
                    object cloned_property = DeepClone(property);
                    property_info.SetValue(cloned_obj, cloned_property);
                }
            }

            return (T)cloned_obj;
        }
    }
}
