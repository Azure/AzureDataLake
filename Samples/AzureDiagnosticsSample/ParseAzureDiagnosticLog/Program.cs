using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ParseAzureDiagnosticLog
{

    class Program
    {
        private static bool quiet=true;

        static void Main(string[] args)
        {
            string this_asm = System.Reflection.Assembly.GetAssembly(typeof (Program)).Location;
            string this_folder = System.IO.Path.GetDirectoryName(this_asm);
            string input_folder = System.IO.Path.Combine(this_folder, "Input");

            parse_adls_logs_in_folder(input_folder);
            parse_adla_logs_in_folder(input_folder);
        }

        private static void parse_adls_logs_in_folder(string input_folder)
        {
            var files = System.IO.Directory.GetFiles(input_folder, "ADLS*.json");

            foreach (var file in files)
            {
                using (var stream_reader = new System.IO.StreamReader(file))
                {
                    foreach (var row in AzureDiagnostics.AzureDiagnosticsUtil.GetLogRecords<AzureDiagnostics.DataLakeStoreProperties>(stream_reader, o=> new AzureDiagnostics.DataLakeStoreProperties(o)))
                    {
                        if (!quiet)
                        {
                            Console.WriteLine("-----------------------------");
                            Console.WriteLine("Time = {0}", row.Time);
                            Console.WriteLine("ResourceId = {0}", row.ResourceId);
                            Console.WriteLine("Category = {0}", row.Category);
                            Console.WriteLine("OperationName = {0}", row.OperationName);
                            Console.WriteLine("ResultType = {0}", row.ResultType);
                            Console.WriteLine("CorrelationId = {0}", row.CorrelationId);
                            Console.WriteLine("Identity = {0}", row.Identity);
                        }
                    }
                }
            }
        }

        private static void parse_adla_logs_in_folder(string input_folder)
        {
            var files = System.IO.Directory.GetFiles(input_folder, "ADLA*.json");

            foreach (var file in files)
            {
                using (var stream_reader = new System.IO.StreamReader(file))
                {
                    var rows = AzureDiagnostics.AzureDiagnosticsUtil.GetLogRecords<AzureDiagnostics.DataLakeAnalyticsProperties>(stream_reader, o => new AzureDiagnostics.DataLakeAnalyticsProperties(o));
                    foreach (var row in rows)
                    {
                        if (!quiet)
                        {
                            Console.WriteLine("-----------------------------");
                            Console.WriteLine("Time = {0}", row.Time);
                            Console.WriteLine("ResourceId = {0}", row.ResourceId);
                            Console.WriteLine("Category = {0}", row.Category);
                            Console.WriteLine("OperationName = {0}", row.OperationName);
                            Console.WriteLine("ResultType = {0}", row.ResultType);
                            Console.WriteLine("CorrelationId = {0}", row.CorrelationId);
                            Console.WriteLine("Identity = {0}", row.Identity);
                        }
                    }
                }
            }
        }

    }
}
