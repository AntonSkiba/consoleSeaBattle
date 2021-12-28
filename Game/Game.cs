using System;

namespace consoleSeaBattle {
	class Game {
		private Menu menu;

		public Game() {
			MenuItem newGame = new MenuItem("newGame", "Новая игра");
			MenuItem exit = new MenuItem("exit", "Выход");
			this.menu = new Menu("Добро пожаловать в консольный Морской бой!", new List<MenuItem> {newGame, exit});
		}

		public async Task start() {
			bool exit = false;
			do {
				Render.RenderTitle("  Морской бой", ConsoleColor.Blue);
				this.menu.print();
				string menuKey = this.menu.select();

				if (menuKey == "newGame") {
					// Создаем поле игрока
					// Field playerField = new Field("Ваше поле");
					// string status = playerField.setShips();
					// if (status == "auto") {
					// 	await playerField.autoSet(5, true, "Автоматическая расстановка");
					// } else if (status != "done") {
					// 	continue;
					// }

					Player player = new Player();
					bool playerReady = await player.setField();
					if (!playerReady) continue;

					MenuItem next = new MenuItem("next", "Продолжить");
					MenuItem reset = new MenuItem("reset", "Сбросить");
					Menu nextMenu = new Menu("Кораблики расставлены!", new List<MenuItem> {next, reset});
					nextMenu.print(player.printField);

					string nextMenuKey = nextMenu.select();
					if (nextMenuKey == "reset") {
						continue;
					}

					Bot bot = new Bot();
					await bot.setField();

					// Field enemyField = new Field("Поле противника");
					// await enemyField.autoSet(5, false, "Противник расставляет кораблики");

					MenuItem yes = new MenuItem("yes", "ДА!");
					MenuItem no = new MenuItem("no", "Нет :c");
					Menu yesNoMenu = new Menu("Все готово! Приступить к бою?", new List<MenuItem> {yes, no});
					yesNoMenu.print();

					string yesNoMenuKey = yesNoMenu.select();
					if (yesNoMenuKey == "no") {
						continue;
					}
					Player winner = await Player.battle(new List<Player> {player, bot});

					if (player.Id == winner.Id) {
						Render.RenderWin();
					} else {
						Render.RenderLose();
					}
					MenuItem gameover = new MenuItem("next", "Enter - Продолжить");
					Menu gameoverMenu = new Menu("", new List<MenuItem> {gameover});
					gameoverMenu.print();
					gameoverMenu.select();

				} else if (menuKey == "score") {
					Console.WriteLine("Пользователь решил посмотреть счет");
				} else if (menuKey == "exit") {
					exit = true;
				}


			} while (!exit);
		}
	}
}
