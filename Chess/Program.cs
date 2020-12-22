using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Ryan's Chess";
            //initilize board and pl1 going first
            Board board = new Board();
            Board.Team player = Board.Team.White;

            Console.WriteLine("Welcome to a game of Chess");
            board.PrintBoard();
            while (!board.gameOver){
                string source = "";
                string destination = "";
                do{
                    do {
                        Console.WriteLine(player.ToString() + " enter source Piece (e.i A4): ");
                        source = Console.ReadLine().Trim().ToUpper();
                    } while (!board.AvailableSource(player, source));
                    //check if good peice
                    Console.WriteLine(player.ToString() + " enter destination (e.i. H7): ");
                    destination = Console.ReadLine().Trim().ToUpper();
                } while (!board.ExecuteMove(player, source, destination));
                //change player
                player = Board.Team.Black == player ? Board.Team.White : Board.Team.Black;
                board.PrintBoard();
            }
            Console.WriteLine(string.Format("{0} team wins!", Board.Team.Black == player ? Board.Team.White.ToString() : Board.Team.Black.ToString()));
            Console.ReadLine();
        }
    }


    //contains all the implementaion for the game chess
    public class Board
    {
        private static uint deminsion = 8;
        private Space[,] rep;
        public bool gameOver;   //false when still; true when king killed


        //starts a new game of chess
        //intitailizes {@code rep}
        public Board()
        {
            gameOver = false;
            rep = new Space[8, 8];
            for(int col = 0; col < deminsion; col++)
            {
                for(int row = 0; row < deminsion; row++)
                    {
                    if(row == 0 || row == 1)
                    {
                        Team white = Team.White;
                        if(row == 0)
                        {
                            if(col == 0 || col == 7)
                            {
                                rep[col, row] = new Space(Value.Rook, white, false);
                            } else if(col == 1 || col == 6)
                            {
                                rep[col, row] = new Space(Value.Knight, white);
                            } else if(col == 2 || col == 5)
                            {
                                rep[col, row] = new Space(Value.Bishop, white);
                            } else if(col == 4)
                            {
                                rep[col, row] = new Space(Value.King, white, false);
                            } else
                            {
                                rep[col, row] = new Space(Value.Queen, white);
                            }
                        } else
                        {
                            rep[col, row] = new Space(Value.Pawn, white);
                        }
                    } else if(row == 6 || row == 7)
                    {
                        Team white = Team.Black;
                        if (row == 7)
                        {
                            if (col == 0 || col == 7)
                            {
                                rep[col, row] = new Space(Value.Rook, white, false);
                            }
                            else if (col == 1 || col == 6)
                            {
                                rep[col, row] = new Space(Value.Knight, white);
                            }
                            else if (col == 2 || col == 5)
                            {
                                rep[col, row] = new Space(Value.Bishop, white);
                            }
                            else if (col == 4)
                            {
                                rep[col, row] = new Space(Value.King, white, false);
                            }
                            else
                            {
                                rep[col, row] = new Space(Value.Queen, white);
                            }
                        }
                        else
                        {
                            rep[col, row] = new Space(Value.Pawn, white);
                        }
                    }
                    else
                    {
                        rep[col, row] = new Space(Value.Empty, Team.None);
                    }
                }
            }

        }
        public void PrintBoard()
        {
            Console.Clear();
            var tileColor1 = ConsoleColor.DarkGray;
            var tileColor2 = ConsoleColor.DarkRed;
            var axisColor = ConsoleColor.Cyan;
            var whiteColor = ConsoleColor.White;
            var blackColor = ConsoleColor.Black;
            var backgroundColor = ConsoleColor.Black;
            var backgroundWriting = ConsoleColor.Black;
            var foregroundWriting = ConsoleColor.White;
            Console.BackgroundColor = backgroundColor;
            StringBuilder header = new StringBuilder();
            Console.ForegroundColor = axisColor;
            header.Append("    A  B  C  D  E  F  G  H ");
            Console.WriteLine(header);
            for(int row = 0; row < deminsion; row++)
            {
                Console.BackgroundColor = backgroundColor;
                Console.ForegroundColor = axisColor;
                StringBuilder str = new StringBuilder();
                str.Append(" " + (row + 1).ToString() + " " /*"|"*/);
                Console.Write(str);
                if(row % 2 == 0) {
                    Console.BackgroundColor = tileColor1;
                }
                for(int col = 0; col < deminsion; col++)
                {
                    Console.BackgroundColor = Console.BackgroundColor == tileColor1 ? tileColor2 : tileColor1;
                    var currentTileColor = Console.BackgroundColor;
                    Console.ForegroundColor = rep[col, row].team == Team.Black ? blackColor : whiteColor;
                    String piece = " ";
                    switch (rep[col, row].worth)
                    {
                        case Value.King:
                            piece += "K";
                            break;
                        case Value.Queen:
                            piece += "Q";
                            break;
                        case Value.Bishop:
                            piece += "B";
                            break;
                        case Value.Rook:
                            piece += "R";
                            break;
                        case Value.Knight:
                            piece += "N";
                            break;
                        case Value.Pawn:
                            piece += "P";
                            break;
                        case Value.Empty:
                            piece += " ";
                            break;
                    }
                    Console.Write(piece + " ");
                    Console.BackgroundColor = currentTileColor;
                }
                Console.BackgroundColor = backgroundColor;
                Console.ForegroundColor = axisColor;
                Console.WriteLine();
            }
            Console.ForegroundColor = foregroundWriting;
            Console.BackgroundColor = backgroundWriting;
        }

        //determines if user input is able to be a peice that is moved
        //@requires {@code input} all upper case and no spaces on either sides
        public bool AvailableSource(Team team, string input)
        {
            //use multiple retunr statements for simplicity
            if(input.Length != 2)
            {
                return false;
            }
            char[] inputChar = input.ToCharArray();
            int colIndex = (int)(inputChar[0]) - 65;
            int rowIndex = Convert.ToInt32(inputChar[1]) - 49;
            if(rowIndex >= 0 && rowIndex < deminsion && colIndex >= 0 && rowIndex < deminsion )
            {
                if(rep[colIndex, rowIndex].team == team)
                {
                    return true;
                }
            }
            return false;
        }

        
        //checks to see if the move can be played
        //{@code source} ^ {@code destination} are uppercase and no spaces on sides
        private bool AvailableDestination(Team team, string source, string destination)
        {
            //check if destination is possible
            Team opposite = team == Team.Black ? Team.White : Team.Black;
            //check if destinaation is empty or opponents space
            if(AvailableSource(Team.None, destination) || AvailableSource(opposite, destination)){
                //check if piece can actually get there
                char []sourceArray = source.ToCharArray();
                int colSource = Convert.ToInt32(sourceArray[0]) - 65;
                int rowSource = Convert.ToInt32(source[1]) - 49;
                char[] destinationArray = destination.ToCharArray();
                int colDest = Convert.ToInt32(destinationArray[0]) - 65;
                int rowDest = Convert.ToInt32(destinationArray[1]) - 49;
                switch (rep[colSource, rowSource].worth)
                {
                    case Value.King:
                        return KingMove(colSource, rowSource, colDest, rowDest);
                    case Value.Queen:
                        return QueenMove(colSource, rowSource, colDest, rowDest);
                    case Value.Bishop:
                        return BishopMove(colSource, rowSource, colDest, rowDest);
                    case Value.Rook:
                        return RookMove(colSource, rowSource, colDest, rowDest);
                    case Value.Knight:
                        return KnightMove(colSource, rowSource, colDest, rowDest);
                    case Value.Pawn:
                        return PawnMove(colSource, rowSource, colDest, rowDest);
                }   
            }
            return false;
        }

        /// <summary>
        /// Checks if move is possible and does it
        /// </summary>
        /// <param name="team"> The team moving </param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// @requires source to be a team peice
        /// <returns>true if does move</returns>
        public bool ExecuteMove(Team team, string source, string destination)
        {
            //check for castling;
            //1. neither have moved
            //2. no peice between them
            //checks that destination is on same team
            bool executeMove = AvailableSource(team, destination);
            if (executeMove) {
                char[] sourceArray = source.ToCharArray();
                int colSource = Convert.ToInt32(sourceArray[0]) - 65;
                int rowSource = Convert.ToInt32(source[1]) - 49;
                char[] destinationArray = destination.ToCharArray();
                int colDest = Convert.ToInt32(destinationArray[0]) - 65;
                int rowDest = Convert.ToInt32(destinationArray[1]) - 49;
                executeMove = executeMove ? (!rep[colSource, rowSource].moved && !rep[colDest, rowDest].moved) : executeMove;
                executeMove = executeMove ? (rep[colSource, rowSource].worth == Value.King || rep[colDest, rowDest].worth == Value.King) : executeMove;
                if (Math.Abs(colSource - colDest) == 4) {
                    //left castling
                    for (int i = 1; i <= 3; i++) {
                        executeMove = executeMove ? rep[i, rowDest].worth == Value.Empty : executeMove;
                    }
                    if (executeMove) {
                        rep[2, rowDest] = new Space(Value.King, team);
                        rep[3, rowDest] = new Space(Value.Rook, team);
                        rep[colSource, rowSource] = new Space();
                        rep[colDest, rowDest] = new Space();
                    }
                } else { // right castling
                    for (int i = 5; i <= 6; i++) {
                        executeMove = executeMove ? rep[i, rowDest].worth == Value.Empty : executeMove;
                    }
                    if (executeMove) {
                        rep[6, rowDest] = new Space(Value.King, team);
                        rep[5, rowDest] = new Space(Value.Rook, team);
                        rep[colSource, rowSource] = new Space();
                        rep[colDest, rowDest] = new Space();
                    }
                }
            }
            
            if (!executeMove) {
                executeMove = AvailableDestination(team, source, destination);
                if (executeMove) {
                    char[] sourceArray = source.ToCharArray();
                    int colSource = Convert.ToInt32(sourceArray[0]) - 65;
                    int rowSource = Convert.ToInt32(source[1]) - 49;
                    char[] destinationArray = destination.ToCharArray();
                    int colDest = Convert.ToInt32(destinationArray[0]) - 65;
                    int rowDest = Convert.ToInt32(destinationArray[1]) - 49;
                    //place new piece in source
                    if (rep[colDest, rowDest].worth == Value.King) {
                        this.gameOver = true;
                    }
                    rep[colDest, rowDest] = new Space(rep[colSource, rowSource].worth, team);
                    //clear source
                    rep[colSource, rowSource] = new Space();
                    //check if pawn made it to the end
                    if (rep[colDest, rowDest].worth == Value.Pawn && ((rowDest == 0 && team == Team.Black) || (rowDest == 7 && team == Team.White))) {
                        bool correctInput = false;
                        do {
                            Console.Write("Enter 'Q' for Queen of 'K' for knight: ");
                            string peice = Console.ReadLine().Trim().ToUpper();
                            if (peice.Equals("Q")) {
                                rep[colDest, rowDest] = new Space(Value.Queen, team);
                                correctInput = true;
                            } else if (peice.Equals("K")) {
                                rep[colDest, rowDest] = new Space(Value.Knight, team);
                                correctInput = true;
                            }
                        } while (!correctInput);
                    }
                }
            }

            return executeMove;
        }


        ///all parameters are 0-7 and rep what they say they are
        ///returns true if knight can move the destination
        ///@requires the source int to point to a Knight of correct team
        ///@requires teh destination to be empty or opponent
        ///**remember knights move (2,1) or (1,2);
        private bool KnightMove(int colSource, int rowSource, int colDest, int rowDest)
        {
            //moves left or right 2
            if(Math.Abs(colSource - colDest) == 2)
            {
                //moves up or down one
                if(Math.Abs(rowSource - rowDest) == 1){
                    return true;
                }
            }
            //moves left or right 1
            else if(Math.Abs(colSource - colDest) == 1)
            {
                //moves up or down 2
                if(Math.Abs(rowSource - rowDest) == 2)
                {
                    return true;
                }
            }
            //all els or not possible
            return false;
        }


        /// <summary>
        /// determines if Rook can be moved to spot
        /// </summary>
        /// <param name="colSource"> column Rook is in</param>
        /// <param name="rowSource"> row Rook is in</param>
        /// <param name="colDest"> col Rook might go to</param>
        /// <param name="rowDest"> row Rook might go to</param>
        /// @requires source is a Rook
        /// @requires destinaion is anywhere on board and is empty or enemy
        /// <returns> true if move is possible; else false</returns>
        private bool RookMove(int colSource, int rowSource, int colDest,int rowDest)
        {
            bool rookMove = true;
            //vertical movement
            if (colSource == colDest)
            {
                //moving down
                if(rowSource < rowDest)
                {
                    for (int i = rowSource + 1; i < rowDest; i++) {
                        if (rookMove) {
                            rookMove = rep[colSource, i].team == Team.None;
                        }
                    }
                }
                //moving up
                if(rowSource > rowDest)
                {
                    for(int i = rowDest + 1; i < rowSource; i++)
                    {
                        if (rookMove)
                        {
                            rookMove = rep[colSource, i].team == Team.None;
                        }
                    }
                }
            }
            //horizontal move
            else if(rowDest == rowSource) {
                //moving right
                if(colSource < colDest) {
                    for(int i = colSource + 1; i < colDest; i++) {
                        if (rookMove) {
                            rookMove = rep[i, rowSource].team == Team.None;
                        }
                    }
                } //oving left
                else {
                    for(int i = colDest + 1; i < colSource; i++) {
                        if (rookMove) {
                            rookMove = rep[i, rowDest].team == Team.None;
                        }
                    }
                }
            } else {
                rookMove = false;
            }
            //need horizontal and diagonal cases
            return rookMove;
        }

        /// <summary>
        /// determines if bishop can be moves to spot
        /// </summary>
        /// <param name="colSource"></param>
        /// <param name="rowSource"></param>
        /// <param name="colDest"></param>
        /// <param name="rowDest"></param>
        /// @required destination is empty or enemy
        /// <returns>true if move possible; vice versa</returns>
        private bool BishopMove(int colSource, int rowSource, int colDest, int rowDest) {
            bool bishopMove = true;
            if(Math.Abs(colSource - colDest) == Math.Abs(rowSource - rowDest)) {
                //up
                if(rowSource > rowDest) {
                    //right
                    if(colSource < colDest) {
                        int row = rowSource - 1;
                        int col = colSource + 1;
                        while(row != rowDest && bishopMove) {
                            bishopMove = rep[col++, row--].team == Team.None;
                        }
                    } // left
                    else {
                        int row = rowSource - 1;
                        int col = colSource - 1;
                        while(row != rowDest && bishopMove) {
                            bishopMove = rep[col--, row--].team == Team.None;
                        }
                    }
                }//down
                else {
                    //right
                    if(colSource < colDest) {
                        int row = rowSource + 1;
                        int col = colSource + 1;
                        while(row != rowDest && bishopMove) {
                            bishopMove = rep[col++, row++].team == Team.None;
                        }
                    } //left
                    else {
                        int row = rowSource + 1;
                        int col = colSource - 1;
                        while(row != rowDest && bishopMove) {
                            bishopMove = rep[col--, row++].team == Team.None;
                        }
                    }
                }
            } else {
                bishopMove = false;
            }
            return bishopMove;
        }

        /// <summary>
        /// determines if teh queen can move to the spot
        /// </summary>
        /// <param name="colSource"></param>
        /// <param name="rowSource"></param>
        /// <param name="colDest"></param>
        /// <param name="rowDest"></param>
        /// @requires dest to be empty or enemy
        /// <returns>true if it can move</returns>
        private bool QueenMove(int colSource, int rowSource, int colDest, int rowDest) {
            return BishopMove(colSource, rowSource, colDest, rowDest) || RookMove(colSource, rowSource, colDest, rowDest);
        }

        /// <summary>
        /// determines if the king can move to spot
        /// </summary>
        /// <param name="colSource"></param>
        /// <param name="rowSource"></param>
        /// <param name="colDest"></param>
        /// <param name="rowDest"></param>
        /// @requires teh destination ot be empty or enemy
        /// <returns>true if it can move there</returns>
        private bool KingMove(int colSource, int rowSource, int colDest, int rowDest) {
            return (Math.Abs(colSource - colDest) <= 1) && (Math.Abs(rowSource - rowDest) <= 1);
        }

        /// <summary>
        /// determines if pawn can move
        /// </summary>
        /// <param name="colSource"></param>
        /// <param name="rowSource"></param>
        /// <param name="colDest"></param>
        /// <param name="rowDest"></param>
        /// @requirs the destination is empty or an enemy
        /// <returns>true if pawn can move</returns>
        private bool PawnMove(int colSource, int rowSource, int colDest, int rowDest) {
            bool pawnMove = true;
            //white
            if(rep[colSource,rowSource].team == Team.White) {
                //move down one
                if (colSource == colDest && rowDest - rowSource == 1) {
                    pawnMove = rep[colDest, rowDest].team == Team.None;
                } //move down two
                else if (colSource == colDest && rowDest - rowSource == 2) {
                    int row = rowSource + 1;
                    while (pawnMove && row <= rowDest) {
                        pawnMove = rep[colSource, row].team == Team.None;
                        row++;
                    }
                } //down and right (kill)
                else if (colDest - colSource == 1 && rowDest - rowSource == 1) {
                    pawnMove = rep[colDest, rowDest].team == Team.Black;
                }  //down and left (kill)
                else if (colDest - colSource == -1 && rowDest - rowSource == 1) {
                    pawnMove = rep[colDest, rowDest].team == Team.Black;
                } else {
                    pawnMove = false;
                }
            } //black
            else {
                //move up one
                if (colSource == colDest && rowDest - rowSource == -1) {
                    pawnMove = rep[colDest, rowDest].team == Team.None;
                } //move down two
                else if (colSource == colDest && rowDest - rowSource == -2) {
                    int row = rowSource - 1;
                    while (pawnMove && row >= rowDest) {
                        pawnMove = rep[colSource, row].team == Team.None;
                        row--;
                    }
                } //up and right (kill)
                else if (colDest - colSource == 1 && rowDest - rowSource == -1) {
                    pawnMove = rep[colDest, rowDest].team == Team.White;
                }  //up and left (kill)
                else if (colDest - colSource == -1 && rowDest - rowSource == -1) {
                    pawnMove = rep[colDest, rowDest].team == Team.White;
                } else {
                    pawnMove = false;
                }
            }
            return pawnMove;
        }



        public class Space {
            private Value _worth;
            private Team _team;
            public readonly bool moved;

            public Space(Value worth, Team team){
                _worth = worth;
                _team = team;
                moved = true;
            }

            /// <summary>
            /// makes an empty space
            /// </summary>
            public Space(){
                _worth = Value.Empty;
                _team = Team.None;
            }

            public Space(Value worth, Team team, bool Moved = false) {
                _worth = worth;
                _team = team;
                moved = Moved;
            }

            public Value worth {
                get { return _worth; }
                set { _worth = value; }
            }

            public Team team{
                get { return _team; }
                set { _team = value; }
            }
            
        }

        public enum Value{
            King, Queen, Rook, Knight, Bishop, Pawn, Empty
        };

        public enum Team {
            Black, White, None
        };

    }
}
