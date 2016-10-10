using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Analytics;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLake.Analytics
{
    public class AnalyticsCatalogClient : AccountClientBase
    {
        private ADL.Analytics.IDataLakeAnalyticsCatalogManagementClient _adla_catalog_rest_client;

        public AnalyticsCatalogClient(string account, AzureDataLake.Authentication.AuthenticatedSession authSession) :
            base(account, authSession)
        {
            this._adla_catalog_rest_client = new ADL.Analytics.DataLakeAnalyticsCatalogManagementClient(this.AuthenticatedSession.Credentials);
        }

        public ADL.Analytics.Models.USqlDatabase GetDatabase(GetJobListOptions options, string name)
        {

            var db = this._adla_catalog_rest_client.Catalog.GetDatabase(this.Account, name);
            return db;
        }

        public IEnumerable<ADL.Analytics.Models.USqlDatabase[]> ListDatabasesPaged()
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<ADL.Analytics.Models.USqlDatabase>();

            string @select = null;
            bool? count = null;

            // Handle the initial response
            var page = this._adla_catalog_rest_client.Catalog.ListDatabases(this.Account, oDataQuery, @select, count);
            foreach (var cur_page in RESTUtil.EnumPages<ADL.Analytics.Models.USqlDatabase> (page, p => this._adla_catalog_rest_client.Catalog.ListDatabasesNext(p.NextPageLink)))
            {
                yield return cur_page;
            }
        }

        public IEnumerable<ADL.Analytics.Models.USqlAssemblyClr[]> ListAssembliesPaged(string name, string dbname)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<ADL.Analytics.Models.USqlAssembly>();

            string @select = null;
            bool? count = null;

            // Handle the initial response
            var page = this._adla_catalog_rest_client.Catalog.ListAssemblies(this.Account, dbname, oDataQuery, @select, count);
            foreach (var cur_page in RESTUtil.EnumPages<ADL.Analytics.Models.USqlAssemblyClr>(page, p => this._adla_catalog_rest_client.Catalog.ListAssembliesNext(p.NextPageLink)))
            {
                yield return cur_page;
            }
        }

        public IEnumerable<ADL.Analytics.Models.USqlExternalDataSource[]> ListExternalDatasourcesPaged(string name, string dbname)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<ADL.Analytics.Models.USqlExternalDataSource>();

            string @select = null;
            bool? count = null;

            // Handle the initial response
            var page = this._adla_catalog_rest_client.Catalog.ListExternalDataSources(this.Account, dbname, oDataQuery, @select, count);
            foreach (var cur_page in RESTUtil.EnumPages<ADL.Analytics.Models.USqlExternalDataSource>(page, p => this._adla_catalog_rest_client.Catalog.ListExternalDataSourcesNext(p.NextPageLink)))
            {
                yield return cur_page;
            }
        }

        public IEnumerable<ADL.Analytics.Models.USqlProcedure[]> ListProceduresPaged(string name, string dbname, string schema)
        {
            var oDataQuery = new Microsoft.Rest.Azure.OData.ODataQuery<ADL.Analytics.Models.USqlProcedure>();

            string @select = null;
            bool? count = null;

            // Handle the initial response
            var page = this._adla_catalog_rest_client.Catalog.ListProcedures(this.Account, dbname, schema, oDataQuery, @select, count);
            foreach (var cur_page in RESTUtil.EnumPages<ADL.Analytics.Models.USqlProcedure>(page, p => this._adla_catalog_rest_client.Catalog.ListProceduresNext(p.NextPageLink)))
            {
                yield return cur_page;
            }
        }

    }
}