using System;

namespace consoleSeaBattle {
	
	class Program {

		public static void Main() {
			Console.Clear();
			Console.SetWindowSize(70, 30);

			Console.CursorVisible = false;
			
			Game game = new Game();
			Task start = game.start();
			start.Wait();
		}

	}

}
