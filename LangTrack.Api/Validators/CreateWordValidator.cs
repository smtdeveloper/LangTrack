using LangTrack.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace LangTrack.Api.Validators;

public class CreateWordValidator
{
    public static List<string> Validate(CreateWordDto dto)
    {
        var errors = new List<string>();

        // Text validation
        if (string.IsNullOrWhiteSpace(dto.Text))
        {
            errors.Add("Text is required");
        }
        else if (dto.Text.Trim().Length < 1 || dto.Text.Trim().Length > 128)
        {
            errors.Add("Text must be between 1 and 128 characters");
        }

        // Meaning validation
        if (string.IsNullOrWhiteSpace(dto.Meaning))
        {
            errors.Add("Meaning is required");
        }
        else if (dto.Meaning.Trim().Length < 1 || dto.Meaning.Trim().Length > 512)
        {
            errors.Add("Meaning must be between 1 and 512 characters");
        }

        // Example validation (optional)
        if (!string.IsNullOrWhiteSpace(dto.Example) && dto.Example.Trim().Length > 512)
        {
            errors.Add("Example must be at most 512 characters");
        }

        // Tags validation (optional)
        if (!string.IsNullOrWhiteSpace(dto.Tags) && dto.Tags.Trim().Length > 256)
        {
            errors.Add("Tags must be at most 256 characters");
        }

        return errors;
    }
}
