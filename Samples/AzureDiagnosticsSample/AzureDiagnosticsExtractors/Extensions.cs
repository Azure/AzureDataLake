using Microsoft.Analytics.Types.Sql;
using System;
using System.Linq;
using System.Text;

namespace AzureDiagnosticsExtractors
{
    static class Extensions
    {
        public static System.DateTime? ToDateTimeNullable(this System.DateTimeOffset? dto)
        {
            System.DateTime? dt;
            if (dto.HasValue)
            {
                dt = (System.DateTime?)dto.Value.DateTime;
            }
            else
            {
                dt = null;
            }
            return dt;
        }
    }
}

