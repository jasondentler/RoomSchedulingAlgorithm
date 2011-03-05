using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
            p.BuildTimeslots();
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

        private void BuildTimeslots()
        {
            var roomLoader = new RoomLoader(Path.Combine(RawDataDirectory, "Rooms.xml"));
            var rooms = roomLoader.Load().OrderBy(r => r.RoomNumber);

            var patternGenerator = new TimePatternGenerator();
            var patterns = patternGenerator.Generate();

            var slotGenerator = new TimeslotGenerator(rooms, patterns);
            var slots = slotGenerator.Generate();

            foreach (var slot in slots)
                Console.WriteLine(slot.ToString());
        }

    }
}
