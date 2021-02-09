using System.ComponentModel;

namespace ClassLibrary1
{
    [DisplayName("Settings")]
    public sealed class Settings
    {
        public static readonly Settings Current = new Settings();

        private Settings()
        {
        }

        [Description("Settings used to evolve a strategy")]
        public GeneticSettings GeneticSettings { get; } = new GeneticSettings();

        [Description("Settings used to test a strategy")]
        public TestSettings TestSettings { get; } = new TestSettings();
    }

    [DisplayName("Genetic Settings")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public sealed class GeneticSettings
    {
        [Description("Minumum number of candidates per generation")]
        public int MinPopulationSize { get; set; } = 500;

        [Description("Maximum number of candidates per generation")]
        public int MaxPopulationSize { get; set; } = 1000;

        [Description("Type of selection used to select parents for crossover")]
        public SelectionType SelectionType { get; set; } = SelectionType.Tournament;

        [Description("If using Tournament selection, how many to select")]
        public int TournamentSize { get; set; } = 3;

        [Description("Type of crossover used")]
        public CrossoverType CrossoverType { get; set; } = CrossoverType.Uniform;

        [Description("If using Uniform crossover, the probability offspring will have the genes of its parent")]
        public float MixProbability { get; set; } = 0.5f;

        [Description("The probability crossover will occur")]
        public float CrossoverProbability { get; set; } = 0.75f;

        [Description("The probability mutation will occur")]
        public float MutationProbability { get; set; } = 0.1f;

        [Description("The expected number of stagnant generations (i.e. no change in fitness) to reach the termination.")]
        public int NumStagnantGenerations { get; set; } = 25;

        [Description("Save a PNG image for each evolved generation")]
        public bool SaveImagePerGeneration { get; set; } = false;

        public override string ToString()
        {
            return string.Empty;
        }
    }

    [DisplayName("Test Settings")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public sealed class TestSettings
    {
        [Description("Bet amount per hand")]
        public double BetAmount { get; set; } = 2;

        [Description("Payout for a player blackjack")]
        public double BlackjackPayout { get; set; } = 1.5;

        [Description("Number of decks in the shoe")]
        public int NumDecks { get; set; } = 6;

        [Description("Number of rounds played per test")]
        public int NumRounds { get; set; } = 100000;

        [Description("Number of test runs")]
        public int NumTests { get; set; } = 100;

        public override string ToString()
        {
            return string.Empty;
        }
    }

    public enum SelectionType
    {
        Elite,
        RouletteWheel,
        StochasticUniversalSampling,
        Tournament
    }

    public enum CrossoverType
    {
        AlternatingPosition,
        CutAndSplice,
        Cycle,
        OnePoint,
        TwoPoint,
        OrderBased,
        Ordered,
        PartiallyMapped,
        PositionBased,
        ThreeParent,
        Uniform,
        VotingRecombination
    }
}
