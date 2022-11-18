using MySql.Data.MySqlClient;


namespace SharpenHabits
{
	class Program
	{
		static string connectionString = "Server=127.0.0.1;Database=sharpendb;Uid=root;Pwd=Jtm4sF@aMYSQ";
        static async Task Main(string[] args) 
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

            Console.WriteLine("Welcome to Sharpen Habits!");
            Console.WriteLine("---------------------------");
			GetUserInput();
        }

		static void GetUserInput()
		{
			Console.Clear();
			bool closeApp = false;
			while (closeApp == false)
			{
				Console.WriteLine("Menu:");
                Console.WriteLine("Please select an option to begin");
                Console.WriteLine("Press 0 to close the application");
                Console.WriteLine("Press 1 to view all habits");
                Console.WriteLine("Press 2 to insert a habit");
                Console.WriteLine("Press 3 to delete a habit");
                Console.WriteLine("Press 4 to update a habit");
                Console.WriteLine("--------------------------------------\n");

				string selectedOption = Console.ReadLine();

				switch (selectedOption)
				{
					case "0":
						Console.WriteLine("Goodbye!");
						closeApp = true;
						break;
					//case 1:
					//	GetAllHabits();
					//	break;
					case "2":
						InsertHabit();
						break;
					//case 3:
					//	DeleteHabit();
					//	break;
					//case 4:
					//	UpdateHabit();
					//	break;
					//default:
					//	break;
				}
			}
		}
		private static async void InsertHabit()
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
		internal static string GetDateInput()
		{
			Console.WriteLine("Please insert the date: (Format: dd-mm-yy). Type 0 to return to the main menu");

			string input = Console.ReadLine();

			if (input == "0") GetUserInput();

			return input;
		}
		internal static int GetNumberInput(string message)
		{
			Console.WriteLine(message);

			string rawInput = Console.ReadLine();

			if (rawInput == "0") GetUserInput();

			int finalInput = Convert.ToInt32(rawInput);

			return finalInput;
		}
        
	}
}


