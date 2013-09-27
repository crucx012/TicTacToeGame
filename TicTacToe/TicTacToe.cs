using System;
using System.Linq;

namespace TicTacToe
{
    public enum Marker { N = 0, O = 1, X = 2 };

    public class TicTacToe
    {
        static readonly Cell[] Cells = new Cell[9];
        readonly GameBoard _board = new GameBoard(Cells);
        Player _person1;
        Player _person2;
        private int _turn;
        private int _order;
        readonly int[] _score = new int[3];
        private readonly Marker[,] _plays = new Marker[9, 9];

        public TicTacToe()
        {
            for (var i = 0; i < 9; i++)
                Cells[i] = new Cell { CurrentMarker = Marker.N };
            for (var i = 0; i < 3; i++)
                _score[i] = 0;
        }

        public void PreparePlayers(int players)
        {
            if (players == 0)
            {
                _person1 = new Cpu();
                _person2 = new Cpu();
            }
            if (players == 1)
            {
                _person1 = new Human();
                _person2 = new Cpu();
            }
            if (players == 2)
            {
                _person1 = new Human();
                _person2 = new Human();
            }
        }

        public void SetGameMarkers(Marker marker1, Marker marker2)
        {
            _person1.Marker = marker1;
            _person2.Marker = marker2;
        }
        
        public string UpdateCell(int cell)
        {
            if (Cells[cell] == null)
                return " ";
            switch (Cells[cell].CurrentMarker)
            {
                case Marker.O: return "O";
                case Marker.X: return "X";
                default: return " ";
            }
        }

        public int FirstTurnSecondPlayer()
        {
            var random = new Random();
            _order = random.Next(0, 2);

            return _order;
        }

        public void GameLoop(int value)
        {
            bool isGameOver;
            do
            {
                if (_order == 0)
                PersonOneTurn(_person1.IsCpu() ? 0: value);
                else 
                PersonTwoTurn(_person1.IsCpu() ? 0: value);
                isGameOver = CheckIfGameIsOver();
            } while (!(isGameOver));
        }

        public int PersonOneTurn(int index)
        {
            _person1.Turn(_board, _person1.IsCpu() ? _person1.AI(_board) : Cells[index]);
            _order++;
            return _order;
        }

        public int PersonTwoTurn(int index)
        {
            _person2.Turn(_board, _person2.IsCpu() ? _person2.AI(_board) /*_board.ChooseRandomRemainingCell()*/ : Cells[index]);
            _order--;
            return _order;
        }

        public void SetPlays()
        {
            for (var i = 0; i < 9; i++)
                _plays[_turn, i] = Cells[i].CurrentMarker;
            _turn++;
        }

        public bool CheckIfCellIsAvailable(int index)
        {
            var isNotAvailable = Cells[index].CurrentMarker != Marker.N;
            return isNotAvailable;
        }

        public bool CheckIfGameIsOver()
        {
            Marker winner = _board.CheckBoardForWinner();
            if (winner != Marker.N)
            {
                AccumulatePersonScore(winner);
                return true;
            }
            if (Cells.All(c => c.CurrentMarker != Marker.N))
            {
                AccumulateDrawScore();
                return true;
            }
            return false;
        }

        public Marker CheckForGameWinner()
        {
            var winner = _board.CheckBoardForWinner();
            return winner != Marker.N ? winner : Marker.N;
        }

        public int[] AccumulatePersonScore(Marker personMarker)
        {
            if (personMarker == _person1.Marker)
                _score[0]++;
            if (personMarker == _person2.Marker)
                _score[1]++;
            return _score;
        }

        public int[] AccumulateDrawScore()
        {
            _score[2]++;
            return _score;
        }

        public int[] UpdateScores()
        {
            return _score;
        }

        public Marker[,] UpdatePlays()
        {
            return _plays;
        }

        public void NewGame()
        {
            for (var i = 0; i < 9; i++)
                Cells[i].CurrentMarker = Marker.N;
            for (var i = 0; i < 9; i++)
                for (var j = 0; j < 9; j++)
                    _plays[i, j] = Marker.N;
            _turn = 0;
        }

        public int[] Restart()
        {
            NewGame();
            for (var i = 0; i < 3; i++)
                _score[i] = 0;
            return UpdateScores();
        }
    }
}