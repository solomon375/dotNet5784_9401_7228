using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public static class Tools
{
    public static string ToStringProperty<T>(this T obj)
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
