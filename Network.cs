using System.Linq;

namespace NeuralNetwork
{
    public class NeuralNetwork
    {
        List<Layer> layers = new List<Layer>();
        double learning_rate;

        public NeuralNetwork(int n_inputs)
        {
            layers.Add(new Layer(256, n_inputs, false));
            layers.Add(new Layer(32, 256, false));
            layers.Add(new Layer(1, 32, true));
        }

        public double[] Predict(double[] trainX)
        {          
            return Forwardpropagate(trainX);
        }

        public void Train(int n_epochs, double learning_rate, List<double[]> trainX, List<double[]> trainY)
        {
            if (trainX.Count != trainY.Count) 
            {
                Console.WriteLine("Inputs and labels dont match");
                return;
            }

            this.learning_rate = learning_rate;

            for (int epoch = 0; epoch < n_epochs; epoch++)
            {
                double[] outputs = new double[trainX.Count];
                double[] losses = new double[trainX.Count];

                for (int item = 0; item < trainX.Count; item++)
                {
                    (double, double) stats = Step(trainX[item], trainY[item]);
                    outputs[item] = stats.Item1;
                    losses[item] = stats.Item2;
                }
                double avgLoss = CalculateAverageLoss(losses);
                Console.WriteLine("Epoch #"+(epoch+1).ToString()+" | Average loss: "+avgLoss.ToString());
            }
        }

        (double, double) Step(double[] trainX, double[] trainY)
        {
            double[] predY = Forwardpropagate(trainX);
            Backpropagate(predY, trainY);

            // double loss = MeanSquaredError();
            double outputError = trainY[0] - predY[0];
            return (predY[0], outputError);
        }

        double[] Forwardpropagate(double[] trainX)
        {
            double[] output = trainX;
            for (int x = 0; x < layers.Count; x++)
            {
                output = layers[x].Forward(output);
            }
            return output;
        }

        // ------------------- BACKPROPAGATION --------------------

        void Backpropagate(double[] output, double[] trainY)
        {
            double[] outputDeltas = OutputDeltas(output, trainY);
            double[] deltas = new double[layers[layers.Count-2].n_neurons];

            for (int l = layers.Count-2; l >= 0; l--)
            {
                if (l == layers.Count-2)
                {
                    deltas = HiddenDeltas(layers[l].output, layers[l+1].weights, outputDeltas);

                } else
                {
                    deltas = HiddenDeltas(layers[l].output, layers[l+1].weights, deltas);
                }
                double[,] weightGradient = WeightGradient(layers[l].input, deltas);
                SetWeightUpdate(weightGradient, l);

                SetBiasUpdate(deltas, l);
            }
        }
        
        double[] OutputDeltas(double[] output, double[] correct)
        {   
            int n_neurons = output.Length;
            double[] deltas = new double[n_neurons];
            double error;
            double deriv;

            for (int neuron = 0; neuron < n_neurons; neuron++)
            {
                error = correct[neuron] - output[neuron];
                deriv = ReLUDerivative(output[neuron]);
                deltas[neuron] = error * deriv;
            }

            return deltas;
        }

        double[] HiddenDeltas(double[] output, double[,] weightsNextLayer, double[] deltasNextLayer)
        {
            int n_neurons = output.Length;
            double[] deltas = new double[n_neurons];
            double sumWeightsAndDeltasNextLayer;
            double deriv;

            for (int neuron = 0; neuron < n_neurons; neuron++)
            {
                sumWeightsAndDeltasNextLayer = SumWeightsAndDeltasNextLayer(weightsNextLayer, deltasNextLayer, neuron);
                deriv = ReLUDerivative(output[neuron]);
                deltas[neuron] = sumWeightsAndDeltasNextLayer * deriv;
            }

            return deltas;
        }

        double SumWeightsAndDeltasNextLayer(double[,] weightsNextLayer, double[] deltasNextLayer, int neuronPrevLayer)
        {
            double sum = 0;

            for (int neuronNextLayer = 0; neuronNextLayer < weightsNextLayer.GetLength(1); neuronNextLayer++)
            {
                sum += deltasNextLayer[neuronNextLayer] * weightsNextLayer[neuronPrevLayer, neuronNextLayer];
            }

            return sum;
        }

        double[,] WeightGradient(double[] input, double[] deltasThisLayer)
        {
            int n_synapses = input.Length;
            int n_neurons = deltasThisLayer.Length;
            double[,] gradient = new double[n_synapses, n_neurons];

            for (int synapse = 0; synapse < n_synapses; synapse++)
            {
                for (int neuron = 0; neuron < n_neurons; neuron++)
                {
                    gradient[synapse, neuron] = input[synapse] * deltasThisLayer[neuron];
                }
            }

            return gradient;
        }

        void SetBiasUpdate(double[] deltasThisLayer, int layer)
        {
            for (int neuron = 0; neuron < layers[layer].n_neurons; neuron++)
            {
                layers[layer].biases[neuron] -= deltasThisLayer[neuron] * learning_rate;
            }
        }

        void SetWeightUpdate(double[,] weightUpdate, int layer)
        {
            for (int synapse = 0; synapse < layers[layer].n_synapses; synapse++)
            {
                for (int neuron = 0; neuron < layers[layer].n_neurons; neuron++)
                {
                    layers[layer].weights[synapse, neuron] -= weightUpdate[synapse, neuron] * learning_rate;
                }
            }
        }

        double ReLUDerivative(double value) 
        {
            return value < 0 ? 0 : 1;
        }

        // --------------------------------------------------------

        double CalculateAverageLoss(double[] losses)
        {
            return losses.Sum() / losses.Length;
        }

        // double MSE(double[] output)
        // {
        //     // TODO: mean squared error
        // }
    }
}