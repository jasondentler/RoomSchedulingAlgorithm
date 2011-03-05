using System;
using System.Diagnostics;
using System.IO;

namespace AlgorithmRunner
{
    class Program
    {

        static Program()
        {
            var workingDirectory = Process.GetCurrentProcess().StartInfo.WorkingDirectory;
            RawDataDirectory = Path.Combine(workingDirectory, @"..\..\..\RawData");
        }

        private static readonly string RawDataDirectory;

        static void Main(string[] args)
        {
            var p = new Program();
            p.BuildTimePatterns();
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        private void BuildConflictWeightFile()
        {
            var weights = new ConflictWeightsGenerator(
                Path.Combine(RawDataDirectory, "Sections.Fall2010.xml"),
                Path.Combine(RawDataDirectory, "Sections.xml"),
                Path.Combine(RawDataDirectory, "StudentSections.Fall2010.xml"));
            weights.Generate(Path.Combine(RawDataDirectory, "ConflictWeights.xml"));
        }

        private void BuildTimePatterns()
        {
            var loader = new TimePatternLoader(
                Path.Combine(RawDataDirectory, "SectionMeetings.xml"));
            var sw = loader.Load();
            File.WriteAllText(Path.Combine(RawDataDirectory, "Patterns.txt"), sw.ToString());
        }

    }
}
