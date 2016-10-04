namespace AzureDataLake
{
    public static class StringUtil
    {
        public static string ToLowercaseFirstLetter(string field_name_str)
        {
            string result = field_name_str.Substring(0, 1).ToLowerInvariant() + field_name_str.Substring(1);
            return result;
        }
    }
}