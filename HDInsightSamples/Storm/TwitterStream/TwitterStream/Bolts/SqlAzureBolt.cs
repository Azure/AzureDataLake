using Microsoft.SCP;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace TwitterStream.Bolts
{
    public class SqlAzureBolt : ISCPBolt
    {
        public Context context;

        public String sqlConnectionString = null;
        public SqlConnection sqlConnection = null;
        public String tableName = null;

        public SqlCommand sqlDdlCommand = null;

        public SqlCommand sqlInsertCommand = null;
        private bool isSqlInsertCommandPrepared = false;

        //Table Columns - Key = Name & Value = Type
        public Dictionary<string, string> tableColumns = new Dictionary<string, string>();

        public SqlAzureBolt(Context context, Dictionary<string, Object> parms)
        {
            this.context = context;

            //TODO:Specify SQL Connection Settings in SCPHost.exe.config
            this.sqlConnectionString = ConfigurationManager.AppSettings["sql.connectionstring"];
            this.tableName = ConfigurationManager.AppSettings["sql.tablename"];
            var allColumns = ConfigurationManager.AppSettings["sql.tablecolumns"]
                .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (var column in allColumns)
            {
                var columnNameType = column.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                tableColumns.Add(columnNameType[0], columnNameType[1]);
            }

            Dictionary<string, List<Type>> inputSchema = new Dictionary<string, List<Type>>();
            //TODO: Add/Remove fields based on columns in your table - based on app.config entries
            inputSchema.Add(Constants.DEFAULT_STREAM_ID, new List<Type>() 
                { 
                    typeof(long), 
                    typeof(string) 
                }
                );
            this.context.DeclareComponentSchema(new ComponentStreamSchema(inputSchema, null));

            Initialize();
        }

        public static SqlAzureBolt Get(Context context, Dictionary<string, Object> parms)
        {
            return new SqlAzureBolt(context, parms);
        }

        public void Initialize()
        {
            InitSqlAzureConnection();
            this.sqlDdlCommand = this.sqlConnection.CreateCommand();
            this.sqlDdlCommand.CommandText = GenerateCreateTableTSql();
            this.sqlDdlCommand.ExecuteNonQuery();
            this.sqlInsertCommand = this.sqlConnection.CreateCommand();
        }

        public void InitSqlAzureConnection()
        {
            if (this.sqlConnection == null || this.sqlConnection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    this.sqlConnection = new SqlConnection(this.sqlConnectionString);
                    this.sqlConnection.Open();
                }
                catch (Exception ex)
                {
                    Context.Logger.Error("Init connection failed. Error: " + ex.ToString());
                }
            }
        }

        public string GenerateCreateTableTSql()
        {
            var command = new StringBuilder();
            command.AppendFormat("IF EXISTS (SELECT * FROM SYS.TABLES T WHERE T.Name = '{0}') DROP TABLE {0};\r\n", tableName);
            command.AppendFormat("CREATE TABLE {0} (", tableName);

            command.AppendFormat("[ID] BIGINT IDENTITY PRIMARY KEY, [TIMESTAMP] DATE DEFAULT GETDATE(), ");

            int i = 0;
            foreach (var key in this.tableColumns.Keys)
            {
                if (i > 0)
                {
                    command.AppendFormat(", ");
                }
                command.AppendFormat("[{0}] {1}", key, this.tableColumns[key]);
                i++;
            }

            command.AppendFormat(");");
            Context.Logger.Info("Generated CreateTable Sql Command: " + command.ToString());
            return command.ToString();
        }

        public string GenerateInsertTSql()
        {
            var command = new StringBuilder();
            command.AppendFormat("INSERT INTO {0} (", tableName);

            int i = 0;
            foreach (var key in this.tableColumns.Keys)
            {
                if (i > 0)
                {
                    command.AppendFormat(", ");
                }
                command.AppendFormat(key);
                i++;
            }

            command.AppendFormat(")\r\nVALUES (");

            i = 0;
            foreach (var key in this.tableColumns.Keys)
            {
                if (i > 0)
                {
                    command.AppendFormat(", ");
                }
                command.AppendFormat("@" + key);
                i++;
            }

            command.AppendFormat(")");
            Context.Logger.Info("Generated Insert Sql Command: " + command.ToString());
            return command.ToString();
        }

        /// <summary>
        /// Prepare the Insert statement so that subsequent inserts are fast
        /// </summary>
        public void PrepareSqlInsertCommand()
        {
            if (!this.isSqlInsertCommandPrepared)
            {
                this.sqlInsertCommand.CommandText = GenerateInsertTSql();
                this.sqlInsertCommand.Prepare();
                this.isSqlInsertCommandPrepared = true;
                Context.Logger.Info("Prepared Insert Sql Command: " + this.sqlInsertCommand.CommandText);
            }
        }

        public bool Insert(List<object> row)
        {
            //Re-open sql connection if it got reset
            InitSqlAzureConnection();
            PrepareSqlInsertCommand();
            try
            {
                int i = 0;
                foreach (var key in this.tableColumns.Keys)
                {
                    var paramName = "@" + key;
                    if (this.sqlInsertCommand.Parameters.Contains(paramName))
                    {
                        this.sqlInsertCommand.Parameters[paramName].Value = row[i];
                    }
                    else
                    {
                        this.sqlInsertCommand.Parameters.AddWithValue(paramName, row[i]);
                    }
                    i++;
                }
                var rowsImpacted = this.sqlInsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Context.Logger.Error("ERROR encountered while inserting tuple. Command: {0}; Exception: {1}", this.sqlInsertCommand.CommandText, ex.ToString());
                this.isSqlInsertCommandPrepared = false;
                return false;
            }
            return true;
        }

        public void Execute(SCPTuple tuple)
        {
            Context.Logger.Info("SqlAzureBolt: Execute enter.");

            if (Insert(tuple.GetValues()))
            {
                Context.Logger.Info("SqlAzureBolt: Tuple inserted.");
            }
            else
            {
                Context.Logger.Info("SqlAzureBolt: Tuple insert failed.");
            }

            Context.Logger.Info("SqlAzureBolt: Execute exit.");
        }
    }
}
