using System.IO;

namespace AlgorithmRunner
{
    class Program
    {

        private const string RawData = @"C:\Projects\Room Scheduling Algorithm\RawData";

        static void Main(string[] args)
        {
            var p = new Program();
            p.BuildConflictWeightFile();
        }

        private void BuildConflictWeightFile()
        {
            var weights = new ConflictWeightsGenerator(
                Path.Combine(RawData, "Sections.Fall2010.xml"),
                Path.Combine(RawData, "Sections.xml"),
                Path.Combine(RawData, "StudentSections.Fall2010.xml"));
            weights.Generate(Path.Combine(RawData, "ConflictWeights.xml"));
        }

    }
}
