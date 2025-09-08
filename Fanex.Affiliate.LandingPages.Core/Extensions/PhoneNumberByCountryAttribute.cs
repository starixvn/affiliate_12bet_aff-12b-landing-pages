namespace Fanex.Affiliate.LandingPages.Core.Extensions;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PhoneNumbers;

public class PhoneNumberByCountryAttribute : ValidationAttribute, IClientModelValidator
{
    public string CountryProperty { get; }

    public PhoneNumberByCountryAttribute(string countryProperty)
    {
        CountryProperty = countryProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var phoneNumber = value as string;
        if (string.IsNullOrEmpty(phoneNumber))
            return ValidationResult.Success;

        var countryProp = validationContext.ObjectType.GetProperty(CountryProperty);
        var country = countryProp?.GetValue(validationContext.ObjectInstance, null)?.ToString();

        var phoneUtil = PhoneNumberUtil.GetInstance();
        try
        {
            var parsed = phoneUtil.Parse(phoneNumber, country);
            if (phoneUtil.IsValidNumber(parsed))
                return ValidationResult.Success;
        }
        catch { }

        return new ValidationResult($"Invalid phone number for the country {country}");
    }

    // Thêm rule cho client
    public void AddValidation(ClientModelValidationContext context)
    {
        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-phonenocountry", ErrorMessage ?? "Invalid phone number");
        MergeAttribute(context.Attributes, "data-val-phonenocountry-countryproperty", CountryProperty);
    }

    private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
    {
        if (attributes.ContainsKey(key)) return false;
        attributes.Add(key, value);
        return true;
    }
}
