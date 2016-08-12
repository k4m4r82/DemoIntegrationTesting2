﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SimPeg.Repository.Api;
using Dapper;
using System.Data;
using System.Data.Common;

namespace SimPeg.Repository.Service
{
    public class DapperContext : IDapperContext
    {
        private readonly string _providerName;
        private readonly string _connectionString;
        private IDbConnection _db;

        public DapperContext()
        {
            _providerName = "System.Data.SqlClient";
            _connectionString = @"Data Source=(local)\SQLSERVER2014;Initial Catalog=SimPeg;Integrated Security=SSPI;";
        }

        private IDbConnection GetOpenConnection(string providerName, string connectionString)
        {
            DbConnection conn = null;

            try
            {
                DbProviderFactory provider = DbProviderFactories.GetFactory(providerName);
                conn = provider.CreateConnection();
                conn.ConnectionString = connectionString;
                conn.Open();
            }
            catch
            {
            }

            return conn;
        }

        public IDbConnection db
        {
            get { return _db ?? (_db = GetOpenConnection(_providerName, _connectionString)); }
        }

        public void Dispose()
        {
            if (_db != null)
            {
                try
                {
                    if (_db.State != ConnectionState.Closed)
                        _db.Close();
                }
                finally
                {
                    _db.Dispose();
                }
            }

            GC.SuppressFinalize(this);
        }
    }
}
