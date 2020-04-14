using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task2.DemoSource;

namespace Task2.DemoImplementation
{
    public class ToDo
    {
        public IEnumerable<(Account, Person)> FindPersonAndGroupByEmail(
            IEnumerable<Group> groups,
            IEnumerable<Account> accounts,
            IEnumerable<string> emails)
        {
            var people = groups.SelectMany(group => group.People);

            var matchingPeopleToEmails = new BlockingCollection<Person>();

            Parallel.ForEach(people, person =>
            {
                if (person.Emails.Any(email => emails.Contains(email.Email)))
                    matchingPeopleToEmails.Add(person);
            });

            var personWithSingleEmail = matchingPeopleToEmails
                .SelectMany(person => person.Emails, (person, email) => new { Person = person, EmailAddress = email });

            return accounts
                .AsParallel()
                .Select(account => 
                (
                    account,
                    personWithSingleEmail.FirstOrDefault(p => p.EmailAddress.Email == account.EmailAddress.Email)?.Person)
                )
                .Where(x => x.Item2 != null);
        }
    }
}
