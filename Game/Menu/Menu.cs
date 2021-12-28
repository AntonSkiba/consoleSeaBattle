using System;
using static consoleSeaBattle.Game;

namespace consoleSeaBattle {
	class Menu {
		private byte selected = 0;
		private List<MenuItem> items;
		private string header;
		private string footer;

		private Action callback = null;

		public Menu(string header, List<MenuItem> items) {
			this.items = items;
			this.header = header;
			this.footer = "";
		}

		public Menu(string header) {
			this.header = header;
			this.items = new List<MenuItem>{};
			this.footer = "";
		}

		public void Add(MenuItem item) {
			this.items.Add(item);
		}

		private void next() {
			this.selected++;
			this.selected %= Convert.ToByte(this.items.Count);

			this.print();
		}

		public void setFooter(string footer) {
			this.footer = footer;
			this.print();
		}

		private void prev() {
			this.selected += Convert.ToByte(this.items.Count);
			this.selected--;
			this.selected %= Convert.ToByte(this.items.Count);

			this.print();
		}

		public string select() {
			bool isSelected = false;
			do {
				ConsoleKeyInfo key = Console.ReadKey(true);
				switch(key.Key) {
					case ConsoleKey.W:
						this.prev();
						break;
					case ConsoleKey.S:
						this.next();
						break;
					default: break;
				}

				if (key.Key == ConsoleKey.Enter) {
					isSelected = true;
				}

			} while (!isSelected);

			return this.items[this.selected].key;
		}

		public void print(Action callback = null) {
			if (this.items.Count > 1) Render.Clear();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(header + "\n");
			Console.ResetColor();
			
			for (byte i = 0; i < this.items.Count; i++) {
				if (i == this.selected) {
					Console.BackgroundColor = ConsoleColor.Blue;
					Console.WriteLine("> " + this.items[i].title);
					Console.ResetColor();
				} else {
					Console.WriteLine("  " + this.items[i].title);
				}
			}

			if (callback != null) {
				this.callback = callback;	
			}

			if (this.callback != null) {
				this.callback();
			}
		}

	}
}
