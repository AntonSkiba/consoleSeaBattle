using System;

namespace consoleSeaBattle {
	class Alphabet {
		public static void print (string text) {
			text = text.ToLower();
			(_, int top) = Console.GetCursorPosition();
			for (byte i = 0; i < text.Length; i++) {
				(int left, _) = Console.GetCursorPosition();
				string[] output = Alphabet.getSymbol(text[i]);

				for (byte j = 0; j < output.Length; j++) {
					Console.SetCursorPosition(left, top + j);
					Console.Write(output[j]);
				}
			}
		}

		public static string[] getSymbol(char inputSymbol) {
			string[] outputSymbol = new string[3];
			switch(inputSymbol) {
				case 'а':
					outputSymbol[0] = "╔═╗ ";
					outputSymbol[1] = "╠═╣ ";
					outputSymbol[2] = "║ ║ ";
					break;
				case 'б':
					outputSymbol[0] = "╔══ ";
					outputSymbol[1] = "╠═╗ ";
					outputSymbol[2] = "╚═╝ ";
					break;
				case 'в':
					outputSymbol[0] = "╦═╗ ";
					outputSymbol[1] = "╠═╣ ";
					outputSymbol[2] = "╩═╝ ";
					break;
				case 'г':
					outputSymbol[0] = "╔══ ";
					outputSymbol[1] = "║   ";
					outputSymbol[2] = "║   ";
					break;
				case 'д':
					outputSymbol[0] = "╔═╗ ";
					outputSymbol[1] = "║ ║ ";
					outputSymbol[2] = "╩═╩ ";
					break;
				case 'е':
					outputSymbol[0] = "╔══ ";
					outputSymbol[1] = "╠═  ";
					outputSymbol[2] = "╚══ ";
					break;
				case 'ж':
					outputSymbol[0] = "╗║╔ ";
					outputSymbol[1] = "╠╬╣ ";
					outputSymbol[2] = "╝║╚ ";
					break;
				case 'з':
					outputSymbol[0] = "══╗ ";
					outputSymbol[1] = "══╣ ";
					outputSymbol[2] = "══╝ ";
					break;
				case 'и':
					outputSymbol[0] = "║ ║ ";
					outputSymbol[1] = "║ ║ ";
					outputSymbol[2] = "╚═╝ ";
					break;
				case 'й':
					outputSymbol[0] = "║ ╚ ";
					outputSymbol[1] = "║ ║ ";
					outputSymbol[2] = "╚═╝ ";
					break;
				case 'к':
					outputSymbol[0] = "║ ╔ ";
					outputSymbol[1] = "╠═╣ ";
					outputSymbol[2] = "║ ╚ ";
					break;
				case 'л':
					outputSymbol[0] = "╔═╗ ";
					outputSymbol[1] = "║ ║ ";
					outputSymbol[2] = "╝ ║ ";
					break;
				case 'м':
					outputSymbol[0] = "╔╦╗ ";
					outputSymbol[1] = "║║║ ";
					outputSymbol[2] = "║║║ ";
					break;
				case 'н':
					outputSymbol[0] = "║ ║ ";
					outputSymbol[1] = "╠═╣ ";
					outputSymbol[2] = "║ ║ ";
					break;
				case 'о':
					outputSymbol[0] = "╔═╗ ";
					outputSymbol[1] = "║ ║ ";
					outputSymbol[2] = "╚═╝ ";
					break;
				case 'п':
					outputSymbol[0] = "╔═╗ ";
					outputSymbol[1] = "║ ║ ";
					outputSymbol[2] = "║ ║ ";
					break;
				case 'р':
					outputSymbol[0] = "╔═╗ ";
					outputSymbol[1] = "║ ║ ";
					outputSymbol[2] = "╠═╝ ";
					break;
				case 'с':
					outputSymbol[0] = "╔═╗ ";
					outputSymbol[1] = "║   ";
					outputSymbol[2] = "╚═╝ ";
					break;
				case 'т':
					outputSymbol[0] = "╔╦╗ ";
					outputSymbol[1] = " ║  ";
					outputSymbol[2] = " ║  ";
					break;
				case 'у':
					outputSymbol[0] = "║ ║ ";
					outputSymbol[1] = "╚═╣ ";
					outputSymbol[2] = "╚═╝ ";
					break;
				case 'ф':
					outputSymbol[0] = "╔╦╗ ";
					outputSymbol[1] = "║║║ ";
					outputSymbol[2] = "╚╬╝ ";
					break;
				case 'х':
					outputSymbol[0] = "╗ ╔ ";
					outputSymbol[1] = "╠═╣ ";
					outputSymbol[2] = "╝ ╚ ";
					break;
				case 'ц':
					outputSymbol[0] = "║ ║ ";
					outputSymbol[1] = "║ ║ ";
					outputSymbol[2] = "╚═╩ ";
					break;
				case 'ч':
					outputSymbol[0] = "║ ║ ";
					outputSymbol[1] = "║ ║ ";
					outputSymbol[2] = "╚═╣ ";
					break;
				case 'ш':
					outputSymbol[0] = "║║║ ";
					outputSymbol[1] = "║║║ ";
					outputSymbol[2] = "╚╩╝ ";
					break;
				case 'щ':
					outputSymbol[0] = "║║║ ";
					outputSymbol[1] = "║║║ ";
					outputSymbol[2] = "╚╩╩ ";
					break;
				case 'ъ':
					outputSymbol[0] = "╬═╗ ";
					outputSymbol[1] = "║ ║ ";
					outputSymbol[2] = "╚═╝ ";
					break;
				case 'ы':
					outputSymbol[0] = "║ ║ ";
					outputSymbol[1] = "╠═╗ ";
					outputSymbol[2] = "╚═╝ ";
					break;
				case 'ь':
					outputSymbol[0] = "╠═╗ ";
					outputSymbol[1] = "║ ║ ";
					outputSymbol[2] = "╚═╝ ";
					break;
				case 'э':
					outputSymbol[0] = "╔═╗ ";
					outputSymbol[1] = " ═╣ ";
					outputSymbol[2] = "╚═╝ ";
					break;
				case 'ю':
					outputSymbol[0] = "║╔╗ ";
					outputSymbol[1] = "╠╣║ ";
					outputSymbol[2] = "║╚╝ ";
					break;
				case 'я':
					outputSymbol[0] = "╔═╗ ";
					outputSymbol[1] = "╚═╣ ";
					outputSymbol[2] = "╔═╣ ";
					break;
				case ' ':
					outputSymbol[0] = "    ";
					outputSymbol[1] = "    ";
					outputSymbol[2] = "    ";
					break;
				default: break;
			}
			return outputSymbol;
		}
	}
}
