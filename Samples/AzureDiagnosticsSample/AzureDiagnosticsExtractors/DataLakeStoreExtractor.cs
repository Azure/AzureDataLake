using System.Collections.Generic;
using Microsoft.Analytics.Interfaces;

namespace AzureDiagnosticsExtractors
{
    [SqlUserDefinedExtractor(AtomicFileProcessing = true)]
    public class DataLakeStoreExtractor : IExtractor
    {

        public DataLakeStoreExtractor()
        {
        }

        public override IEnumerable<IRow> Extract(IUnstructuredReader input, IUpdatableRow output_row)
        {
            var s = new System.IO.StreamReader(input.BaseStream);
            {
                var rows = AzureDiagnostics.AzureDiagnosticsUtil.GetLogADLSRecords(s);

                foreach (var row in rows)
                {
                    output_row.Set<System.DateTime>("Time", row.Time.DateTime);
                    output_row.Set<string>("ResourceId", row.ResourceId);
                    output_row.Set<string>("Category", row.Category);
                    output_row.Set<string>("OperationName", row.OperationName);
                    output_row.Set<string>("ResultType", row.ResultType);
                    output_row.Set<string>("ResultSignature", row.ResultType);
                    output_row.Set<string>("CorrelationId", row.CorrelationId);
                    output_row.Set<string>("Identity", row.Identity);

                    var props = row.Properties;
                    output_row.Set<string>("ADLS_StreamName", props.StreamName);

                    yield return output_row.AsReadOnly();
                }
            }

        }
    }
}