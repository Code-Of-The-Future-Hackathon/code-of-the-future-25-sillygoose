using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities.Net;
using System.Net;
using System.Net.Mail;

namespace ConsoleApp6
{
    internal class Program
    {
        public static bool Query(string key, float value, string query, MySqlConnection conn, string log_measure)
        {
            string updated_query = query.Replace("@key", key);

            using MySqlCommand cmd = new MySqlCommand(updated_query, conn);
            cmd.Parameters.AddWithValue("@RAM_Used_Percent", value);

            using MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine($"{key} Exceeded {value:0.00}{log_measure}");
                return true;
            }
            return false;
        }
        static void Main(string[] args)
        {
            float w_ram_percent_used = 80;
            float w_cpu_usage = 30;
            float w_storage_percent_used = 80;
            float w_wifi_up_usage = 50;
            float w_wifi_down_usage = 50;
            float w_ethernet_up_usage = 50;
            float w_ethernet_down_usage = 50;

            DateTime next_ram_warning = DateTime.Now;
            DateTime next_cpu_usage_warning = DateTime.Now;
            DateTime next_storage_warning = DateTime.Now;
            DateTime next_wifi_warning = DateTime.Now;
            DateTime next_ethernet_warning = DateTime.Now;

            int minutes_cd_ram_warning = 5;
            int minutes_cd_cpu_usage_warning = 5;
            int minutes_cd_storage_warning = 5;
            int minutes_cd_wifi_warning = 5;
            int minutes_cd_ethernet_warning = 5;

            string fromAddress = "simwarnings@gmail.com";
            string toAddress = "warningsreceiver@gmail.com";
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string username = "simwarnings@gmail.com";
            string password = "";
            

            string connectionString = "server=localhost;user=root;password=<pass>;database=Sensors";

            using MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "SELECT Time_Of_Reading, @key FROM SensorReadings " +
                "WHERE @key >= @RAM_Used_Percent AND TIMESTAMPDIFF(SECOND, Time_Of_Reading, NOW()) <= 3";

            string log_query = "INSERT INTO WarningLogs(Time_Of_Logging, Warning_Type, Warning_Message) " +
                "VALUES (@time, @type, @message)";

            while(true)
            {
                if(Query("RAM_Used_Percent", w_ram_percent_used, query, conn, "%"))
                {
                    if(next_ram_warning<=DateTime.Now)
                    {
                        using MySqlCommand cmd = new MySqlCommand(log_query, conn);
                        cmd.Parameters.AddWithValue("@time", DateTime.Now);
                        cmd.Parameters.AddWithValue("@type", "RAM Usage");
                        cmd.Parameters.AddWithValue("@message", $"RAM usage exceeds the specified {w_ram_percent_used}%");

                        cmd.ExecuteNonQuery();

                        try
                        {
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress(fromAddress);
                            mail.To.Add(toAddress);
                            mail.Subject = $"RAM usage exceeds the specified {w_ram_percent_used}%";
                            mail.Body = mail.Subject;

                            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                            smtpClient.Credentials = new NetworkCredential(username, password);
                            smtpClient.EnableSsl = true;

                            smtpClient.Send(mail);
                            //Console.WriteLine("Email sent successfully!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error sending email: {ex.Message}");
                        }

                        next_ram_warning = DateTime.Now.AddMinutes(minutes_cd_ram_warning);
                    }
                }
                if(Query("CPU_Usage", w_cpu_usage, query, conn, "%"))
                {
                    if (next_cpu_usage_warning <= DateTime.Now)
                    {
                        using MySqlCommand cmd = new MySqlCommand(log_query, conn);
                        cmd.Parameters.AddWithValue("@time", DateTime.Now);
                        cmd.Parameters.AddWithValue("@type", "CPU Usage");
                        cmd.Parameters.AddWithValue("@message", $"CPU usage exceeds the specified {w_cpu_usage}%");

                        cmd.ExecuteNonQuery();

                        try
                        {
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress(fromAddress);
                            mail.To.Add(toAddress);
                            mail.Subject = $"CPU usage exceeds the specified {w_cpu_usage}%";
                            mail.Body = mail.Subject;

                            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                            smtpClient.Credentials = new NetworkCredential(username, password);
                            smtpClient.EnableSsl = true;

                            smtpClient.Send(mail);
                            //Console.WriteLine("Email sent successfully!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error sending email: {ex.Message}");
                        }

                        next_cpu_usage_warning = DateTime.Now.AddMinutes(minutes_cd_cpu_usage_warning);
                    }
                }
                if(Query("Storage_Used_Percent", w_storage_percent_used, query, conn, "%"))
                {
                    if (next_storage_warning <= DateTime.Now)
                    {
                        using MySqlCommand cmd = new MySqlCommand(log_query, conn);
                        cmd.Parameters.AddWithValue("@time", DateTime.Now);
                        cmd.Parameters.AddWithValue("@type", "Storage Low");
                        cmd.Parameters.AddWithValue("@message", $"Storage used exceeds the specified {w_storage_percent_used}%");

                        cmd.ExecuteNonQuery();

                        try
                        {
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress(fromAddress);
                            mail.To.Add(toAddress);
                            mail.Subject = $"Storage usage exceeds the specified {w_storage_percent_used}%";
                            mail.Body = mail.Subject;

                            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                            smtpClient.Credentials = new NetworkCredential(username, password);
                            smtpClient.EnableSsl = true;

                            smtpClient.Send(mail);
                            //Console.WriteLine("Email sent successfully!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error sending email: {ex.Message}");
                        }

                        next_storage_warning = DateTime.Now.AddMinutes(minutes_cd_storage_warning);
                    }
                }
                if(Query("Wifi_Up", w_wifi_up_usage, query, conn, ""))
                {
                    if (next_wifi_warning <= DateTime.Now)
                    {
                        using MySqlCommand cmd = new MySqlCommand(log_query, conn);
                        cmd.Parameters.AddWithValue("@time", DateTime.Now);
                        cmd.Parameters.AddWithValue("@type", "Wifi Usage");
                        cmd.Parameters.AddWithValue("@message", $"Wifi usage exceeds the specified {w_wifi_up_usage}");

                        cmd.ExecuteNonQuery();

                        try
                        {
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress(fromAddress);
                            mail.To.Add(toAddress);
                            mail.Subject = $"RAM usage exceeds the specified {w_wifi_up_usage}%";
                            mail.Body = mail.Subject;

                            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                            smtpClient.Credentials = new NetworkCredential(username, password);
                            smtpClient.EnableSsl = true;

                            smtpClient.Send(mail);
                            //Console.WriteLine("Email sent successfully!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error sending email: {ex.Message}");
                        }

                        next_wifi_warning = DateTime.Now.AddMinutes(minutes_cd_wifi_warning);
                    }
                }
                if(Query("Wifi_Down", w_wifi_down_usage, query, conn, ""))
                {
                    if (next_wifi_warning <= DateTime.Now)
                    {
                        using MySqlCommand cmd = new MySqlCommand(log_query, conn);
                        cmd.Parameters.AddWithValue("@time", DateTime.Now);
                        cmd.Parameters.AddWithValue("@type", "Wifi Usage");
                        cmd.Parameters.AddWithValue("@message", $"Wifi usage exceeds the specified {w_wifi_down_usage}");

                        cmd.ExecuteNonQuery();

                        try
                        {
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress(fromAddress);
                            mail.To.Add(toAddress);
                            mail.Subject = $"RAM usage exceeds the specified {w_wifi_down_usage}%";
                            mail.Body = mail.Subject;

                            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                            smtpClient.Credentials = new NetworkCredential(username, password);
                            smtpClient.EnableSsl = true;

                            smtpClient.Send(mail);
                            //Console.WriteLine("Email sent successfully!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error sending email: {ex.Message}");
                        }

                        next_wifi_warning = DateTime.Now.AddMinutes(minutes_cd_wifi_warning);
                    }
                }
                if(Query("Ethernet_Up", w_ethernet_up_usage, query, conn, ""))
                {
                    if (next_ethernet_warning <= DateTime.Now)
                    {
                        using MySqlCommand cmd = new MySqlCommand(log_query, conn);
                        cmd.Parameters.AddWithValue("@time", DateTime.Now);
                        cmd.Parameters.AddWithValue("@type", "Ethernet Usage");
                        cmd.Parameters.AddWithValue("@message", $"Ethernet usage exceeds the specified {w_ethernet_up_usage}");

                        cmd.ExecuteNonQuery();

                        try
                        {
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress(fromAddress);
                            mail.To.Add(toAddress);
                            mail.Subject = $"RAM usage exceeds the specified {w_ethernet_up_usage}%";
                            mail.Body = mail.Subject;

                            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                            smtpClient.Credentials = new NetworkCredential(username, password);
                            smtpClient.EnableSsl = true;

                            smtpClient.Send(mail);
                            //Console.WriteLine("Email sent successfully!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error sending email: {ex.Message}");
                        }

                        next_ethernet_warning = DateTime.Now.AddMinutes(minutes_cd_ethernet_warning);
                    }
                }
                if(Query("Ethernet_Down", w_ethernet_down_usage, query, conn, ""))
                {
                    if (next_ethernet_warning <= DateTime.Now)
                    {
                        using MySqlCommand cmd = new MySqlCommand(log_query, conn);
                        cmd.Parameters.AddWithValue("@time", DateTime.Now);
                        cmd.Parameters.AddWithValue("@type", "Ethernet Usage");
                        cmd.Parameters.AddWithValue("@message", $"Ethernet usage exceeds the specified {w_ethernet_down_usage}");

                        cmd.ExecuteNonQuery();

                        try
                        {
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress(fromAddress);
                            mail.To.Add(toAddress);
                            mail.Subject = $"RAM usage exceeds the specified {w_ethernet_up_usage}%";
                            mail.Body = mail.Subject;

                            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                            smtpClient.Credentials = new NetworkCredential(username, password);
                            smtpClient.EnableSsl = true;

                            smtpClient.Send(mail);
                            //Console.WriteLine("Email sent successfully!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error sending email: {ex.Message}");
                        }

                        next_ethernet_warning = DateTime.Now.AddMinutes(minutes_cd_ethernet_warning);
                    }
                }

                Console.WriteLine();

                Thread.Sleep(1000);
            }

            conn.Close();
        }
    }
}
