using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AlgorithmRunner.ConflictWeights;
using AlgorithmRunner.Entities;
using AlgorithmRunner.Indexers;

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
            p.BuildMatrix();
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

        private void BuildMatrix()
        {
            var roomLoader = new RoomLoader(Path.Combine(RawDataDirectory, "Rooms.xml"));
            var rooms = roomLoader.Load().OrderBy(r => r.RoomNumber).ToArray();
            Console.WriteLine("{0} rooms loaded", rooms.Count());

            var patternGenerator = new TimePatternGenerator();
            var patterns = patternGenerator.Generate().Where(p => p.Start.Hour < 12).ToArray();
            Console.WriteLine("{0} time patterns generated", patterns.Count());

            var slotGenerator = new RoomPatternJoiner(rooms, patterns);
            var slots = slotGenerator.Generate().ToArray();
            Console.WriteLine("{0} time slots created.", slots.Count());

            var instructorLoader = new InstructorLoader(
                Path.Combine(RawDataDirectory, "Faculty.xml"));
            var instructorMap = instructorLoader.Load();

            var FFaculty = instructorMap
                .Where(e => e.Value.FirstName.StartsWith("F") && e.Value.LastName == "Faculty");

            while (FFaculty.Any())
            {
                instructorMap.Remove(FFaculty.First());
            }

            Console.WriteLine("{0} instructors loaded.",
                              instructorMap.Count);
            
            var sectionLoader = new SectionLoader(
                Path.Combine(RawDataDirectory, "Sections.xml"));
            var sectionMap = sectionLoader.Load();
            Console.WriteLine("{0} sections loaded",
                              sectionMap.Count);

            var sectionInstructorJoiner = new SectionInstructorJoiner(
                Path.Combine(RawDataDirectory, "SectionFaculty.xml"),
                sectionMap, instructorMap);
            sectionInstructorJoiner.Load();

            var sections = sectionMap.Values.Distinct()
                .Where(section => new[] {"ENGL", "MATH", "HIST"}.Any(subject => section.Name.StartsWith(subject)))
                .ToArray();

            var sectionSlotJoiner = new SectionSlotJoiner(sections, slots);
            var sectionSlots = sectionSlotJoiner.Generate().ToArray();

            Console.WriteLine("{0} valid section/slot combinations for {1} sections and {2} slots",
                sectionSlots.Count(), sections.Count(), slots.Count());

            FindImpossibleSections(sections, sectionSlots);

            var conflictIndexGenerator = new ConflictIndexGenerator(sectionSlots);
            var conflictIndex = conflictIndexGenerator.Generate().ToArray();

        }

        private void FindImpossibleSections(IEnumerable<Section> sections, IEnumerable<SectionSlot> sectionSlots)
        {
            var possibleSections = sectionSlots.Select(ss => ss.Section).Distinct().ToArray();
            var impossibleSections = sections.Except(possibleSections);
            
            if (impossibleSections.Any())
            {
                Console.WriteLine("{0} sections removed from preprocessor because no suitable slot could be found",
                    impossibleSections.Count());
                foreach (var section in impossibleSections)
                    Console.WriteLine(section.Name);
            }
        }

    }
}
