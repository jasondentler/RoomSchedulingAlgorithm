UPDATE Sections 
SET InstructorId = (SELECT Id FROM Instructors WHERE ExternalInstructorId = @ExternalInstructorId LIMIT 1)
WHERE ExternalSectionId = @ExternalSectionId
