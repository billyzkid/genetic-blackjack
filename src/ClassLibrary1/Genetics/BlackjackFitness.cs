using ClassLibrary1.Models;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;

namespace ClassLibrary1.Genetics
{
    public sealed class BlackjackFitness : IFitness
    {
        public double Evaluate(IChromosome chromosome)
        {
            var strategy = (BlackjackChromosome)chromosome;

            return StrategyTester.Test(strategy);
        }
    }
}
