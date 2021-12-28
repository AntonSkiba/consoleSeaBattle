using System;

namespace consoleSeaBattle {
	class Render {
		public static void RenderTitle(string text, ConsoleColor color) {
			Console.ForegroundColor = color;
			Console.Clear();
			Console.Write("\n\n");
			Alphabet.print(text);
			Console.Write("\n\n");
			Console.ResetColor();
		}

		public static void RenderWin() {
			Render.RenderTitle("    Победа", ConsoleColor.Green);
			Render.Clear();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("         │██                              │██ \n");
			Console.Write("         │        ▄▄   ▄▄   ▄▄   ▄▄       │   \n");
			Console.Write("         ▌        ▒▒   ▒▒   ▒▒   ▒▒       ▌   \n");
			Console.Write("         ▌      ▄▀█▀█▀█▀█▀█▀█▀█▀█▀█▀▄     ▌   \n");
			Console.Write("         ▌    ▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄   ▌   \n");
			Console.Write("      ▀██████████████████████████████████████▄\n");
			Console.Write("        ▀███████████████████████████████████▀ \n");
			Console.Write("           ▀██████████████████████████████▀   \n");
			Console.Write("      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒\n");
			Console.Write("      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒\n");
			Console.Write("      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒\n");
			Console.ResetColor();
		}

		public static void RenderLose() {
			Render.RenderTitle("  Поражение", ConsoleColor.Red);
			Render.Clear();
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("         ▄▄                              ▄▄ \n");
			Console.Write("        ████                            ████\n");
			Console.Write("        ▀███▄          ▄▄▄▄▄▄          ▄███▀\n");
			Console.Write("           ▀██▄██▀ ▄██▀▀▀▀▀▀▀▀██▄ ▀██▄██▀   \n");
			Console.Write("            ███████▀▄▄▄▄▄██▄▄▄▄▄▀███████    \n");
			Console.Write("            █▀████████████████████████▀█    \n");
			Console.Write("               ▀████████████████████▀       \n");
			Console.Write("                ██▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀███       \n");
			Console.Write("                ▀█░░▄▄██▄░░▄██▄▄░░███       \n");
			Console.Write("                ▀█░░█████░░█████░░████      \n");
			Console.Write("                 █▄░░▀▀▀░▄▄░▀▀▀░░▄████      \n");
			Console.Write("                  ██▄▄░░████░░▄▄██████      \n");
			Console.Write("                 ▄████░░████░░████████      \n");
			Console.Write("               ▄████▀█░░░░░░░░█▀█████       \n");
			Console.Write("            ▄█████▀█▄▀▀▀█▀▀█▀▀▀▄███████▄    \n");
			Console.Write("          ▀▀▀▀▀     █▄░░▀░░▀░░▄█▀███ ▀▀▀▀▀  \n");
			Console.Write("                     ▀████████▀   ▀▀        \n");
			Console.ResetColor();
		}

		public static void Clear() {
			Console.SetCursorPosition(0, 7);
			for (byte i = 0; i < Console.WindowHeight - 7; i++) {
				Console.Write(new string(' ', Console.WindowWidth)); 
			}
			Console.SetCursorPosition(0, 7);

		}

	}
}
