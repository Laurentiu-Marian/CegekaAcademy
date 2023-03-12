namespace PetShelter.Api.Resources
{
    public class Person
    {
        public string Name { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string IdNumber { get; set; }
    }

    public interface IPersonBuilder
    {
        void AddName(string name);
        void AddDateOfBirth(DateTime dateOfBirth);
        void AddIdNumber(string idNumber);
        void AddAllAtributes(string name, DateTime dateOfBirth, string idNumber);
    }

    public abstract class RoPersonBuilder : IPersonBuilder
    {
        protected Person person = new Person();

        public RoPersonBuilder()
        {
            this.Reset();
        }

        public void Reset()
        {
            this.person = new Person();
        }

        public void AddName(string name)
        {
            person.Name = name;
        }

        public void AddDateOfBirth(DateTime dateOfBirth)
        {
            person.DateOfBirth = dateOfBirth;
        }

        public void AddIdNumber(string idNumber)
        {
            person.IdNumber = idNumber;
        }
        public void AddAllAtributes(string name, DateTime dateOfBirth, string idNumber)
        {
            AddName(name);
            AddDateOfBirth(dateOfBirth);
            AddIdNumber(idNumber);
        }

        public Person GetPerson()
        {
            Person builtPerson = this.person;

            this.Reset();

            return builtPerson;
        }
    }
}
