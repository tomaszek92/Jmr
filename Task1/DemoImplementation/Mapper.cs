using System;
using System.Collections.Generic;
using System.Linq;
using Task1.DemoSource;
using Task1.DemoTarget;

namespace Task1.DemoImplementation
{
    public class Mapper
    {
        // Ta metoda może zostać użyta, np. do mapowania encji bazodanowych na encje dto.
        // W przypadku takiego mapowania, trzeba uważać by właściowść Emails z klasy Person była zmaterializowana.
        // Dzięki temu unikniemy kolejnych zapytań do bazy danych, co będzie mało wydajne.
        public static IEnumerable<PersonWithEmail> Flatten(IEnumerable<Person> people)
        {
            return people
                .Where(person => person.Emails != null)
                .SelectMany(person => person.Emails, (person, email) => new PersonWithEmail
                {
                    SanitizedNameWithId = SanitizeNameWithId(person.Name, person.Id).Trim(),
                    FormattedEmail = $"{email.Email} {email.EmailType}".Trim()
                });
        }

        private static string SanitizeNameWithId(string name, string id) => $"{Sanitize(name)} {Sanitize(id)}";

        private static string Sanitize(string @string) => String
            .Join("", @string.Where(letter => Char.IsDigit(letter)
                                          || (letter >= 'a' && letter <= 'z')
                                          || (letter >= 'A' && letter <= 'Z')));
    }
}
