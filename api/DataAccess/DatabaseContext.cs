using Helper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(AppConfig config)
        {
            _connectionString = config.GetValue("DbConnection");
        }
        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<SqlParameter> SqlParameters => new List<SqlParameter>();
        public async Task<DataSet> ExecuteDataSetAsync(CommandType cmdType, string cmdString, IEnumerable<SqlParameter> sqlParameters = null)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(cmdString, conn);
            using var da = new SqlDataAdapter(cmd);

            cmd.CommandType = cmdType;

            if (sqlParameters != null)
                cmd.Parameters.AddRange(sqlParameters.ToArray());

            await conn.OpenAsync();

            var ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public async Task<DataTable> ExecuteDataTableAsync(CommandType cmdType, string cmdString, IEnumerable<SqlParameter> sqlParameters = null)
        {
            var ds = await ExecuteDataSetAsync(cmdType, cmdString, sqlParameters);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }
        public async Task<DbResponse> ExecuteScalarAsync(CommandType cmdType, string cmdString, IEnumerable<SqlParameter> sqlParameters = null)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(cmdString, conn);

                cmd.CommandType = cmdType;

                if (sqlParameters != null)
                    cmd.Parameters.AddRange(sqlParameters.ToArray());

                await conn.OpenAsync();

                var result = await cmd.ExecuteScalarAsync();

                if (result == null)
                {
                    return new DbResponse
                    {
                        HasError = true,
                        Message = "No result returned from the database."
                    };
                }
                return new DbResponse
                {
                    Message = "Success",
                    Response = result
                };
            }
            catch (SqlException ex)
            {
                return new DbResponse
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }
        public async Task<DbResponse> ExecuteNonQueryAsync(CommandType cmdType, string cmdString, IEnumerable<SqlParameter> sqlParameters = null)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(cmdString, conn);

                cmd.CommandType = cmdType;

                if (sqlParameters != null)
                    cmd.Parameters.AddRange(sqlParameters.ToArray());

                await conn.OpenAsync();
                int affectedRows = await cmd.ExecuteNonQueryAsync();

                return new DbResponse
                {
                    Message = "Success",
                    Response = affectedRows
                };
            }
            catch (SqlException ex)
            {
                return new DbResponse
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }
    }


}
