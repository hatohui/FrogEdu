using FrogEdu.Shared.Kernel;

namespace FrogEdu.Subscription.Domain.ValueObjects;

/// <summary>
/// Value Object representing money with amount and currency
/// Default currency is VND (Vietnamese Dong)
/// </summary>
public sealed class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, string currency = "VND")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty", nameof(currency));

        var normalizedCurrency = currency.Trim().ToUpperInvariant();
        return new Money(amount, normalizedCurrency);
    }

    public static Money Zero(string currency = "VND") => new(0, currency);

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException(
                $"Cannot add money with different currencies: {Currency} and {other.Currency}"
            );

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException(
                $"Cannot subtract money with different currencies: {Currency} and {other.Currency}"
            );

        if (Amount < other.Amount)
            throw new InvalidOperationException("Resulting amount would be negative");

        return new Money(Amount - other.Amount, Currency);
    }

    public bool IsGreaterThan(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException(
                $"Cannot compare money with different currencies: {Currency} and {other.Currency}"
            );

        return Amount > other.Amount;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount:N0} {Currency}";
}
