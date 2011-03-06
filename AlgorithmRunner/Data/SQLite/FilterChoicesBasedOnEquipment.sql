DELETE FROM Choices 
WHERE EXISTS (
	SELECT Choices.Id, S.Equipment 
	FROM SectionEquipment S 
	WHERE Choices.SectionId = S.Id
	EXCEPT 
	SELECT Choices.Id, R.Equipment
	FROM RoomEquipment R
	WHERE Choices.RoomId = R.Id
)