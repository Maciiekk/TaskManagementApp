using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
namespace APPToDo
{
    internal class Connection
    {
        string connstring = String.Format("Server={0};Port={1};" +
            "User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "qaz", "localdb");
        NpgsqlConnection conn;
        private static string activeUser="";
        public Connection()
        {
            conn = new NpgsqlConnection(connstring);
        }

        public async Task<bool> Login(string email, string password)
        {
           // TO DO - add try catch
            conn.Open();
            string command = "SELECT * FROM users WHERE usersemail='" + email + "' AND userspassword= '" + password + "'";
            await using var cmd = new NpgsqlCommand(command, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                conn.Close();
                activeUser = email;
                return true;
            }
            else
            {
                conn.Close();
                return false;
            }

        }

        public async Task<bool> Register(string email, string password, string passwordConfirm)
        {
            conn.Open();
            //TO DO -add try catch -db connection exeption
            try
            {
                string command = "INSERT INTO users (usersemail,userspassword) Values ('" + email + "' ,'" + password + "'); ";
                await using var cmd = new NpgsqlCommand(command, conn);
                await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return true;
            }
            catch (NpgsqlException)
            {
                // if x = 23505....  <- primary key duplicate
                conn.Close();
                return false;
                throw;
            }

          
        }

        public async Task<bool> ListData(List<Task> tasks)
        {
            // TO DO - add try catch
            conn.Open();
            string command = "select tasksname, taskscontent,tasksid,taskscategory from tasks where tasksownersemail='"+activeUser+ "'order by tasksid;";
            await using var cmd = new NpgsqlCommand(command, conn);
            cmd.Connection = conn;
            NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);
            DataSet dataSet = new DataSet();
            Adapter.Fill(dataSet,"LIST");
            int counter = dataSet.Tables["LIST"].Rows.Count;
            int i = 0;
            while(i<counter)
            {
                tasks.Add(new Task()
                {
                    tasksname =dataSet.Tables["LIST"].Rows[i]["tasksname"].ToString(),
                    taskscontent=dataSet.Tables["LIST"].Rows[i]["taskscontent"].ToString(),
                    tasksid = Convert.ToInt32(dataSet.Tables["LIST"].Rows[i]["tasksid"].ToString())

                });
                i++;
            }
            
            conn.Close();
            return true; 
        }

        public async Task<bool> CreateTask(string taskName, string taskContent)
        {
            conn.Open();
            try
            {
                string command = "INSERT INTO tasks (tasksname,taskscontent,tasksownersemail,taskscategory) Values ('" + taskName + "' ,'" + taskContent + "','"+ activeUser + "',0); ";
                await using var cmd = new NpgsqlCommand(command, conn);
                await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return true;
            }
            catch (NpgsqlException)
            {
                // if x = 23505....  <- primary key duplicate
                conn.Close();
                return false;
                throw;
            }

            
        }


        public async Task<bool> EditTask(int tasksid,string taskName, string taskContent)
        {
            conn.Open();
            try
            {
                await using var cmd = new NpgsqlCommand("UPDATE tasks SET tasksname =$1,taskscontent=$1 where tasksid=$3", conn)
                {
                    Parameters =
                {
                new() { Value = taskName },
                new() { Value = taskContent},
                new() { Value = tasksid}
                }
                };
                await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return true;
            }
            catch (NpgsqlException)
            {
                // if x = 23505....  <- primary key duplicate
                conn.Close();
                return false;
                throw;
            }
 
 
        }
        public async Task<bool> DeleteTask(int tasksid)
        {
            conn.Open();
            try
            {
                await using var cmd = new NpgsqlCommand("Delete from tasks  where tasksid=$1", conn)
                {
                    Parameters =
                {
                new() { Value = tasksid}
                }
                };
                await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return true;
            }
            catch (NpgsqlException)
            {
                // if x = 23505....  <- primary key duplicate
                conn.Close();
                return false;
                throw;
            }
        
        }
    }
}
