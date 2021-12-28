using System;
using static consoleSeaBattle.Game;

namespace consoleSeaBattle {
	class Player {
		public Field field;
		protected string loaderText = "Автоматическая расстановка";
		protected string fieldName = "Ваше поле";
		protected bool showCreate = true;
		private int interval = 5;
		public static byte playerCount = 0;

		// подбитая часть корабля соперника
		protected List<Point> explodedPoints = new List<Point>();
		protected List<Point> potentialExplodedPoints = new List<Point>();

		protected Point potentialExploded = null;

		public static List<Player> All;

		public Player() {
			this.Id = Player.playerCount;
			Player.playerCount++;
		}

		public byte Id {get; private set;}

		public async Task<bool> setField(bool bot = false) {
			this.field = new Field(this.fieldName, bot);
			string status = bot ? "auto" : this.field.setShips();
			if (status == "auto") {
				await this.field.autoSet(this.interval, this.showCreate, this.loaderText);
			} else if (status != "done") {
				return false;
			}
			return true;
		}

		// Метод выводит поле игрока
		public void printField() {
			this.field.modernPrint();
		}

		// метод выводит по горизонтали поля игроков
		public static void print() {
			Render.Clear();
			(_, int top) = Console.GetCursorPosition();
			for (byte i = 0; i < Player.All.Count; i++) {
				Console.SetCursorPosition(i*32, top);
				Player.All[i].printField();
			}
			(_, top) = Console.GetCursorPosition();
			Console.SetCursorPosition(1, top + 1);
		}

		// async потому что у бота будет метод переопределен с async/await
		public virtual async Task<bool> aiming(Player target) {
			// создаем прицел
			Point start = new Point(5, 5);
			Ship aim = new Ship(true);

			target.field.setAim(aim);

			bool isPointed = false;
			do {
				aim.setAim(start);
				Player.print();

				ConsoleKeyInfo key = Console.ReadKey(true);
				isPointed = target.field.controlClick(aim, start, key.Key);
			} while (!isPointed);
			return true;
		}

		public async Task<string> turn(Player target) {
			bool end = false;
			string turn = "miss";
			do {
				end = await this.aiming(target);
				string status = target.field.hit();
				// Если уничтожили кораблик, очистим потенциальные точки
				// Иначе допишем в подбитые части и пересчитаем потенциальные
				if (status == "exp" && this.potentialExploded != null) {
					this.explodedPoints.Clear();
					this.potentialExplodedPoints.Clear();
				} else if (status == "hit" && this.potentialExploded != null) {
					this.explodedPoints.Add(this.potentialExploded);
					this.potentialExplodedPoints.Clear();
					this.calculatePotential();
				}
				turn = status;
				target.field.removeAim();
			} while (!end);

			return turn;
		}

		public void calculatePotential() {
			// Если видна только одна подбитая часть, то потенциально корабль со всех сторон
			if (this.explodedPoints.Count == 1) {
				Point point = this.explodedPoints[0];

				int top = point.Y - 1 < 0 ? 0 : point.Y - 1;
				this.potentialExplodedPoints.Add(new Point(point.X, Convert.ToByte(top)));

				int right = point.X + 1 > 9 ? 9 : point.X + 1;
				this.potentialExplodedPoints.Add(new Point(Convert.ToByte(right), point.Y));

				int bottom = point.Y + 1 > 9 ? 9 : point.Y + 1;
				this.potentialExplodedPoints.Add(new Point(point.X, Convert.ToByte(bottom)));

				int left = point.X - 1 < 0 ? 0 : point.X - 1;
				this.potentialExplodedPoints.Add(new Point(Convert.ToByte(left), point.Y));
			} else {
				int size = this.explodedPoints.Count;
				bool isVertical = this.explodedPoints[0].X - this.explodedPoints[size - 1].X == 0;
				if (isVertical) {
					Point start = null;
					Point end = null;
					explodedPoints.ForEach(point => {
						if (start == null || point.Y < start.Y) start = point;
						if (end == null || point.Y > end.Y) end = point;
					});

					int top = start.Y - 1 < 0 ? 0 : start.Y - 1;
					this.potentialExplodedPoints.Add(new Point(start.X, Convert.ToByte(top)));

					int bottom = end.Y + 1 > 9 ? 9 : end.Y + 1;
					this.potentialExplodedPoints.Add(new Point(end.X, Convert.ToByte(bottom)));

				} else {
					Point start = null;
					Point end = null;
					explodedPoints.ForEach(point => {
						if (start == null || point.X < start.X) start = point;
						if (end == null || point.X > end.X) end = point;
					});

					int left = start.X - 1 < 0 ? 0 : start.X - 1;
					this.potentialExplodedPoints.Add(new Point(Convert.ToByte(left), start.Y));

					int right = end.X + 1 > 9 ? 9 : end.X + 1;
					this.potentialExplodedPoints.Add(new Point(Convert.ToByte(right), end.Y));
				}
			}
		}

		// Метод запускает противостояние между противниками и возвращает победителя
		public static async Task<Player> battle(List<Player> players) {
			Player.All = players;
			int counter = 0;
			bool nextTurn = false;
			Player winner, target;
			do {
				int targetCounter = counter + 1 == Player.All.Count ? 0 : counter + 1;

				winner = Player.All[counter];
				target = Player.All[targetCounter];

				string turn = await winner.turn(target);
				nextTurn = true;
				if (turn == "miss") counter = counter + 1 == Player.All.Count ? 0 : counter + 1;
				if (turn == "gameover" && Player.All.Count > 2) {
					Player.All.Remove(target);
				} else if (turn == "gameover") {
					nextTurn = false;
				}
			} while (nextTurn);

			return winner;
		}
	}
}
