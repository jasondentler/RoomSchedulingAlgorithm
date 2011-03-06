using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Xml.Linq;
using AlgorithmRunner.Entities;

namespace AlgorithmRunner.Data
{

    public abstract class Database
    {
        private readonly string _connectionString;
        private readonly DbProviderFactory _factory;
        private IDbConnection _connection;

        protected Database(string connectionStringName)
            : this(ConfigurationManager.ConnectionStrings[connectionStringName])
        {
        }

        protected Database(ConnectionStringSettings connStrSettings)
            : this(connStrSettings.ConnectionString, connStrSettings.ProviderName)
        {
        }

        protected Database(string connectionString, string providerName)
            : this(connectionString, DbProviderFactories.GetFactory(providerName))
        {
        }

        protected Database(string connectionString, DbProviderFactory factory)
        {
            _connectionString = connectionString;
            _factory = factory;
        }

        public virtual void Open()
        {
            Close();
            _connection = _factory.CreateConnection();
            _connection.ConnectionString = _connectionString;
            _connection.Open();
        }

        public virtual void Initialize()
        {
            Open();
            CreateTableInstructors(() => _connection.CreateCommand());
            CreateTableSections(() => _connection.CreateCommand());
            CreateTableSectionEquipment(() => _connection.CreateCommand());
            CreateTableRooms(() => _connection.CreateCommand());
            CreateTableRoomEquipment(() => _connection.CreateCommand());
            CreateTableTimePatterns(() => _connection.CreateCommand());
            CreateTableChoices(() => _connection.CreateCommand());
        }

        public virtual void Close()
        {
            if (_connection != null &&
                _connection.State != ConnectionState.Closed)
                _connection.Close();
            _connection = null;
        }

        protected abstract void CreateTableInstructors(Func<IDbCommand> commandFactory);
        protected abstract void CreateTableSections(Func<IDbCommand> commandFactory);
        protected abstract void CreateTableSectionEquipment(Func<IDbCommand> commandFactory);
        protected abstract void CreateTableRooms(Func<IDbCommand> commandFactory);
        protected abstract void CreateTableRoomEquipment(Func<IDbCommand> commandFactory);
        protected abstract void CreateTableTimePatterns(Func<IDbCommand> commandFactory);
        protected abstract void CreateTableChoices(Func<IDbCommand> commandFactory);

        protected abstract void InsertSection(Func<IDbCommand> commandFactory, string sectionId, short? capacity);
        protected abstract void InsertSectionEquipment(Func<IDbCommand> commandFactory, string sectionId, string equipment);
        protected abstract void UpdateSectionInstructor(Func<IDbCommand> commandFactory, string sectionId, string instructorId);

        protected abstract void InsertRoom(Func<IDbCommand> commandFactory, string roomId, short? capacity);
        protected abstract void InsertRoomEquipment(Func<IDbCommand> commandFactory, string roomId, string equipment);

        protected abstract void InsertTimePattern(Func<IDbCommand> commandFactory, byte days, DateTime start, DateTime end);

        protected abstract void BuildChoices(Func<IDbCommand> commandFactory);
        protected abstract void FilterChoicesBasedOnCapacity(Func<IDbCommand> commandFactory);
        protected abstract void FilterChoicesBasedOnEquipment(Func<IDbCommand> commandFactory);


        protected abstract long CountRows(Func<IDbCommand> command, string tableName);

        protected virtual void InsertSection(string sectionId, short? capacity)
        {
            InsertSection(() => _connection.CreateCommand(), sectionId, capacity);
        }

        protected virtual void InsertSectionEquipment(string sectionId, string equipment)
        {
            InsertSectionEquipment(() => _connection.CreateCommand(), sectionId, equipment);
        }

        protected virtual void UpdateSectionInstructor(string sectionId, string instructorId)
        {
            UpdateSectionInstructor(() => _connection.CreateCommand(), sectionId, instructorId);
        }

        protected virtual void InsertRoom(string roomId, short? capacity)
        {
            InsertRoom(() => _connection.CreateCommand(), roomId, capacity);
        }

        protected virtual void InsertRoomEquipment(string roomId, string equipment)
        {
            InsertRoomEquipment(() => _connection.CreateCommand(), roomId, equipment);
        }

        protected virtual void InsertTimePattern(byte days, DateTime start, DateTime end)
        {
            InsertTimePattern(() => _connection.CreateCommand(), days, start, end);
        }

        public virtual void AddSection(string sectionId, XAttribute capacity, IEnumerable<string> requiredEquipment)
        {
            AddSection(sectionId,
                       capacity == null ? "" : capacity.Value,
                       requiredEquipment);
        }

        public virtual void AddSection(string sectionId, string capacity, IEnumerable<string> requiredEquipment)
        {
            short scapacity;
            AddSection(sectionId,
                       short.TryParse(capacity, out scapacity)
                           ? (short?) scapacity
                           : null,
                       requiredEquipment);
        }

        public virtual void AddSection(string sectionId, short? capacity, IEnumerable<string> requiredEquipment)
        {

            var iSectionId = Convert.ToInt64(sectionId);
            if (iSectionId <= 73544)
                requiredEquipment = requiredEquipment.Union(new[] {"JASON"});

            InsertSection(sectionId, capacity);
            foreach (var equipment in requiredEquipment)
                InsertSectionEquipment(sectionId, equipment);
        }

        public virtual void AssignInstructorToSection(string sectionId, string instructorId)
        {
            UpdateSectionInstructor(sectionId, instructorId);
        }

        public virtual void AddRoom(string roomId, XAttribute capacity, IEnumerable<string> availableEquipment)
        {
            AddRoom(roomId,
                    capacity == null ? "" : capacity.Value,
                    availableEquipment);
        }

        public virtual void AddRoom(string roomId, string capacity, IEnumerable<string> availableEquipment)
        {
            short scapacity;
            AddRoom(roomId,
                    short.TryParse(capacity, out scapacity)
                        ? (short?) scapacity
                        : null,
                    availableEquipment);
        }

        public virtual void AddRoom(string roomId, short? capacity, IEnumerable<string> availableEquipment)
        {
            InsertRoom(roomId, capacity);
            foreach (var equipment in availableEquipment)
                InsertRoomEquipment(roomId, equipment);
        }

        public  virtual void AddTimePattern(TimePattern pattern)
        {
            byte days = Convert.ToByte(pattern.Days);
            InsertTimePattern(days, pattern.Start, pattern.End);
        }

        public virtual void BuildChoices()
        {
            Console.WriteLine("Building choice matrix");
            BuildChoices(() => _connection.CreateCommand());
            Console.WriteLine("Found {0:N0} possible choices.", CountRows("Choices"));
        }

        public virtual void FilterChoicesBasedOnCapacity()
        {
            Console.WriteLine("Filtering choices where rooms are too small.");
            FilterChoicesBasedOnCapacity(() => _connection.CreateCommand());
            Console.WriteLine("After filtering for capacity, {0:N0} choices remain.", CountRows("Choices"));
        }

        public virtual void FilterChoicesBasedOnEquipment()
        {
            Console.WriteLine("Filtering choices where rooms don't have necessary equipment.");
            FilterChoicesBasedOnEquipment(() => _connection.CreateCommand());
            Console.WriteLine("After filtering for equipment, {0:N0} choices remain.", CountRows("Choices"));
        }

        public virtual long CountRows(string tableName)
        {
            return CountRows(() => _connection.CreateCommand(), tableName);
        }

    }

}
