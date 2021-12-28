using System;
using static consoleSeaBattle.Ship;

namespace consoleSeaBattle {
	class Point {
		private byte x;
		private byte y;

		public Point(byte x, byte y) {
			this.x = x;
			this.y = y;
		}

		public Point[] getAround() {
			Point[] around = new Point[8];
			byte left = Convert.ToByte(this.x - 1);
			byte right = Convert.ToByte(this.x + 1);
			byte top = Convert.ToByte(this.y - 1);
			byte bottom = Convert.ToByte(this.y + 1);

			around[0] = new Point(left, top);
			around[1] = new Point(this.x, top);
			around[2] = new Point(right, top);

			around[3] = new Point(left, bottom);
			around[4] = new Point(this.x, bottom);
			around[5] = new Point(right, bottom);

			around[6] = new Point(left, this.y);
			around[7] = new Point(right, this.y);

			return around;
		}

		public bool intersect(Point point) {
			return this.x == point.x && this.y == point.y;
		}

		public byte X {
			get {
				return this.x;
			}
			set {
				this.x = value;
			}
		}

		public byte Y {
			get {
				return this.y;
			}
			set {
				this.y = value;
			}
		}

		public byte[] C {
			get {
				return new byte[] {this.x, this.y};
			}
		}
	}
}
