namespace NeuralNetwork
{
    class Program
    {
        static (List<double[]>, List<double[]>) PrepareDataset(int n_items, int item_size)
        {
            Random R = new Random();
            List<double[]> items = new List<double[]>();
            List<double[]> labels = new List<double[]>();

            for (int i = 0; i < n_items; i++)
            {
                double[] item = new double[item_size];
                double[] label = new double[1];
                int count = 0;

                for (int l = 0; l < item_size; l++)
                {
                    item[l] = R.Next(-20, 21);
                    count += (int)item[l];
                }
                label[0] = count;

                items.Add(item);
                labels.Add(label);
            }

            return (items, labels);
        }

        static void Main()
        {
            // (List<double[]>, List<double[]>) data = PrepareDataset(50, 5);
            // List<double[]> trainX = data.Item1;
            // List<double[]> trainY = data.Item2;

            List<double[]> trainX = new List<double[]>(){
                new double[]{5, 4, 6, -1, 2}
            };

            List<double[]> trainY = new List<double[]>(){
                new double[]{16}
            };

            NeuralNetwork NN = new NeuralNetwork(5);

            NN.Train(10, 0.001, trainX, trainY);


            // --------------------------------- BITBOARDS BLABLA --------------------------------------

            // string fen = "r1bqk1nr/pppp1ppp/2n5/2P1p3/8/2N2N2/PPP1PPPP/R1BQKB1R w KQkq - 0 1";
            // string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

            // Board b = new Board(fen);

            // ulong B_Pieces = b.B_Bishops | b.B_King | b.B_Knights | b.B_Queens | b.B_Rooks | b.B_Pawns;
            // ulong W_Pieces = b.W_Bishops | b.W_King | b.W_Knights | b.W_Queens | b.W_Rooks | b.W_Pawns;
            // ulong all_Pieces = W_Pieces | B_Pieces;

            // Console.WriteLine();
            // b.PPrintBitBoard(b.B_Bishops);
        }
    }
}