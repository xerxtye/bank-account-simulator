using BankAccountApi.Models;
using Xunit;

namespace BankAccountApi.Tests;

public sealed class DtoSecurityTests
{
    [Fact]
    public void BankAccountItemDto_DoesNotExposeSensitiveAccountFields()
    {
        var propertyNames = typeof(BankAccountItemDTO)
            .GetProperties()
            .Select(property => property.Name)
            .ToHashSet(StringComparer.Ordinal);

        Assert.DoesNotContain(nameof(BankAccountItem.PasswordHash), propertyNames);
        Assert.DoesNotContain(nameof(BankAccountItem.Balance), propertyNames);
        Assert.Contains(nameof(BankAccountItem.Id), propertyNames);
        Assert.Contains(nameof(BankAccountItem.FirstName), propertyNames);
        Assert.Contains(nameof(BankAccountItem.LastName), propertyNames);
    }
}
