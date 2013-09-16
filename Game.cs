using System;
using System.Globalization;
using TicTacToe;

namespace TTT
{
    class Program
    {
        public static readonly TicTacToe.TicTacToe TicTacToe = new TicTacToe.TicTacToe();
        private const int FirstPlayerTurn = 0;
        private const int SecondPlayerTurn = 1;
        private const int RefreshInterval = 10;
        private const ConsoleColor TextColor = ConsoleColor.Green;
        private const ConsoleColor XColor = ConsoleColor.Red;
        private const ConsoleColor OColor = ConsoleColor.Blue;
        private static string _marker;
        private static int _players;
        private static int _order;
        private static int[] _scores = new int[3];
        private static bool _continue = true;
        private static bool _showPlays;

        static void Main()
        {
            GetNumberOfPlayers();
            GetpersonMarker();
            GetShowPlays();
            SetPlayerMarkers(_players, _marker);

            do
            {
                PlayGameLoop();
                _continue = PromptNewGame();
            } while (_continue && _scores[2] < int.MaxValue);
            Console.Clear();
            Console.WriteLine("GoodBye");
            Console.ReadKey();
        }

        private static void GetNumberOfPlayers()
        {
            Console.ForegroundColor = TextColor;
            bool isNotValid;
            bool isNotNumber;
            do
            {
                Console.Clear();
                Console.WriteLine("Number of Players? (0-2)");
                var number = Console.ReadKey().KeyChar.ToString(CultureInfo.InvariantCulture);
                isNotNumber = !int.TryParse(number, out _players);
                isNotValid = _players != 0 && _players != 1 && _players != 2;
                Console.Clear();
            } while (isNotValid || isNotNumber);
        }

        private static void GetpersonMarker()
        {
            bool isNotValid;
            do
            {
                Console.WriteLine("First Player Marker? (X or O)");
                _marker = Console.ReadKey().KeyChar.ToString(CultureInfo.InvariantCulture).ToUpper();
                isNotValid = _marker != "O" && _marker != "X";
                Console.Clear();
            } while (isNotValid);
        }

        private static void GetShowPlays()
        {
            string confirm;
            bool isNotValid;
            do
            {
                Console.Clear();
                Console.WriteLine("Display Turn By Turn? (y/n)");
                confirm = Console.ReadKey().KeyChar.ToString(CultureInfo.InvariantCulture);
                isNotValid = confirm != "y" && confirm != "n";
            } while (isNotValid);

            if (confirm == "y")
                _showPlays = true;
            if (confirm == "n")
                _showPlays = false;
        }

        private static void SetPlayerMarkers(int players, string marker)
        {
            TicTacToe.PreparePlayers(players);
            if (marker == "X")
                TicTacToe.SetGameMarkers(global::TicTacToe.Marker.X, global::TicTacToe.Marker.O);
            else
                TicTacToe.SetGameMarkers(global::TicTacToe.Marker.O, global::TicTacToe.Marker.X);
        }

        private static void PlayGameLoop()
        {
            Console.Clear();
            DisplayScores();
            DisplayGrid();
            FirstTurn();
            TurnLoop();
            DisplayWinner();
        }

        private static void DisplayGrid()
        {
            DrawPipes();
            DrawRow(0, 1, 2);
            DrawPipesBarPipes();
            DrawRow(3, 4, 5);
            DrawPipesBarPipes();
            DrawRow(6, 7, 8);
            DrawPipes();
        }

        private static void DrawRow(int a, int b, int c)
        {
            Console.Write("  ");
            DrawCell(a);
            DrawSeparator();
            DrawCell(b);
            DrawSeparator();
            DrawCell(c);
            Console.Write("  ");
        }

        private static void DrawCell(int index)
        {
            const string column = "{0}";
            Console.ForegroundColor = TicTacToe.UpdateCell(index) == "X" ? XColor : OColor;
            Console.Write(column, TicTacToe.UpdateCell(index));
            Console.ForegroundColor = TextColor;
        }

        private static void DrawPipesBarPipes()
        {
            DrawPipes();
            DrawBar();
            DrawPipes();
        }

        private static void DrawPipes()
        {
            const string pipes = "\n     ║     ║     ";
            Console.WriteLine(pipes);
        }

        private static void DrawBar()
        {
            const string bars = "═════╬═════╬═════";
            Console.Write(bars);
        }

        private static void DrawSeparator()
        {
            const string row = "  ║  ";
            Console.Write(row);
        }

        private static void DisplayScores()
        {
            _scores = TicTacToe.UpdateScores();
            Console.WriteLine("{0}:{1}, {2}:{3}, N:{4}", _marker, _scores[0], _marker == "X" ? "O" : "X", _scores[1], _scores[2]);
        }

        private static void FirstTurn()
        {
            _order = TicTacToe.FirstTurnSecondPlayer();
            if (_order == 0) return;
            if (_players != 0)
                Console.WriteLine("\nSecond Player:");
            _order = TicTacToe.PersonTwoTurn(_players == 2 ? ChooseSquare() : HitAnyKey());
            GetPlays();
        }

        private static void TurnLoop()
        {
            bool isGameOver;
            do
            {
                if (_players != 0)
                {
                    RedrawBoard();
                    Console.WriteLine(_order == 0 ? "\nFirst Player:" : "\nSecond Player:");
                }

                if (_order == FirstPlayerTurn)
                    _order = TicTacToe.PersonOneTurn(_players == 0 ? HitAnyKey() : ChooseSquare());
                else if (_order == SecondPlayerTurn)
                    _order = TicTacToe.PersonTwoTurn(_players == 2 ? ChooseSquare() : HitAnyKey());

                GetPlays();
                isGameOver = TicTacToe.CheckIfGameIsOver();

            } while (!isGameOver);
        }

        private static void GetPlays()
        {
            if (_showPlays)
                TicTacToe.SetPlays();
        }

        private static void RedrawBoard()
        {
            Console.Clear();
            DisplayScores();
            DisplayGrid();
        }

        private static int ChooseSquare()
        {
            bool isNotIndex;
            var isNotAvailable = false;
            string square;
            Console.WriteLine("\nChoose a square to claim:");
            Console.WriteLine("1|2|3");
            Console.WriteLine("4|5|6");
            Console.WriteLine("7|8|9");

            do
            {
                square = Console.ReadKey().KeyChar.ToString(CultureInfo.InvariantCulture);
                isNotIndex = square != "1" && square != "2" && square != "3" && square != "4" && square != "5" &&
                             square != "6" && square != "7" && square != "8" && square != "9";

                RedrawBoard();
                Console.WriteLine(_order == 0 ? "\nFirst Player:" : "\nSecond Player:");

                if (isNotIndex)
                    Console.WriteLine("\nplease enter 1 through 9");
                else
                {
                    isNotAvailable = TicTacToe.CheckIfCellIsAvailable(int.Parse(square) - 1);
                    if (!isNotAvailable) continue;
                    Console.WriteLine("\nplease enter an empty square");
                }
            } while (isNotIndex || isNotAvailable);

            var cell = int.Parse(square) - 1;

            return cell;
        }

        private static int HitAnyKey()
        {
            if (_players == 0) return 0;
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            return 0;
        }

        private static void DisplayWinner()
        {
            var winner = TicTacToe.CheckForGameWinner();
            if (_players == 0 && _scores[2] % RefreshInterval != 0 && !Console.KeyAvailable) return;
            RedrawBoard();
            Console.WriteLine("\n" + winner + " Won!");
            if (_showPlays && Console.KeyAvailable || _players > 0)
                DisplayTurnByTurnBoard();
        }

        private static void DisplayTurnByTurnBoard()
        {
            var plays = TicTacToe.UpdatePlays();
            for (var i = 0; i < 9; i++)
            {
                if (IsTurnBlank(plays, i))
                    return;
                Console.WriteLine("\n");
                for (var j = 0; j < 9; j += 3)
                    Console.WriteLine(plays[i, j].ToString() + plays[i, j + 1] + plays[i, j + 2]);
            }
        }

        private static bool IsTurnBlank(Marker[,] plays, int i)
        {
            var isBlankTurn = true;
            for (var j = 0; j < 9; j++)
                if (plays[i, j] != Marker.N)
                    isBlankTurn = false;
            return isBlankTurn;
        }

        private static bool PromptNewGame()
        {
            if (_players > 0 || _players == 0 && Console.KeyAvailable)
                NewGame();
            else
                TicTacToe.NewGame();

            return _continue;
        }

        private static void NewGame()
        {
            string confirm;
            bool isNotValid;
            Console.WriteLine("\n\nNew Game? (y/n)");

            do
            {
                confirm = Console.ReadKey().KeyChar.ToString(CultureInfo.InvariantCulture);
                isNotValid = confirm != "y" && confirm != "n";
            } while (isNotValid);

            if (confirm == "y")
                TicTacToe.NewGame();
            if (confirm == "n")
                _continue = PromptRestart();
        }

        private static bool PromptRestart()
        {
            string confirm;
            bool isNotValid;
            Console.Clear();
            Console.WriteLine("Restart? (y/n)");

            do
            {
                confirm = Console.ReadKey().KeyChar.ToString(CultureInfo.InvariantCulture);
                isNotValid = confirm != "y" && confirm != "n";
            } while (isNotValid);

            if (confirm == "y")
            {
                _scores = TicTacToe.Restart();
                Main();
            }
            if (confirm == "n")
                _continue = false;
            return _continue;
        }
    }
}