INSERT INTO SectionEquipment (SectionId, Equipment) VALUES (
	(SELECT Id FROM Sections WHERE ExternalSectionId = @ExternalSectionId LIMIT 1), 
	@Equipment
)
