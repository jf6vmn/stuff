using System;
using System.Data;
using System.Data.SqlClient;

namespace TutyiMutyiDB
{
    class DataAccessLayer
    {
        private string ConnectionString { get; set; }

        public DataAccessLayer(string connectionString) 
        { ConnectionString = connectionString; }
        public SqlParameter createParameter(string name, object value, DbType dbType)
        { return createParameter(name, 0, value, dbType, ParameterDirection.Input); }
        public SqlParameter createParameter(string name, int size, object value, DbType dbType) 
        { return createParameter(name, size, value, dbType, ParameterDirection.Input); }
        public SqlParameter createParameter(string name, int size, object value, DbType dbType, ParameterDirection direction) 
        { return new SqlParameter 
        { 
            DbType = dbType,
            ParameterName = name,
            Size = size, 
            Direction = direction, 
            Value = value }; 
        }
        public void CloseConnection(SqlConnection connection)
        { connection.Close(); }
        public DataTable getDataTable(string commandText, CommandType commandType, SqlParameter[] parameters = null) 
        { 
            using (var connection = new SqlConnection(ConnectionString)) 
            { connection.Open(); 
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null) 
                    { 
                        foreach (var parameter in parameters) command.Parameters.Add(parameter); 
                    }
                    var dataSet = new DataSet();
                    var dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(dataSet); 
                    return dataSet.Tables[0]; 
                }
            } 
        }
        public DataSet getDataSet(string commandText, CommandType commandType, SqlParameter[] parameters = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
            }
            using (var command = new SqlCommand(commandText, connection)) { command.CommandType = commandType; if (parameters != null) { foreach (var parameter in parameters) command.Parameters.Add(parameter); } var dataSet = new DataSet(); var dataAdapter = new SqlDataAdapter(command); dataAdapter.Fill(dataSet); return dataSet; }
        }
    }
    public IDataReader getDataReader(string commandText, CommandType commandType, SqlParameter[] parameters, out SqlConnection connection)
    { 
        IDataReader reader = null; connection = new SqlConnection(ConnectionString);
        var command = new SqlCommand(commandText, connection) { CommandType = commandType }; 
        if (parameters != null) 
        { 
            foreach (var parameter in parameters) command.Parameters.Add(parameter);
        } 
        connection.Open(); 
        reader = command.ExecuteReader();
        return reader; 
    }
    public object getScalarValue(string commandText, CommandType commandType, SqlParameter[] parameters = null)
    { 
        using (var connection = new SqlConnection(ConnectionString)) 
        {
            connection.Open(); using (var command = new SqlCommand(commandText, connection))
            { 
                command.CommandType = commandType; 
                if (parameters != null) 
                { 
                    foreach (var parameter in parameters)
                        command.Parameters.Add(parameter);
                } return command.ExecuteScalar();
            } 
        } 
    }
    public void delete(string commandText, CommandType commandType, SqlParameter[] parameters = null)
    {
        using (var connection = new SqlConnection(ConnectionString))
            connection.Open(); using (var command = new SqlCommand(commandText, connection)) { command.CommandType = commandType; if (parameters != null) { foreach (var parameter in parameters) command.Parameters.Add(parameter); } command.ExecuteNonQuery(); }
    }

public void insert(string commandText, CommandType commandType, SqlParameter[] parameters) 
{
    using (var connection = new SqlConnection(ConnectionString)) 
    { connection.Open(); 
        using (var command = new SqlCommand(commandText, connection)) 
        { command.CommandType = commandType; 
            if (parameters != null) 
            { 
                foreach (var parameter in parameters) command.Parameters.Add(parameter); 
            } 
            command.ExecuteNonQuery(); 
        } 
    }
}
public int insert(string commandText, CommandType commandType, SqlParameter[] parameters, out int lastId)
{ lastId = 0; using (var connection = new SqlConnection(ConnectionString))
    { connection.Open(); 
        using (var command = new SqlCommand(commandText, connection))
        { command.CommandType = commandType;
            if (parameters != null) 
            { 
                foreach (var parameter in parameters) command.Parameters.Add(parameter); 
            } 
            object newId = command.ExecuteScalar();
            lastId = Convert.ToInt32(newId);
        }
    } 
    return lastId;
}

public long insert(string commandText, CommandType commandType, SqlParameter[] parameters, out long lastId)
{
    lastId = 0; using (var connection = new SqlConnection(ConnectionString))
    {
        connection.Open();
        using (var command = new SqlCommand(commandText, connection))
        {
            command.CommandType = commandType;
            if (parameters != null)
            { 
                foreach (var parameter in parameters) command.Parameters.Add(parameter);
            } 
            object newId = command.ExecuteScalar();
            lastId = Convert.ToInt64(newId);
        }
    }
    return lastId;
}
public void insertWithTransaction(string commandText, CommandType commandType, SqlParameter[] parameters)
{
    SqlTransaction transaction = null;
    using (var connection = new SqlConnection(ConnectionString))
    { 
        connection.Open();
        transaction = connection.BeginTransaction();
        using (var command = new SqlCommand(commandText, connection))
        {
            command.CommandType = commandType;
            if (parameters != null)
            {
                foreach (var parameter in parameters) command.Parameters.Add(parameter); 
            }
            try 
            { 
                command.ExecuteNonQuery(); 
                transaction.Commit(); 
            }
            catch (Exception) 
            {
                transaction.Rollback(); 
            }
            finally
            { connection.Close(); 
            } 
        } 
    }
}

    public void insertWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
    {
        SqlTransaction transaction = null; using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open(); transaction = connection.BeginTransaction(isolationLevel);
            using (var command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType; if (parameters != null)
                { 
                    foreach (var parameter in parameters) command.Parameters.Add(parameter); 
                }
                try 
                { 
                    command.ExecuteNonQuery();
                    transaction.Commit();
                } 
                catch (Exception) 
                {
                    transaction.Rollback();
                }
                finally
                { 
                    connection.Close();
                }
            }
        }
    }
    public void update(string commandText, CommandType commandType, SqlParameter[] parameters) 
    { 
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open(); 
            using (var command = new SqlCommand(commandText, connection)) 
            {
                command.CommandType = commandType;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                        command.Parameters.Add(parameter);
                }
                command.ExecuteNonQuery();
            } 
        } 
    }
    public void updateWithTransaction(string commandText, CommandType commandType, SqlParameter[] parameters)
    {
        SqlTransaction transaction = null; using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open(); 
            transaction = connection.BeginTransaction();
            using (var command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType; 
                if (parameters != null)
                {
                    foreach (var parameter in parameters) command.Parameters.Add(parameter);
                }
                try
                {
                    command.ExecuteNonQuery();
                    transaction.Commit(); 
                }
                catch (Exception) 
                { 
                    transaction.Rollback(); 
                }
                finally
                { 
                    connection.Close(); 
                }
            }
        }
    }
    public void updateWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
    { 
        SqlTransaction transaction = null;
        using (var connection = new SqlConnection(ConnectionString)) 
        { connection.Open(); 
            transaction = connection.BeginTransaction(isolationLevel);
            using (var command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType; 
                if (parameters != null) 
                {
                    foreach (var parameter in parameters) command.Parameters.Add(parameter);
                }
                try 
                { 
                    command.ExecuteNonQuery(); 
                    transaction.Commit(); 
                }
                catch (Exception) 
                {
                    transaction.Rollback();
                }
                finally 
                { 
                    connection.Close();
                }
            } 
        } 
    }
}