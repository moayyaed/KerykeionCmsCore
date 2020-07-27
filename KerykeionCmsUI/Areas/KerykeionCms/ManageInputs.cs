using System;

namespace KerykeionCmsUI.Areas.KerykeionCms
{
    public class ManageInputs
    {
        public static string GetInputType(string clrTypeName) => ReturnSpecificInputType(clrTypeName);
        public static string GetInputValue(string val) => ReturnValue(val);
        public static string GetDefaultValue(string clrTypeName) => ReturnDefaultValue(clrTypeName);

        private static string ReturnDefaultValue(string clrTypeName)
        {
            if (clrTypeName.Contains("decimal", StringComparison.OrdinalIgnoreCase) || clrTypeName.Contains("single", StringComparison.OrdinalIgnoreCase) || clrTypeName.Contains("double", StringComparison.OrdinalIgnoreCase) || clrTypeName.Contains("int", StringComparison.OrdinalIgnoreCase) || clrTypeName.Contains("byte", StringComparison.OrdinalIgnoreCase))
            {
                return "value=0";
            }
            return "";
        }

        private static string ReturnValue(string val)
        {
            if (bool.TryParse(val, out _))
            {
                if (bool.Parse(val))
                {
                    return "checked";
                }
                return "";
            }

            if (string.IsNullOrEmpty(val))
            {
                return "";
            }

            if (int.TryParse(val, out _))
            {
                return $"{int.Parse(val)}";
            }

            if (decimal.TryParse(val, out _))
            {
                return $"{decimal.Parse(val)}";
            }

            if (TimeSpan.TryParse(val, out _))
            {
                return $"{TimeSpan.Parse(val):hh\\:mm}";
            }

            if (DateTime.TryParse(val, out _))
            {
                return $"{DateTime.Parse(val):dd-MM-yyyy HH:mm}";
            }


            return $"{val}";
        }

        private static string ReturnSpecificInputType(string clrTypeFullName)
        {
            if (clrTypeFullName == null)
            {
                return "type=text";
            }

            if (clrTypeFullName.Contains("string", StringComparison.OrdinalIgnoreCase)) return "type=text";
            if (clrTypeFullName.Contains("int", StringComparison.OrdinalIgnoreCase) || clrTypeFullName.Contains("byte", StringComparison.OrdinalIgnoreCase)) return "type=number";
            if (clrTypeFullName.Contains("bool", StringComparison.OrdinalIgnoreCase)) return "type=checkbox value=True";
            if (clrTypeFullName.Contains("date", StringComparison.OrdinalIgnoreCase)) return "type=datetime-local";
            if (clrTypeFullName.Contains("char", StringComparison.OrdinalIgnoreCase)) return "type=text maxlength=1";
            if (clrTypeFullName.Contains("time", StringComparison.OrdinalIgnoreCase)) return "type=time";

            return "type=text";
        }
    }
}
