using System;

namespace consoleSeaBattle {
	class Bot : Player {
		private Random random;

		public Bot(): base() {
			base.fieldName = "Поле противника";
			base.loaderText = "Противник расставляет свои корабли";
			base.showCreate = false;
			this.random = new Random();
		}

		public async Task<bool> setField() {
			return await base.setField(true);
		}

		public override async Task<bool> aiming(Player target) {
			// создаем прицел
			Point start = new Point(Convert.ToByte(random.Next(9)), Convert.ToByte(random.Next(9)));

			// определяем свободные точки на поле
			List<Point> freePoints = target.field.getFreePoints();
			
			Ship aim = new Ship(true);
			PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromMilliseconds(100));

			ConsoleKey[] keys = new ConsoleKey[5] {
				ConsoleKey.W,
				ConsoleKey.A,
				ConsoleKey.S,
				ConsoleKey.D,
				ConsoleKey.Enter
			};

			target.field.setAim(aim);

			bool isPointed = false;
			while (!isPointed) {
				await timer.WaitForNextTickAsync();
				Point randomPoint = freePoints[this.random.Next(freePoints.Count)];
				int randomControl = this.random.Next(5);

				if (base.potentialExplodedPoints.Count > 0) {
					int randomPotentialIndex = this.random.Next(base.potentialExplodedPoints.Count);
					randomPoint = base.potentialExplodedPoints[randomPotentialIndex];
					randomControl = 4;
					base.potentialExplodedPoints.RemoveAt(randomPotentialIndex);
				}

				aim.setAim(randomPoint);
				Player.print();

				if (randomControl == 4) {
					isPointed = target.field.controlClick(aim, randomPoint, ConsoleKey.Enter);
					base.potentialExploded = aim.getPoints()[0];
				}
			}
			return true;
		}
	}
}
