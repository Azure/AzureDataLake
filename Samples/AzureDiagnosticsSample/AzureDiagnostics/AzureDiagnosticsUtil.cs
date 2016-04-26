using System.Collections.Generic;

namespace AzureDiagnostics
{
    public static class AzureDiagnosticsUtil
    {
        public static IEnumerable<LogRecord<DataLakeAnalyticsProperties>> GetLogADLARecords(System.IO.StreamReader stream_reader)
        {
            var rows = GetLogRecords<DataLakeAnalyticsProperties>(stream_reader, o => new DataLakeAnalyticsProperties(o));
            return rows;
        }

        public static IEnumerable<LogRecord<DataLakeStoreProperties>> GetLogADLSRecords(System.IO.StreamReader stream_reader)
        {
            var rows = GetLogRecords<DataLakeStoreProperties>(stream_reader, o => new DataLakeStoreProperties(o));
            return rows;
        }

        public static IEnumerable<LogRecord<T>> GetLogRecords<T>(System.IO.StreamReader sr, System.Func<Newtonsoft.Json.Linq.JObject,T> func)
        {
            var jr = new Newtonsoft.Json.JsonTextReader(sr);
            var djom = Newtonsoft.Json.Linq.JObject.ReadFrom(jr);

            foreach (var jt_doc in djom.Children()) // dom contains doc(s)
            {
                foreach (var jt_records in jt_doc.Children()) // foc contains records (an array)
                {
                    foreach (var jt_record in jt_records.Children()) // loop through specific records
                    {
                        var jo_record = (Newtonsoft.Json.Linq.JObject)jt_record;
                        var o = NewAzureDiagnosticLogRecord<T>(jo_record);

                        var PropertiesJSON = (Newtonsoft.Json.Linq.JObject)jo_record["properties"];

                        o.Properties = func(PropertiesJSON);
                        yield return o;
                    }
                }
            }
        }



        public static LogRecord<T> NewAzureDiagnosticLogRecord<T>(Newtonsoft.Json.Linq.JObject jo_record)
        {
            var o = new LogRecord<T>();
            o.Time = jo_record.GetDateTimeOffset("time");
            o.ResourceId = jo_record["resourceId"].ToString();
            o.Category = jo_record["category"].ToString();
            o.OperationName = jo_record["operationName"].ToString();
            o.ResultType = jo_record.GetString("resultType",null);
            o.ResultSignature = jo_record.GetString("resultSignature", null);
            o.CorrelationId = jo_record.GetString( "correlationId" , null );
            o.Identity = jo_record["identity"].ToString();

            return o;
        }
    }


    public static class Extensions
    {
        public static string GetString(this Newtonsoft.Json.Linq.JObject jo, string name, string default_value)
        {
            if (jo[name] != null)
            {
                return jo[name].ToString();
            }
            return default_value;
        }

        public static System.DateTimeOffset? GetDateTimeOffsetNullable(this Newtonsoft.Json.Linq.JObject jo, string name)
        {
            string s = jo.GetString(name, null);
           
            if (s!=null)
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    return null;
                }

                return System.DateTimeOffset.Parse(s);
            }
            return null;
        }

        public static System.DateTimeOffset GetDateTimeOffset(this Newtonsoft.Json.Linq.JObject jo, string name)
        {
            string s = jo.GetString(name, null);
            return System.DateTimeOffset.Parse(s);
        }

    }
}