using Todorov.Demos.CQRS.Infrastructure.Interfaces;

namespace Todorov.Demos.CQRS.Write.Domain.Commands
{
    public class SignPetition : ICommand
    {
        public SignPetition(string email, string firstName, string lastName)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        public string Email { get; }   
        public string FirstName { get; }
        public string LastName { get; }    

        public override string ToString()
        {
            return string.Format("[SignPetition: Email={0}, FirstName={1}, LastName={2}]", Email, FirstName, LastName);
        }
    }
}
