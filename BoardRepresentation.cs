namespace NeuralNetwork
{
    public class Board
    {
        public ulong W_Pawns = new ulong();
        public ulong B_Pawns = new ulong();
        public ulong W_Bishops = new ulong();
        public ulong B_Bishops = new ulong();
        public ulong W_Knights = new ulong();
        public ulong B_Knights = new ulong();
        public ulong W_Rooks = new ulong();
        public ulong B_Rooks = new ulong();
        public ulong W_Queens = new ulong();
        public ulong B_Queens = new ulong();
        public ulong W_King = new ulong();
        public ulong B_King = new ulong();

        // true = white, false = black
        public bool SideToPlay = new bool();
        public bool W_CastleQueenside = false;
        public bool B_CastleQueenside = false;
        public bool W_CastleKingside = false;
        public bool B_CastleKingside = false;

        // public ulong possibleEnPassant = new ulong();

        // how many moves since last pawn move or capture
        public int HalfMove = new int();
        public int FullMove = new int();

        public Board(string fen)
        {
            ParseFromFen(fen);
        }

        void ParseFromFen(string fen)
        {
            // r1bqk1nr/pppp1ppp/2n5/2P1p3/8/2N2N2/PPP1PPPP/R1BQKB1R w KQkq - 0 1
            
            string[] parts = fen.Split(" ");

            ParseBitboardsFromFen(parts[0]);
            
            SideToPlay = parts[1] == "w" ? true : false;

            if (parts[2].Contains('K')) 
            {
                W_CastleKingside = true;
            } else if (parts[2].Contains('Q'))
            {
                W_CastleQueenside = true;
            } else if (parts[2].Contains('k'))
            {
                B_CastleQueenside = true;
            } else if (parts[2].Contains('q'))
            {
                B_CastleQueenside = true;
            }

            HalfMove = Int32.Parse(parts[4]);
            FullMove = Int32.Parse(parts[5]);
        }

        void ParseBitboardsFromFen(string fen)
        {
            string curChar;
            int intOut;
            int idx = 56;
            int max_idx = idx + 7;

            for (int x = 0; x < fen.Length; x++)
            {

                curChar = fen[x].ToString();
                if (curChar == "/") { continue; }

                if (idx > max_idx)
                {
                    idx -= 16;
                    max_idx -= 8;
                }

                switch (curChar)
                {
                    case "p":
                        B_Pawns = SetBitTo1(B_Pawns, 63-idx);
                        break;
                    case "b":
                        B_Bishops = SetBitTo1(B_Bishops, 63-idx);
                        break;
                    case "n":
                        B_Knights = SetBitTo1(B_Knights, 63-idx);
                        break;
                    case "r":
                        B_Rooks = SetBitTo1(B_Rooks, 63-idx);
                        break;
                    case "q":
                        B_Queens = SetBitTo1(B_Queens, 63-idx);
                        break;
                    case "k":
                        B_King = SetBitTo1(B_King, 63-idx);
                        break;
                    case "P":
                        W_Pawns = SetBitTo1(W_Pawns, 63-idx);
                        break;
                    case "B":
                        W_Bishops = SetBitTo1(W_Bishops, 63-idx);
                        break;
                    case "N":
                        W_Knights = SetBitTo1(W_Knights, 63-idx);
                        break;
                    case "R":
                        W_Knights = SetBitTo1(W_Knights, 63-idx);
                        break;
                    case "Q":
                        W_Queens = SetBitTo1(W_Queens, 63-idx);
                        break;
                    case "K":
                        W_King = SetBitTo1(W_King, 63-idx);
                        break;
                    default:
                        break;
                }

                if (Int32.TryParse(curChar, out intOut))
                {
                    idx += intOut;
                } else
                {
                    idx += 1;
                }
            }
        }

        public void PPrintBitBoard(ulong bitboard)
        {
            string binaryString = GetBinaryString(FlipBitboardVertical(bitboard));

            for (int i = 0; i < binaryString.Length; i++)
            { 
                Console.Write(binaryString[i] + " ");
                if ((i + 1) % 8 == 0) 
                {
                    Console.Write("\n");
                }
            }
        }

        public string GetBinaryString(ulong n)
        {
            char[] b = new char[64];
            int pos = 63;
            int i = 0;

            while (i < 64)
            {
                if ((n & (1UL << i)) != 0)
                    b[pos] = '1';
                else
                    b[pos] = '0';
                pos--;
                i++;
            }
            return new string(b);
        }

        ulong FlipBitboardVertical(ulong x) {
            return  ( x << 56 ) |
                    ( x << 40 & 0x00ff000000000000 ) |
                    ( x << 24 & 0x0000ff0000000000 ) |
                    ( x <<  8 & 0x000000ff00000000 ) |
                    ( x >>  8 & 0x00000000ff000000 ) |
                    ( x >> 24 & 0x0000000000ff0000 ) |
                    ( x >> 40 & 0x000000000000ff00 ) |
                    ( x >> 56 );
        }

        ulong SetBitTo1(ulong value, int position)
        {
            // Set a bit at position to 1.
            return value |= (1UL << position);
        }

        ulong SetBitTo0(ulong value, int position)
        {
            // Set a bit at position to 0.
            return value & ~(1UL << position);
        }
    }
}