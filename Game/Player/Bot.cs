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
			
			// if (base.exploded != null) {
			// 	start = new Point(base.exploded.X, base.exploded.Y);
			// } else {
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
				
				
				// // Если есть потенциальные точки, идем к ним
				// if (base.potentialExplodedPoints.Count > 0) {
				// 	// определяем направление
				// 	randomPoint = base.potentialExplodedPoints[0];
				// 	// byte controlIdx = 10;
				// 	// if (start.Y - endPoint.Y > 0) controlIdx = 0;
				// 	// if (start.X - endPoint.X > 0) controlIdx = 1;
				// 	// if (start.Y - endPoint.Y < 0) controlIdx = 2;
				// 	// if (start.X - endPoint.X < 0) controlIdx = 3;

				// 	// если бот дошел до потенциальной точки, он стреляет
				// 	// if (controlIdx == 10) {
				// 	// 	ConsoleKey enter = keys[4];
				// 	// 	isPointed = target.field.controlClick(aim, start, enter);
				// 	// 	base.potentialExplodedPoints.RemoveAt(0);
				// 	// 	base.potentialExploded = aim.getPoints()[0];
				// 	// } else {
				// 	// 	ConsoleKey control = keys[controlIdx];
				// 	// 	target.field.controlClick(aim, start, control);
				// 	// }
				// } else {
				// 	int controlIdx = this.random.Next(keys.Length);

				// 	// если выпал enter то нужно определить пустая ли эта клетка
				// 	if (controlIdx == 4) {
				// 		bool isCheckCell = target.field.isCheckCell(aim.getPoints()[0].Y, aim.getPoints()[0].X);
				// 		if (isCheckCell) {
				// 			controlIdx = this.random.Next(keys.Length - 1);
				// 		}
				// 	}
				// 	ConsoleKey randomControl = keys[controlIdx];
				// 	isPointed = target.field.controlClick(aim, start, randomControl);
				// 	base.potentialExploded = aim.getPoints()[0];
				// }
			}
			return true;
		}
	}
}
