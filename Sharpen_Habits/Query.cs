using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using static Sharpen_Habits.Program;

namespace Sharpen_Habits
{
    class Query
    {
        static string connectionString = "Server=127.0.0.1;Database=sharpendb;Uid=root;Pwd=Jtm4sF@aMYSQ";
        public static async Task Init()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                        @"CREATE TABLE IF NOT EXISTS habits (
							Id INTEGER PRIMARY KEY AUTO_INCREMENT,
							Date TEXT,
							Quantity INTEGER
						)";
                    await command.ExecuteNonQueryAsync();

                    connection.Close();
                }
            }
        }
        public static async Task InsertHabit()
        {
            string date = GetDateInput();

            int quantity = GetNumberInput("Please insert the number of cups of water per day");


            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                        $"INSERT INTO habits(date, quantity) VALUES('{date}', {quantity})";

                    await command.ExecuteNonQueryAsync();

                    connection.Close();
                }
            }
        }
        public static async Task GetAllHabits() 
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                        $"SELECT * FROM habits";

                    await command.ExecuteNonQueryAsync();

                    List<Habit> tableData = new();

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows) 
                    { 
                        while (reader.Read())
                        {
                            tableData.Add(
                                new Habit
                                {
                                    Id = reader.GetInt32(0),
                                    Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                                    Quantity = reader.GetInt32(2),
                                });
                        }
                    } else 
                    {
                        Console.WriteLine("No rows found");
                    }

                    connection.Close();

                    Console.WriteLine("\n-------------------------\n");
                    foreach (var row in tableData)
                    {
                        Console.WriteLine($"{row.Id} - {row.Date.ToString("dd-MMM-yyyy")} - Quantity: {row.Quantity}");
                    }
                    Console.WriteLine("\n-------------------------\n");
                }
            }
        }
        public static async Task DeleteHabit() 
        {
            Console.Clear();
            await GetAllHabits();

            int recordId = GetNumberInput("Please enter the Id of the record you would like to delete, or 0 to return to the main menu");


            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand
                {
                    Connection= connection,
                    CommandText = $"DELETE from habits WHERE Id = '{recordId}'",
                };

                int rowCount = await command.ExecuteNonQueryAsync();

                if (rowCount == 0)
                {
                    Console.WriteLine($"Record with Id {recordId} doesn't exist");
                    DeleteHabit();
                }
            }
        }
        public static async Task UpdateHabit()
        {
            Console.Clear();
            await GetAllHabits();

            int updateId = GetNumberInput("Please enter the Id of the record you would like to update, or 0 to return to the main menu");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habits WHERE Id = {updateId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0) 
                {
                    Console.WriteLine($"Record with Id {updateId} doesn't exist");
                    connection.Close();
                    UpdateHabit();
                }
                string date = GetDateInput();

                int quantity = GetNumberInput("Please insert number of glasses (no decimals)");

                var command = connection.CreateCommand();
                command.CommandText = $"UPDATE habits SET date = '{date}', quantity = {quantity} WHERE Id = {updateId}";
                command.ExecuteNonQuery();

                connection.Close();
            }
        }
        internal static string GetDateInput()
        {
            Console.WriteLine("Please insert the date: (Format: dd-mm-yy). Type 0 to return to the main menu");

            string input = Console.ReadLine();

            if (input == "0") Sharpen_Habits.Program.GetUserInput();

            while(DateTime.TryParseExact(input, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _) == false)
            {
                Console.WriteLine("Invalid date of format dd-mm-yy. Type enter a valid date or type 0 to return to the main menu");
                input = Console.ReadLine(); 
            }

            return input;
        }
        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string rawInput = Console.ReadLine();

            if (rawInput == "0") GetUserInput();

            while ((Int32.TryParse(rawInput, out _) || Convert.ToInt32(rawInput) < 0) == false)
            {
                Console.WriteLine("Invalid input. Please enter a positive integer");
                rawInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(rawInput);

            return finalInput;
        }

    }

    public class Habit
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int  Quantity { get; set;}
    }
}

