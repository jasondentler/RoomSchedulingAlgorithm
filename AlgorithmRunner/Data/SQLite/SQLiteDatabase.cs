using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace AlgorithmRunner.Data.SQLite
{
    public class SQLiteDatabase
        : Database
    {

        static SQLiteDatabase()
        {
            Resources = new ConcurrentDictionary<string, string>();
        }

        public SQLiteDatabase()
            : base("db")
        {
        }

        private static readonly ConcurrentDictionary<string, string> Resources;

        protected virtual void LoadCommandText(IDbCommand command, string resourceName)
        {
            command.CommandText = Resources.GetOrAdd(resourceName,
                                                     rs =>
                                                     {
                                                         var asm = this.GetType().Assembly;
                                                         var ns = this.GetType().Namespace;
                                                         var qualifiedResourceName =
                                                             string.Format("{0}.{1}", ns, resourceName);
                                                         using (
                                                             var strm =
                                                                 asm.GetManifestResourceStream(
                                                                     qualifiedResourceName))
                                                         using (var rdr = new StreamReader(strm))
                                                         {
                                                             return rdr.ReadToEnd();
                                                         }
                                                     });
        }

        protected virtual void AddParam(IDbCommand command, string name, object value)
        {
            var param = command.CreateParameter();
            param.ParameterName = name;
            param.Value = value;
            command.Parameters.Add(param);
        }

        protected virtual void Execute(IDbCommand command)
        {
            command.ExecuteNonQuery();
        }

        protected virtual void Execute(Func<IDbCommand> commandFactory, string resourceName)
        {
            using (var command = commandFactory())
            {
                LoadCommandText(command, resourceName);
                Execute(command);
            }
        }

        protected virtual void Execute(Func<IDbCommand> commandFactory, string resourceName, IDictionary<string, object> parameters)
        {
            using (var command = commandFactory())
            {
                LoadCommandText(command, resourceName);
                foreach (var param in parameters)
                    AddParam(command, param.Key, param.Value);
                Execute(command);
            }
        }

        protected override void CreateTableInstructors(Func<IDbCommand> commandFactory)
        {
            Execute(commandFactory, "CreateTableInstructors.sql");
        }

        protected override void CreateTableSections(Func<IDbCommand> commandFactory)
        {
            Execute(commandFactory, "CreateTableSections.sql");
        }

        protected override void CreateTableSectionEquipment(Func<IDbCommand> commandFactory)
        {
            Execute(commandFactory, "CreateTableSectionEquipment.sql");
        }

        protected override void CreateTableRooms(Func<IDbCommand> commandFactory)
        {
            Execute(commandFactory, "CreateTableRooms.sql");
        }

        protected override void CreateTableRoomEquipment(Func<IDbCommand> commandFactory)
        {
            Execute(commandFactory, "CreateTableRoomEquipment.sql");
        }

        protected override void CreateTableTimePatterns(Func<IDbCommand> commandFactory)
        {
            Execute(commandFactory, "CreateTableTimePatterns.sql");
        }

        protected override void CreateTableChoices(Func<IDbCommand> commandFactory)
        {
            Execute(commandFactory, "CreateTableChoices.sql");
            Execute(commandFactory, "CreateIndexSectionRoom.sql");
            Execute(commandFactory, "CreateIndexRoomTimePattern.sql");
        }

        protected override void InsertSection(Func<IDbCommand> commandFactory, string sectionId, short? capacity)
        {
            Execute(commandFactory, "InsertSection.sql",
                    new Dictionary<string, object>()
                        {
                            {"ExternalSectionId", sectionId},
                            {
                                "Capacity", capacity.HasValue
                                                ? (object) capacity.Value
                                                : DBNull.Value
                                }
                        });
        }

        protected override void InsertSectionEquipment(Func<IDbCommand> commandFactory, string sectionId, string equipment)
        {
            Execute(commandFactory, "InsertSectionEquipment.sql",
                    new Dictionary<string, object>()
                        {
                            {"ExternalSectionId", sectionId},
                            {"Equipment", equipment}
                        });
        }

        protected override void UpdateSectionInstructor(Func<IDbCommand> commandFactory, string sectionId, string instructorId)
        {
            Execute(commandFactory, "UpdateSectionInstructor.sql",
                    new Dictionary<string, object>()
                        {
                            {"ExternalSectionId", sectionId},
                            {"ExternalInstructorId", instructorId}
                        });
        }

        protected override void InsertRoom(Func<IDbCommand> commandFactory, string roomId, short? capacity)
        {
            Execute(commandFactory, "InsertRoom.sql",
                    new Dictionary<string, object>()
                        {
                            {"ExternalRoomId", roomId},
                            {
                                "Capacity", capacity.HasValue
                                                ? (object) capacity.Value
                                                : DBNull.Value
                                }
                        });
        }

        protected override void InsertRoomEquipment(Func<IDbCommand> commandFactory, string roomId, string equipment)
        {
            Execute(commandFactory, "InsertRoomEquipment.sql",
                new Dictionary<string, object>()
                    {
                        {"ExternalRoomId", roomId},
                        {"Equipment", equipment}
                    });
        }

        protected override void InsertTimePattern(Func<IDbCommand> commandFactory, byte days, DateTime start, DateTime end)
        {
            Execute(commandFactory, "InsertTimePattern.sql",
                    new Dictionary<string, object>()
                        {
                            {"Days", days},
                            {"StartTime", start},
                            {"EndTime", end},
                        });
        }

        protected override void BuildChoices(Func<IDbCommand> commandFactory)
        {
            Execute(commandFactory, "BuildChoices.sql");
        }

        protected override void FilterChoicesBasedOnCapacity(Func<IDbCommand> commandFactory)
        {
            Execute(commandFactory, "FilterChoicesBasedOnCapacity.sql");
        }

        protected override void FilterChoicesBasedOnEquipment(Func<IDbCommand> commandFactory)
        {
            Execute(commandFactory, "FilterChoicesBasedOnEquipment.sql");
        }

        protected override long CountRows(Func<IDbCommand> commandFactory, string tableName)
        {
            using (var command = commandFactory())
            {
                command.CommandText = string.Format("SELECT Count(*) FROM {0}", tableName);
                var result = command.ExecuteScalar();
                return Convert.ToInt64(result);
            }
        }


    }
}
