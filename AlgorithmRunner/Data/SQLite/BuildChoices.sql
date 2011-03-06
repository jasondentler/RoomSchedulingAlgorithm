INSERT INTO Choices (SectionId, RoomId, TimePatternId)
SELECT S.Id, R.Id, TP.Id FROM Sections S, Rooms R, TimePatterns TP