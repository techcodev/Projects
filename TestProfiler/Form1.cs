using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace TestProfiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTest1_Click(object sender, EventArgs e)
        {
            HoldingTask(1000);
            HoldingTask(2000);
            HoldingTask(5000);
        }

        

        private void btnTest2_Click(object sender, EventArgs e)
        {
            GetSomeResponseHtml("http://www.google.com");
            GetSomeResponseHtml("http://www.yahoo.com");
            GetSomeResponseHtml("http://www.nodus.com");

            GetSomeResponseHtml("http://www.outlook.com");
        }

        private void btnTest3_Click(object sender, EventArgs e)
        {
            var datatableResult = SomeDatabaseAction();
            var line = string.Empty;
            for (int i = 0; i < datatableResult.Rows.Count; i++)
            {
                var row = datatableResult.Rows[i];
                StringBuilder sb = new StringBuilder();
                sb.Append(row[0]);
                for (int j = 1; j < row.ItemArray.Length; j++)
                {
                    sb.Append("," + row[j]);
                }
                sb.Append("\r\n");
                line += sb.ToString();
            }
            LogToFile(line);
        }

        private void HoldingTask(int millisecond)
        {
            System.Threading.Thread.Sleep(millisecond);
        }

        private string GetSomeResponseHtml(string url)
        {
            var htmlSource = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                htmlSource = sr.ReadToEnd();
            }
            return htmlSource;
        }

        private DataTable SomeDatabaseAction()
        {
            string connectionString = "server=chocolate;database=SchoolEnrollment;uid=sa;password=goldfish";            
            DataTable result = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = conn;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetAllApplicationConfiguration";
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        result.Load(dr);
                    }
                }
                conn.Close();
            }
            return result;
        }

        private void LogToFile(string message)
        {
            var filename = @"c:\temp\test.log";
            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                lock (sw)
                {
                    try
                    {
                        string timeStamp = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                        string logMessage = timeStamp + ":\t" + message;
                        sw.WriteLine(logMessage);
                        sw.Flush();
                    }
                    finally
                    {
                        sw.Close();
                    }
                }
            }
        }

    }
}
