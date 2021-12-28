using System;
using static consoleSeaBattle.Menu;

namespace consoleSeaBattle {
	class MenuItem {
		public string key;
		public string title;

		public MenuItem(string key, string title) {
			this.key = key;
			this.title = title;
		}
	}
}
