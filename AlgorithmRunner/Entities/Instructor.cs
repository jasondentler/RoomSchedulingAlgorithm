namespace AlgorithmRunner.Entities
{

    public class Instructor 
    {

        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public Instructor(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }

}
