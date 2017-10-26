namespace Todorov.Demos.CQRS.Read.Models
{
    public class PetitionSigner
    {
        public PetitionSigner(string email, string firstName, string lastName)
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
            return $"[PetitionSigner: Email={Email}, FirstName={FirstName}, LastName={LastName}]";
        }
    }
}
