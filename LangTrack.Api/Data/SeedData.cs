using LangTrack.Domain.Entities;
using LangTrack.Infrastructure.Data;

namespace LangTrack.Api.Data;

public static class SeedData
{
    public static async Task SeedAsync(LangTrackDbContext context)
    {
        if (context.Words.Any()) return; // Already seeded

        var words = new List<Word>
        {
            new Word
            {
                Text = "coherent",
                Meaning = "tutarlı, mantıklı",
                Example = "A coherent plan is essential for success.",
                Tags = "adjective,b2"
            },
            new Word
            {
                Text = "comprehensive",
                Meaning = "kapsamlı, detaylı",
                Example = "The report provides a comprehensive analysis.",
                Tags = "adjective,c1"
            },
            new Word
            {
                Text = "elaborate",
                Meaning = "detaylandırmak, karmaşık",
                Example = "Please elaborate on your proposal.",
                Tags = "verb,b2"
            },
            new Word
            {
                Text = "sophisticated",
                Meaning = "sofistike, gelişmiş",
                Example = "The software has sophisticated features.",
                Tags = "adjective,c1"
            },
            new Word
            {
                Text = "ambiguous",
                Meaning = "belirsiz, çift anlamlı",
                Example = "The instructions were ambiguous.",
                Tags = "adjective,b2"
            },
            new Word
            {
                Text = "paradigm",
                Meaning = "paradigma, örnek model",
                Example = "This represents a new paradigm in education.",
                Tags = "noun,c1"
            },
            new Word
            {
                Text = "scrutinize",
                Meaning = "dikkatle incelemek",
                Example = "We need to scrutinize the data carefully.",
                Tags = "verb,c1"
            },
            new Word
            {
                Text = "ubiquitous",
                Meaning = "her yerde bulunan, yaygın",
                Example = "Smartphones are ubiquitous nowadays.",
                Tags = "adjective,c1"
            },
            new Word
            {
                Text = "meticulous",
                Meaning = "titiz, dikkatli",
                Example = "She is meticulous about details.",
                Tags = "adjective,c1"
            },
            new Word
            {
                Text = "resilient",
                Meaning = "dayanıklı, esnek",
                Example = "The team showed resilient performance.",
                Tags = "adjective,b2"
            }
        };

        context.Words.AddRange(words);
        await context.SaveChangesAsync();
    }
}
