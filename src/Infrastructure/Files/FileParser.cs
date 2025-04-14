using System;
using Application.Files.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Files;

public class FileParser : IFileParser
{
    public char GetDelimiterFromCsv(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        string? headerLine = reader.ReadLine();

        if (string.IsNullOrWhiteSpace(headerLine))
        {
            throw new InvalidDataException("CSV file is empty or invalid.");
        }
            
        var counts = Delimiters.Allowed
            .Select(delimiter => new { Delimiter = delimiter, Count = headerLine.Count(c => c == delimiter) })
            .OrderByDescending(x => x.Count)
            .ToList();

        if (counts.First().Count == 0 || (counts.Count > 1 && counts[0].Count == counts[1].Count))
        {
            throw new InvalidDataException("Unable to determine delimiter.");
        }            

        return counts.First().Delimiter;
    }
}
