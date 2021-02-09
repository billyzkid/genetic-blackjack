using ClassLibrary1;
using ClassLibrary1.Genetics;
using ClassLibrary1.Models;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Infrastructure.Framework.Threading;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private IStrategy geneticStrategy;
        private IStrategy basicStrategy;

        public MainWindow()
        {
            InitializeComponent();
            PropertyGrid.ExpandAllProperties();
        }

        public Settings Settings { get; } = Settings.Current;

        private void EvolveGeneticStrategyButton_Click(object sender, RoutedEventArgs e)
        {
            OutputTextBlock.Text = "Evolving...";

            Task.Run(() =>
            {
                var chromosome = new BlackjackChromosome();
                var fitness = new BlackjackFitness();
                var population = new Population(Settings.GeneticSettings.MinPopulationSize, Settings.GeneticSettings.MaxPopulationSize, chromosome);

                ISelection selection;

                switch (Settings.GeneticSettings.SelectionType)
                {
                    case SelectionType.Elite:
                        selection = new EliteSelection();
                        break;

                    case SelectionType.RouletteWheel:
                        selection = new RouletteWheelSelection();
                        break;

                    case SelectionType.StochasticUniversalSampling:
                        selection = new StochasticUniversalSamplingSelection();
                        break;

                    case SelectionType.Tournament:
                        selection = new TournamentSelection(Settings.GeneticSettings.TournamentSize);
                        break;

                    default:
                        throw new InvalidOperationException();
                }

                ICrossover crossover;

                switch (Settings.GeneticSettings.CrossoverType)
                {
                    case CrossoverType.AlternatingPosition:
                        crossover = new AlternatingPositionCrossover();
                        break;

                    case CrossoverType.CutAndSplice:
                        crossover = new CutAndSpliceCrossover();
                        break;

                    case CrossoverType.Cycle:
                        crossover = new CycleCrossover();
                        break;

                    case CrossoverType.OnePoint:
                        crossover = new OnePointCrossover();
                        break;

                    case CrossoverType.TwoPoint:
                        crossover = new TwoPointCrossover();
                        break;

                    case CrossoverType.OrderBased:
                        crossover = new OrderBasedCrossover();
                        break;

                    case CrossoverType.Ordered:
                        crossover = new OrderedCrossover();
                        break;

                    case CrossoverType.PartiallyMapped:
                        crossover = new PartiallyMappedCrossover();
                        break;

                    case CrossoverType.PositionBased:
                        crossover = new PositionBasedCrossover();
                        break;

                    case CrossoverType.ThreeParent:
                        crossover = new ThreeParentCrossover();
                        break;

                    case CrossoverType.Uniform:
                        crossover = new UniformCrossover(Settings.Current.GeneticSettings.MixProbability);
                        break;

                    case CrossoverType.VotingRecombination:
                        crossover = new VotingRecombinationCrossover();
                        break;

                    default:
                        throw new InvalidOperationException();
                }

                var mutation = new UniformMutation();
                var termination = new FitnessStagnationTermination(Settings.Current.GeneticSettings.NumStagnantGenerations);
                var taskExecutor = new ParallelTaskExecutor();

                var ga = new GeneticAlgorithm(
                    population,
                    fitness,
                    selection,
                    crossover,
                    mutation);

                ga.Termination = termination;
                ga.TaskExecutor = taskExecutor;
                ga.MutationProbability = Settings.GeneticSettings.MutationProbability;
                ga.CrossoverProbability = Settings.GeneticSettings.CrossoverProbability;
                
                var latestFitness = double.MinValue;

                ga.GenerationRan += (s, o) =>
                {
                    geneticStrategy = (IStrategy)ga.BestChromosome;

                    var generationNumber = ga.GenerationsNumber;
                    var bestFitness = ga.BestChromosome.Fitness.Value;
                    var avgFitness = ga.Population.CurrentGeneration.Chromosomes.Average(c => c.Fitness.Value);

                    Dispatcher.Invoke(() =>
                    {
                        if (generationNumber == 1)
                        {
                            OutputTextBlock.Text = string.Empty;
                        }

                        OutputTextBlock.Text = $"Gen: {generationNumber}\tFit: {bestFitness}\tAvg: {avgFitness.ToString("0")}\n" + OutputTextBlock.Text;

                        if (bestFitness != latestFitness)
                        {
                            latestFitness = bestFitness;

                            var savedImageName = Settings.Current.GeneticSettings.SaveImagePerGeneration ? "gen" + generationNumber : null;
 
                            StrategyViewer.Draw(GeneticStrategyCanvas, geneticStrategy, $"Best from generation {generationNumber}", savedImageName);
                        }
                    }, DispatcherPriority.Background);
                };

                ga.TerminationReached += (s, o) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        OutputTextBlock.Text = "Termination reached.\n" + OutputTextBlock.Text;
                        TestGeneticStrategyButton.IsEnabled = true;
                    }, DispatcherPriority.Background);
                };

                ga.Start();
            });
        }

        private void TestGeneticStrategyButton_Click(object sender, RoutedEventArgs e)
        {
            TestStrategy(geneticStrategy);
        }

        private void ShowBasicStrategyButton_Click(object sender, RoutedEventArgs e)
        {
            basicStrategy = new BasicStrategy();

            StrategyViewer.Draw(BasicStrategyCanvas, basicStrategy, string.Empty, null);

            TestBasicStrategyButton.IsEnabled = true;
        }

        private void TestBasicStrategyButton_Click(object sender, RoutedEventArgs e)
        {
            TestStrategy(basicStrategy);
        }

        private void TestStrategy(IStrategy strategy)
        {
            OutputTextBlock.Text = "Testing...";

            Task.Run(() =>
            {
                double avg, stdDev, coeffVariation;
                StrategyTester.GetStatistics(strategy, out avg, out stdDev, out coeffVariation);

                Dispatcher.Invoke(() =>
                {
                    OutputTextBlock.Text = $"Test results:\nAverage score: {avg.ToString("0")}\nStandard deviation: {stdDev.ToString("0")}\nCoefficient of variation: {coeffVariation.ToString("0.0000")}";
                }, DispatcherPriority.Background);
            });
        }
    }
}
