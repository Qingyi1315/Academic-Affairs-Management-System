using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace 教务管理系统
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // 执行非查询命令（如插入、更新、删除）
        public int ExecuteNonQuery(string sql, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"执行非查询命令异常: {ex.Message}");
                    return -1;
                }
            }
        }

        // 执行查询命令并返回 DataTable
        public DataTable ExecuteQuery(string sql, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            return dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"执行查询命令异常: {ex.Message}");
                    return null;
                }
            }
        }

        public object ExecuteScalar(string sql, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        return command.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"数据库查询失败: {ex.Message}");
                    return null;
                }
            }
        }
    }
}