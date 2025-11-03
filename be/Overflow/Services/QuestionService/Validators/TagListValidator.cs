using System.ComponentModel.DataAnnotations;

namespace QuestionService.Validators;

public class TagListValidator(int min, int max) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not List<string> tagSlugs)
        {
            return new ValidationResult("TagSlugs must be a list of strings.");
        }

        if (tagSlugs.Count < min || tagSlugs.Count > max)
        {
            return new ValidationResult($"The number of tags must be between {min} and {max}.");
        }

        return ValidationResult.Success;
    }
}
