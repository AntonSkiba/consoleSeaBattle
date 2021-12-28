using System;
using static consoleSeaBattle.Game;

namespace consoleSeaBattle {
	class Ship {
		public byte size;
		private Point[] points;
		private Point[] around;
		private List<Point> exploded;
		public bool isVertical = true;

		public Ship(byte size) {
			this.size = size;
			this.points = new Point[size];
			// окружение это 3 + 3 точки на вершинах и size*3 точек тело корабля
			this.around =  new Point[(size + 2)*3];
			this.exploded = new List<Point>() {};
		}

		public Ship(bool isDirection) {
			this.size = 1;
			this.points = new Point[1];
			// по 4 клетки в каждом направлении
			this.around = new Point[16];
			this.exploded = new List<Point>() {};
		}

		public void setAim(Point start) {
			this.points[0] = start;

			for (byte i = 0; i < 4; i++) {
				// все точки слева
				int left = start.X - i;
				if (left < 0) left = 0;
				this.around[i*4] = new Point(Convert.ToByte(left), start.Y);

				// все точки справа
				int right = start.X + i;
				if (right > 9) right = 9;
				this.around[i*4 + 1] = new Point(Convert.ToByte(right), start.Y);

				// все точки сверху
				int top = start.Y - i;
				if (top < 0) top = 0;
				this.around[i*4 + 2] = new Point(start.X, Convert.ToByte(top));

				// все точки снизу
				int bottom = start.Y + i;
				if (bottom > 9) bottom = 9;
				this.around[i*4 + 3] = new Point(start.X, Convert.ToByte(bottom));
			}
		}

		public void setPoints(Point start) {
			for (byte i = 0; i < this.size; i++) {
				if (isVertical) {
					this.points[i] = new Point(start.X, Convert.ToByte(start.Y + i));

				} else {
					this.points[i] = new Point(Convert.ToByte(start.X + i), start.Y);
				}
			}

			if (isVertical) {
				int startY = this.points[0].Y - 1;
				int endY = this.points[this.size - 1].Y + 1;
				int startX = this.points[0].X - 1;
				int endX = this.points[0].X + 1;
				int aroundCount = 0;

				for (int y = 0; y <= endY - startY; y++) {
					for (int x = 0; x <= endX - startX; x++) {
						int realX = x + startX;
						int realY = y + startY;
						if (realX < 0) realX = 0;
						if (realY < 0) realY = 0;
						this.around[aroundCount] = new Point(Convert.ToByte(realX), Convert.ToByte(realY));
						aroundCount++;
					}
				}
			} else {
				int startY = this.points[0].Y - 1;
				int endY = this.points[0].Y + 1;
				int startX = this.points[0].X - 1;
				int endX = this.points[this.size - 1].X + 1;
				int aroundCount = 0;

				for (int x = 0; x <= endX - startX; x++) {
					for (int y = 0; y <= endY - startY; y++) {
						int realX = x + startX;
						int realY = y + startY;
						if (realX < 0) realX = 0;
						if (realY < 0) realY = 0;
						this.around[aroundCount] = new Point(Convert.ToByte(realX), Convert.ToByte(realY));
						aroundCount++;
					}
				}
			}
		}
		
		public Point[] getPoints() {
			return this.points;
		}

		public Point[] getAround() {
			return this.around;
		}

		public static bool isAround(List<Ship> ships, Point position) {
			bool isAround = false;
			ships.ForEach(ship => {
				if (ship.inAround(position)) {
					isAround = true;
				}
			});
			return isAround;
		}

		// метод проверяет входит ли точка в область кораблика
		public bool inAround(Point position) {
			bool inAround = false;
			for (byte i = 0; i < this.around.Length; i++) {
				if (position.intersect(this.around[i])) {
					inAround = true;
				}
			}
			return inAround;
		}

		// метод проверяет входит ли точка в кораблик
		public bool inShip(Point position) {
			bool inShip = false;
			for (byte i = 0; i < this.points.Length; i++) {
				if (position.intersect(this.points[i])) {
					inShip = true;
				}
			}
			return inShip;
		}

		public static string getType(List<Ship> ships, Point position) {
			byte aroundCount = 0;
			byte shipCount = 0;
			bool isExploded = false;

			ships.ForEach(ship => {
				if (ship.inAround(position)) {
					aroundCount++;
				}
				if (ship.inShip(position)) {
					shipCount++;
				}
				if (ship.isExploded(position)) {
					isExploded = true;
				}
			});

			if (isExploded) return "exploded";
			// если найдено более одного кораблика в точке, то конфликт
			if (shipCount > 1 || (shipCount == 1 && aroundCount > 1)) {
				return "conflict";
			} else if (shipCount == 1) {
				return "placed";
			} else return "";

		}

		public static Ship hit(List<Ship> ships, Point position) {
			Ship explodedShip = null;
			ships.ForEach(ship => {
				bool isTotalExploded = false;
				if (ship.inShip(position)) {
					isTotalExploded = ship.setExploded(position);
				}

				if (isTotalExploded) {
					explodedShip = ship;
				}
			});

			return explodedShip;
		}
		public bool isExploded(Point pos) {
			bool isExploded = false;
			this.exploded.ForEach(point => {
				if (point.X == pos.X && point.Y == pos.Y) {
					isExploded = true;
				}
			});
			return isExploded;
		}
		public bool setExploded(Point pos) {
			foreach (Point point in this.points) {
				if (point.X == pos.X && point.Y == pos.Y) {
					this.exploded.Add(point);
				}
			}

			return this.exploded.Count == this.points.Length;
		}
	}
}
