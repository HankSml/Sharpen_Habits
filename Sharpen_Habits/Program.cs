using static Sharpen_Habits.Query;


namespace Sharpen_Habits
{
	class Program
	{
		static async void Main(string[] args)
		{
			await Sharpen_Habits.Query.Init();

			await GetUserInput();
		}

		public static async Task GetUserInput()
		{
			Console.Clear();
			bool closeApp = false;
			while (closeApp == false)
			{
                Console.WriteLine("Welcome to Sharpen Habits!");
                Console.WriteLine("---------------------------\n");
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
					case "1":
						await Sharpen_Habits.Query.GetAllHabits();
						break;
					case "2":
						await Sharpen_Habits.Query.InsertHabit();
						break;
					case "3":
						await Sharpen_Habits.Query.DeleteHabit();
						break;
					case "4":
						await Sharpen_Habits.Query.UpdateHabit();
						break;
					default:
						break;
				}
            }
		}
	}
}


