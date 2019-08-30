using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ConnectToDBApp
{
    class Program
    {
        public static string connectionstring;

        static void Main(string[] args)
        {
            connectionstring = "<copy connection string from Azure>";
            var conn = new SqlConnection(connectionstring);

            var cmd = new SqlCommand("INSERT Employee (FirstName, LastName) VALUES (@FirstName, @LastName)", conn);

            EmployeeEntity employee = new EmployeeEntity();
            employee.FirstName = "john";
            employee.LastName = "doe";

            cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
            cmd.Parameters.AddWithValue("@LastName", employee.LastName);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

        }
    }

    public class EmployeeEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
