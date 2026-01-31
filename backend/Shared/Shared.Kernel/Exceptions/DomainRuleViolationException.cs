using FrogEdu.Shared.Kernel.Exceptions;

public class DomainRuleViolationException : DomainException
{
    public DomainRuleViolationException(string message)
        : base(message) { }
}
