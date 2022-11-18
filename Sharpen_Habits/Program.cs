using MySql.Data.MySqlClient;


namespace SharpenHabits
{
	class Program
	{
		static async Task Main(string[] args) 
		{
			string connectionString = "Server=127.0.0.1;Database=sharpendb;Uid=root;Pwd=Jtm4sF@aMYSQ";
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
        }

		static void GetUserInput()
		{
			Console.Clear();
			bool closeApp = false;
			while (closeApp == false)
			{

			}
		}
        
	}
}


