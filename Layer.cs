namespace NeuralNetwork
{
    public class Layer
    {
        Random random;
        int randomSeed = 1;

        bool outputFlag;
        public double[,] weights;
        public double[] biases;
        public double[] input;
        public double[] output;
        public int n_synapses;
        public int n_neurons;

        public Layer(int n_neurons, int n_synapses, bool outputFlag)
        {
            random = new Random(randomSeed);
            weights = new double[n_synapses, n_neurons];
            biases = new double[n_neurons];
            input = new double[n_synapses];
            output = new double[n_neurons];
            this.n_synapses = n_synapses;
            this.n_neurons = n_neurons;
            this.outputFlag = outputFlag;

            InitializeWeights();
            InitializeBiases();
        }

        void InitializeWeights()
        {
            for (int y = 0; y < n_synapses; y++)
            {
                for (int x = 0; x < n_neurons; x++)
                {
                    // generates a random double in range < -0.5;0.5 >
                    weights[y, x] = random.NextDouble() - 0.5;
                }
            }
        }

        void InitializeBiases()
        {
            for (int i = 0; i < n_neurons; i++)
            {
                // generates a random double in range < -1;1 >
                biases[i] = (2 * random.NextDouble()) - 1;
            }
        }

        public double[] Forward(double[] input)
        {
            this.input = input;
            
            for (int neuron = 0; neuron < n_neurons; neuron++)
            {   
                double neuronInput = 0;
                for (int synapse = 0; synapse < n_synapses; synapse++)
                {
                    neuronInput += input[synapse] * weights[synapse, neuron] + biases[neuron];
                }
                if (outputFlag)
                {
                    output[neuron] = ReLU(neuronInput);
                } else 
                {
                    output[neuron] = neuronInput;
                }
            }

            return output;
        }

        double ReLU(double x)
        {
            return Math.Max(0, x);
        }
    }
}