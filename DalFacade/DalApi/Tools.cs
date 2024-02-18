using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public static class Tools
{
    /*internal static string ToStringProperty<Item>(this Item item)
 => string.Join(Environment.NewLine, from property in item?.GetType().GetProperties()
                                     let value = property.GetValue(item)
                                     select property.PropertyType.IsAssignableTo(typeof(IEnumerable<>)) ?
                                    $"{property.Name}:  {string.Join(Environment.NewLine, value)}" : $"{property.Name}: {property.GetValue(item)}");*/
    
    public static string StringProperty<T>(this T obj)
    {

         Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            string result = $"{type.Name} properties:\n";

            foreach (var property in properties)
            {
                object? value = property.GetValue(obj);

                if (value is System.Collections.IEnumerable enumerable && !(value is string))
                {
                    // Handle IEnumerable properties (e.g., collections)
                    string elements = string.Join("\n", enumerable.Cast<object>());
                    result += $"{property.Name}: {elements}\n";
                }
                else
                {
                    // Handle other properties
                    result += $"{property.Name}: {value}\n";
                }
            }

            return result;
    }
}
