using System.Collections.Generic;
using Microsoft.Analytics.Interfaces;

namespace AzureDiagnosticsExtractors
{
    [SqlUserDefinedExtractor(AtomicFileProcessing = true)]
    public class DataLakeAnalyticsExtractor : IExtractor
    {

        public DataLakeAnalyticsExtractor()
        {
        }

        public override IEnumerable<IRow> Extract(IUnstructuredReader input, IUpdatableRow output_row)
        {
            var s = new System.IO.StreamReader(input.BaseStream);
            {
                var rows = AzureDiagnostics.AzureDiagnosticsUtil.GetLogADLARecords(s);

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
                    output_row.Set<string>("ADLA_JobId", props.JobId);
                    output_row.Set<string>("ADLA_JobName", props.JobName);
                    output_row.Set<string>("ADLA_JobRuntimeName", props.JobRuntimeName);


                    output_row.Set<System.DateTime?>("ADLA_StartTime", props.StartTime.ToDateTimeNullable());
                    output_row.Set<System.DateTime?>("ADLA_SubmitTime", props.SubmitTime.ToDateTimeNullable());
                    output_row.Set<System.DateTime?>("ADLA_EndTime", props.EndTime.ToDateTimeNullable());

                    yield return output_row.AsReadOnly();
                }
            }

        }
    }
}