using System;

namespace consoleSeaBattle {
	
	class Program {

		public static void Main() {
			Console.Clear();
			Console.SetWindowSize(80, 40);
			Console.SetBufferSize(80, 40);

			Console.CursorVisible = false;
			
			Game game = new Game();
			Task start = game.start();
			start.Wait();
		}

	}

}
