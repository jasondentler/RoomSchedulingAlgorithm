using System;
using System.Diagnostics;
using System.IO;
using AlgorithmRunner.ConflictWeights;
using AlgorithmRunner.Data;
using AlgorithmRunner.Data.SQLite;

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
            p.PopulateDatabase();
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

        private void PopulateDatabase()
        {
            var db = new SQLiteDatabase();

            Time(db.Initialize);

            var sectionLoader = new SectionLoader(
                Path.Combine(RawDataDirectory, "Sections.xml"), db);
            Time(sectionLoader.Load);
            
            var instructorLoader = new InstructorLoader(
                Path.Combine(RawDataDirectory, "SectionFaculty.xml"), db);
            Time(instructorLoader.Load);

            var roomLoader = new RoomLoader(
                Path.Combine(RawDataDirectory, "Rooms.xml"), db);
            Time(roomLoader.Load);
            
            var timePatternGenerator = new TimePatternGenerator(db);
            Time(timePatternGenerator.Generate);

            Time(db.BuildChoices);
            Time(db.FilterChoicesBasedOnCapacity);
            Time(db.FilterChoicesBasedOnEquipment);
        }

        private void Time(Action action)
        {
            var sw = new Stopwatch();
            sw.Start();
            action();
            sw.Stop();
            if (sw.Elapsed.TotalSeconds >= 1.0)
            {
                Console.WriteLine("Operation took {0}", sw.Elapsed);
                Console.WriteLine();
            }
        }

    }
}
