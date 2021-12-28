using System;
using System.Timers;

namespace consoleSeaBattle {
	class Field {
		private byte size = 10;
		private Menu menu;

		private List<Ship>[] ships;
		private List<Ship> placedShips;
		private List<Ship> explodedShips;

		// точки, в которые уже стреляли
		private List<Point> checkCells;

		private Ship aim;
		private string status = "";

		private string statusPoint = "";

		private string abc = "abcdefghij";

		private string[] cells;
		private Random random;
		private string name;
		private bool hide;

		public Field(string name, bool hide) {
			this.createShips();
			this.random = new Random();
			this.name = name;
			this.hide = hide;
			this.checkCells = new List<Point>();
		}

		public string setShips() {
			bool goBack = false;
			bool isDone = false;
			bool isAuto = false;

			do {
				this.menu = new Menu("Выберите тип кораблика для размещения: ");

				for (byte i = 0; i < ships.Length; i++) {
					if (ships[i].Count > 0) {
						string title = "";
						for (byte j = 0; j < ships[i][0].size; j++) {
							title += "#";
						}
						title += " - " + ships[i].Count + "x";
						MenuItem item = new MenuItem("" + (ships[i][0].size - 1), title);
						this.menu.Add(item);
					}
				}

				this.menu.Add(new MenuItem("auto", "Расставить автоматически"));
				this.menu.Add(new MenuItem("back", "Назад"));

				
				this.menu.print(this.modernPrint);

				string menuKey = this.menu.select();
				if (menuKey == "back") {
					goBack = true;
				} else if (menuKey == "auto") {
					isAuto = true;
				} else {
					Ship selectedShip = this.ships[Convert.ToInt32(menuKey)][0];
					this.placedShips.Add(selectedShip);
					this.setShip(selectedShip);
					this.ships[Convert.ToInt32(menuKey)].RemoveAt(0);
				}


				isDone = Array.TrueForAll(this.ships, list => list.Count == 0);
			} while(!goBack && !isDone && !isAuto);

			if (isDone) return "done";
			if (isAuto) return "auto";
			else return "back";
		}

		public async Task autoSet(int interval, bool show, string loaderText) {
			// составляем рандомную последовательность выбора типов
			byte[] sequence = new byte[] {0, 0, 0, 0, 1, 1, 1, 2, 2, 3};
			for (byte i = Convert.ToByte(sequence.Length - 1); i >= 1; i--) {
				byte j = Convert.ToByte(this.random.Next(i + 1));
				byte temp = sequence[j];
				sequence[j] = sequence[i];
				sequence[i] = temp;
				
			}
			
			foreach (byte el in sequence) {
				if (this.ships[el].Count > 0) {
					Ship selectedShip = this.ships[el][0];
					this.placedShips.Add(selectedShip);
					await setAutoShip(selectedShip, interval, show, loaderText);
					this.ships[el].RemoveAt(0);
				}
			}
		}

		private async Task setAutoShip(Ship ship, int interval, bool show, string loaderText) {
			PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromMilliseconds(interval));
			ConsoleKey[] keys = new ConsoleKey[6] {
				ConsoleKey.W,
				ConsoleKey.A,
				ConsoleKey.S,
				ConsoleKey.D,
				ConsoleKey.E,
				ConsoleKey.Enter
			};
			string loader = @"||||////----\\\\";
			byte loadCount = 0;
			bool isDone = false;
			Point start = new Point(5, 5);
			while (!isDone) {
				await timer.WaitForNextTickAsync();
				ship.setPoints(start);

				Render.Clear();

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write(" " + loader[loadCount] + "  " + loaderText + "\n");
				loadCount++;
				loadCount %= Convert.ToByte(loader.Length);
				Console.ResetColor();

				if (show) {
					this.modernPrint();
				}

				ConsoleKey randomControl = keys[this.random.Next(keys.Length)];
				isDone = controlClick(ship, start, randomControl);
				this.statusPoint = "";
			}

		}

		private bool hasShips() {
			return this.placedShips.Count > 0;
		}

		public bool controlClick(Ship ship, Point start, ConsoleKey key) {
			bool isDone = false;
			switch(key) {
				case ConsoleKey.W:
					if (start.Y > 0) {
						start.Y--;
					}
					this.status = "";
					break;
				case ConsoleKey.A:
					if (start.X > 0) {
						start.X--;
					}
					this.status = "";
					break;
				case ConsoleKey.S:
					if ((ship.isVertical && start.Y < this.size - ship.size)
						|| (!ship.isVertical && start.Y < this.size - 1)) {
						start.Y++;
					}
					this.status = "";
					break;
				case ConsoleKey.D:
					if ((!ship.isVertical && start.X < this.size - ship.size)
							|| (ship.isVertical && start.X < this.size - 1)) {
						start.X++;
					}
					this.status = "";
					break;
				case ConsoleKey.E:
					if (ship.isVertical && start.X > this.size - ship.size) {
						start.X = Convert.ToByte(this.size - ship.size);
					}
					if (!ship.isVertical && start.Y > this.size - ship.size) {
						start.Y = Convert.ToByte(this.size - ship.size);
					}
					ship.isVertical = !ship.isVertical;
					this.status = "";
					break;
				case ConsoleKey.Enter:
					isDone = true;
					bool isCheck = this.isCheckCell(start.Y, start.X);
					if (!isCheck) {
						for (byte i = 0; i < ship.getPoints().Length; i++) {
							if (Ship.getType(this.placedShips, ship.getPoints()[i]) == "conflict") {
								isDone = false;
								break;
							}
						}
					} else isDone = false;
					
					break;
				default: break;
			}

			this.statusPoint = start.X + " - " + this.abc[start.Y];
			return isDone;
		}

		private void setShip(Ship ship) {
			// устанавливаем начальную позицию
			Point start = new Point(5, 5);
			
			bool done = false;
			do {
				ship.setPoints(start);
				
				Render.Clear();

				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write("W, A, S, D - управление, ");
				Console.ResetColor();
				
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write("E - перевернуть, ");
				Console.ResetColor();

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Enter - поставить");
				Console.ResetColor();

				this.modernPrint();
				ConsoleKeyInfo key = Console.ReadKey(true);
				done = this.controlClick(ship, start, key.Key);
			} while (!done);
			
		}


		public void modernPrint() {
			// сохраняем начальное положение каретки
			(int left, int top) = Console.GetCursorPosition();
			// сразу задаем отступ
			left += 1;
			top += 1;

			// Рисуем заголовок
			Console.SetCursorPosition(left + 4, top);
			Console.Write(this.name + ":");

			// Рисуем цифры
			Console.SetCursorPosition(left + 4, top + 1);
			Console.Write("0 1 2 3 4 5 6 7 8 9");

			// Рисуем верхнюю границу
			Console.SetCursorPosition(left + 2, top + 2);
			Console.BackgroundColor = ConsoleColor.White;
			Console.Write("                        ");
			Console.ResetColor();

			// рисуем поле
			for (byte i = 0; i < this.size; i++) {
				Console.SetCursorPosition(left, top + 3 + i);
				this.printFieldLine(i);
			}

			// Рисуем нижнюю границу
			(_, top) = Console.GetCursorPosition();
			Console.SetCursorPosition(left + 2, top + 1);
			Console.BackgroundColor = ConsoleColor.White;
			Console.Write("                        ");
			Console.ResetColor();

			Console.SetCursorPosition(left + 2, top + 2);

			if (this.status == "exp") {
				Console.ForegroundColor = ConsoleColor.Magenta;
				Console.WriteLine(this.statusPoint + " Уничтожен!");
				Console.ResetColor();
			} else if (this.status == "hit") {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(this.statusPoint + " Попадание!");
				Console.ResetColor();
			} else if (this.status == "miss") {
				Console.ForegroundColor = ConsoleColor.DarkBlue;
				Console.WriteLine(this.statusPoint + " Мимо!");
				Console.ResetColor();
			} else {
				Console.WriteLine(this.statusPoint);
			}

			Console.SetCursorPosition(left, top + 2);
		}

		public string hit() {
			this.status = "miss";
			if (Ship.getType(this.placedShips, this.aim.getPoints()[0]) == "placed") {
				this.status = "hit";
				Ship explodedShip = Ship.hit(this.placedShips, this.aim.getPoints()[0]);
				if (explodedShip != null) {
					this.status = "exp";
					this.placedShips.Remove(explodedShip);
					this.explodedShips.Add(explodedShip);
					// если кораблик уничтожен помечаем точки вокруг него
					this.setCells(explodedShip);

					if (!this.hasShips()) {
						this.status = "gameover";
					}
				}
			}
			this.checkCells.Add(this.aim.getPoints()[0]);
			return this.status;
		}

		private void setCells(Ship ship) {
			Point[] around = ship.getAround();
			for (byte i = 0; i < around.Length; i++) {
				this.checkCells.Add(around[i]);
			}
		}

		public void setAim(Ship aim) {
			this.aim = aim;
		}

		public void removeAim() {
			this.aim = null;
		}

		public void printFieldLine(byte i) {
			Console.Write(this.abc[i] + " ");

			Console.BackgroundColor = ConsoleColor.White;
			Console.Write("  ");
			Console.ResetColor();

			// составляем карту под отдельную линиюю области
			// возможные типы:
			// Blue - пустая клетка
			// Black - корабль (а также прицел в закрытом поле)
			// Red - попадание (а также прицел в открытом поле)
			// Magenta - уничтоженный корабль
			// DarkGray - прицел
			ConsoleColor[] colorMap = new ConsoleColor[10];

			// Заполняем поле водой
			for (byte j = 0; j < this.size; j++) {
				colorMap[j] = ConsoleColor.Blue;
			}

			// Для начала собираем подбитые кораблики
			for (byte j = 0; j < this.size; j++) {
				Point pos = new Point(j, i);
				bool isExplodedAround = Ship.isAround(this.explodedShips, pos);
				string explodedShip = Ship.getType(this.explodedShips, pos);

				if (explodedShip == "exploded") colorMap[j] = ConsoleColor.Magenta;
				else if (isExplodedAround) colorMap[j] = ConsoleColor.Blue; 
			}

			// Далее собираем обычные кораблики с учетом видимости
			for (byte j = 0; j < this.size; j++) {
				Point pos = new Point(j, i);
				bool isAround = Ship.isAround(this.placedShips, pos);
				string shipType = Ship.getType(this.placedShips, pos);

				// Подбитая часть или конфликтующая видна всегда
				if (shipType == "exploded" || shipType == "conflict") colorMap[j] = ConsoleColor.Red;
				else if (shipType == "placed" && !this.hide) colorMap[j] = ConsoleColor.Black;
				else if (isAround && !this.hide) colorMap[j] = ConsoleColor.Blue; 
			}

			// Далее ставим прицел, если он есть
			if (this.aim != null) {
				for (byte j = 0; j < this.size; j++) {
					Point pos = new Point(j, i);
					bool isAimAround = Ship.isAround(new List<Ship> {this.aim}, pos);
					string aimType = Ship.getType(new List<Ship> {this.aim}, pos);
					// ключевая точка это подбитый/установленный/уничтоженный корабль
					bool isKeyPoint = colorMap[j] == ConsoleColor.Black || colorMap[j] == ConsoleColor.Magenta || colorMap[j] == ConsoleColor.Red;

					if (aimType == "placed" && !this.hide) colorMap[j] = ConsoleColor.Red;
					else if (aimType == "placed" && this.hide) colorMap[j] = ConsoleColor.Black;
					else if (isAimAround && !isKeyPoint) colorMap[j] = ConsoleColor.DarkGray;
				}
			}

			// далее рисуем точки в которые уже попадали
			for (byte j = 0; j < this.size; j++) {
				// ключевая точка это подбитый/установленный/уничтоженный корабль
				bool isKeyPoint = colorMap[j] == ConsoleColor.Black || colorMap[j] == ConsoleColor.Magenta || colorMap[j] == ConsoleColor.Red;
				if (!isKeyPoint && this.isCheckCell(i, j)) colorMap[j] = ConsoleColor.DarkBlue;
			}

			for (byte j = 0; j < this.size; j++) {
				Console.BackgroundColor = colorMap[j];
				Console.Write("  ");
				Console.ResetColor();
			}

			Console.BackgroundColor = ConsoleColor.White;
			Console.Write("  ");
			Console.ResetColor();
		}

		public bool isCheckCell(byte i, byte j) {
			return checkCells.Any(cell => cell.X == j && cell.Y == i);
		}

		public List<Point> getFreePoints() {
			List<Point> freePoints = new List<Point>() {};
			for (byte i = 0; i < this.size; i++) {
				for (byte j = 0; j < this.size; j++) {
					if (!isCheckCell(i, j)) {
						freePoints.Add(new Point(j, i));
					}
				}
			}
			return freePoints;
		}

		private void createShips() {
			this.ships = new List<Ship>[] {
				new List<Ship>{new Ship(1), new Ship(1), new Ship(1), new Ship(1)},
				new List<Ship>{new Ship(2), new Ship(2), new Ship(2)},
				new List<Ship>{new Ship(3), new Ship(3)},
				new List<Ship>{new Ship(4)}
			};

			this.placedShips = new List<Ship>();
			this.explodedShips = new List<Ship>();
		}

	}
}
