using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestMaryBot.UserBase;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Dapper;

namespace TestMaryBot
{
    public class DatabaseManager
    {
        private string connectionString = "Server=localhost;Port=3306;Database=testbot;Uid=root;Pwd=************;";
        public bool CheckRecordExists(long id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT COUNT(*) FROM users WHERE chatId = {id}";
                MySqlCommand command = new MySqlCommand(query, connection);

                int count = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();

                return count > 0;
            }
        }
        public bool CheckRecordExists(string username)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT COUNT(*) FROM users WHERE tgusername = '{username}'";
                MySqlCommand command = new MySqlCommand(query, connection);

                int count = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();

                return count > 0;
            }
        }
        public string CheckPresenceAdmin(string name, string username) //наличие админа с именем и юзернеймом
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT * FROM users WHERE username = '{name}' AND tgusername = '{username}'";
                MySqlCommand command = new MySqlCommand(query, connection);
                string output = " ";
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        output = $"Имя: {reader["username"]}" +
                    $"\nЮзернейм: {reader["tgusername"]}" +
                    $"\nДата рождения: {reader["dateBirth"]}" +
                    $"\nДата регистрации: {reader["dateregistration"]}";

                    }
                }
                connection.Close();
                return output;
            }
        }
        public string? GetStatusById(long id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT type FROM users WHERE chatId = {id};";
                MySqlCommand command = new MySqlCommand(query, connection);

                string? status = command.ExecuteScalar()?.ToString();

                connection.Close();

                return status;
            }
        }//проверка на админа по айди
        public string GetStatusByUsername(string username)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT type FROM users WHERE tgusername = '{username}';";
                MySqlCommand command = new MySqlCommand(query, connection);

                string status = command.ExecuteScalar().ToString();

                connection.Close();

                return status;
            }
        }//проверка на админа по юзернейму
        public void GetUserData(User user)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT username, tgusername, email, phoneNumber, dateBirth, gender FROM users WHERE chatId = {user.ChatId}";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    user.Name = reader["username"].ToString();
                    user.Username = reader["tgusername"].ToString();
                    user.Email = reader["email"].ToString();
                    user.Number = reader["phoneNumber"].ToString();
                    user.Gender = reader["gender"].ToString();
                    user.DateOfBirth = Convert.ToDateTime(reader["dateBirth"]);
                }

                connection.Close();
            }
        } //получение данных юзера и вбивание их в модель
        public List<string> GetAllTypesProducts()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                connection.Open();
                var categories = connection.Query<string>("SELECT DISTINCT title FROM types_products;");
                connection.Close();
                return categories.ToList();

            }
        }//получение всех категорий
        public int GetNumberCategoryProduct(string name)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                connection.Open();
                string query = $"SELECT number FROM types_products WHERE title = '{name}';";
                MySqlCommand command = new MySqlCommand(query, connection);
                int num = Convert.ToInt16( command.ExecuteScalar());
                connection.Close();
                return num;

            }
        }//получение номера категории
        public void UpdateData(string field, string value, long chatId, string username)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $"";
                connection.Open();
                if (chatId == 0)
                {
                    if (field == "Имя")
                        query = $"UPDATE users SET username = '{value}' WHERE tgusername = '{username}';";
                    else if (field == "Фамилия")
                        query = $"UPDATE users SET secondusername = '{value}' WHERE tgusername = '{username}';";
                    else if (field == "Дата рождения")
                        query = $"UPDATE users SET dateBirth = '{value}' WHERE tgusername = '{username}';";

                    else if (field == "Пол")
                        query = $"UPDATE users SET gender = '{value}' WHERE tgusername = '{username}';";

                    else if (field == "Страна")
                        query = $"UPDATE users SET country = '{value}' WHERE tgusername = '{username}';";

                    else if (field == "Номер телефона")
                        query = $"UPDATE users SET phoneNumber = '{value}' WHERE tgusername = '{username}';";

                    else if (field == "Электропочта")
                        query = $"UPDATE users SET email = '{value}' WHERE tgusername = '{username}';";
                    else if (field == "id")
                        query = $"UPDATE users SET chatId = {value} WHERE tgusername = '{username}';";

                }
                else
                {
                    if (field == "Имя")
                        query = $"UPDATE users SET username = '{value}' WHERE chatId = {chatId};";
                    else if (field == "Фамилия")
                        query = $"UPDATE users SET secondusername = '{value}' WHERE chatId = {chatId};";
                    else if (field == "Дата рождения")
                        query = $"UPDATE users SET dateBirth = '{value}' WHERE chatId = {chatId};";

                    else if (field == "Пол")
                        query = $"UPDATE users SET gender = '{value}' WHERE chatId = {chatId};";

                    else if (field == "Страна")
                        query = $"UPDATE users SET country = '{value}' WHERE chatId = {chatId};";

                    else if (field == "Номер телефона")
                        query = $"UPDATE users SET phoneNumber = '{value}' WHERE chatId = {chatId};";

                    else if (field == "Электропочта")
                        query = $"UPDATE users SET email = '{value}' WHERE chatId = {chatId};";
                    else if (field == "Юзернейм") //здесь кинь предупреждение
                        query = $"UPDATE users SET username = '{value}' WHERE chatId = {chatId};";
                }
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void AddNewAdmin(string name, string date, string username, string country, string email, string number, string date2)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"INSERT INTO users (username, dateBirth, tgusername, country, email, phoneNumber, type, dateregistration) VALUES ('{name}', '{date}', '{username}', '{country}', '{email}','{number}', 'админ', '{date2}')";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }
        public void AddNewRegularUser(long chatId, string name, string secondName, string username, string email, string number, string date, string gender, string date2, string country)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"INSERT INTO users (chatId, username, tgusername, secondusername, email, phoneNumber, dateBirth, gender, type, dateregistration, country) VALUES ({chatId},'{name}','{username}','{secondName}', '{email}','{number}','{date}','{gender}', 'обычный', '{date2}', '{country}')";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }
        public void AddNewProduct(int type, string name, int price_r, int price_e, string desc, int amount, string hashtags)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"INSERT INTO products (type, title, price_r, price_e, description_prod, amount, hashtags) VALUES ('{type}', '{name}', {price_r}, {price_e}, '{desc}', {amount}, '{hashtags}')";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

    }
}
