INSERT INTO RoomEquipment (RoomId, Equipment) VALUES (
	(SELECT Id FROM Rooms WHERE ExternalRoomId = @ExternalRoomId LIMIT 1), 
	@Equipment
)
