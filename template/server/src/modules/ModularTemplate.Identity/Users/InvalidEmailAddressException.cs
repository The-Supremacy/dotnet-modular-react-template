using ModularTemplate.SharedKernel.Domain;

namespace ModularTemplate.Identity.Users;

public sealed class InvalidEmailAddressException : DomainException
{
    public InvalidEmailAddressException(string message)
        : base(message)
    {
    }
}
