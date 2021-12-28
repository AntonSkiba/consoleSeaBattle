using System;

namespace consoleSeaBattle {
	
	class Program {

		public static void Main() {
			Game game = new Game();
			Task start = game.start();
			start.Wait();
		}

	}

}
