DELETE FROM Choices
WHERE Id IN (
	SELECT C.Id
	FROM Choices C, Sections S, Rooms R
	WHERE C.SectionId = S.Id AND C.RoomId = R.Id 
	AND S.Capacity IS NOT NULL
	AND R.Capacity IS NOT NULL
	AND S.Capacity > R.Capacity
)