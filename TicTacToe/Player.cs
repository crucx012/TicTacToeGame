namespace TicTacToe
{
    public abstract class Player
    {
        public Marker Marker = Marker.X;

        public abstract Cell AI(GameBoard board);

        public abstract void Turn(GameBoard board, Cell claimedCell);

        public abstract bool IsCpu();
    }
}