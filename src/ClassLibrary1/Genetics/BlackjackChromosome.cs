using ClassLibrary1.Models;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;

namespace ClassLibrary1.Genetics
{
    public sealed class BlackjackChromosome : ChromosomeBase, IStrategy
    {
        public BlackjackChromosome() : base(340)
        {
            for (var i = 0; i < Length; i++)
            {
                ReplaceGene(i, GenerateGene(i));
            }
        }

        public override Gene GenerateGene(int geneIndex)
        {
            const int PAIRS_INDEX = 240;

            int action;

            if (geneIndex < PAIRS_INDEX)
            {
                // stand, hit, or double
                action = RandomizationProvider.Current.GetInt(0, 3);
            }
            else
            {
                // stand, hit, double, or split
                action = RandomizationProvider.Current.GetInt(0, 4);
            }

            return new Gene(action);
        }

        public override IChromosome CreateNew()
        {
            return new BlackjackChromosome();
        }

        public Action GetAction(Hand playerHand, Hand dealerHand)
        {
            var index = this.GetActionIndex(playerHand, dealerHand);

            return (Action)GetGene(index).Value;
        }
    }
}
