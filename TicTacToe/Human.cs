namespace TicTacToe
{
    public class Human : Player
    {

        public override Cell AI(GameBoard board)
        {
            return null;
        }

        public override void Turn(GameBoard board, Cell claimedCell)
        {
            board.SetClaimedCell(claimedCell, Marker);
        }

        public override bool IsCpu()
        {
            return false;
        }
    }
}