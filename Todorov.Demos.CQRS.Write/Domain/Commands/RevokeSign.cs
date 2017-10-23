using Todorov.Demos.CQRS.Infrastructure.Interfaces;

namespace Todorov.Demos.CQRS.Write.Domain.Commands
{
    public class RevokeSign : ICommand
    {
        public RevokeSign(string email)
        {
            Email = email;
        }

        public string Email { get; }

        public override string ToString()
        {
            return string.Format("[RevokeSign: Email={0}]", Email);
        }
    }
}
