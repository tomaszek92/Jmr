using AutoFixture;
using Shouldly;
using System.Linq;
using Task2.DemoImplementation;
using Task2.DemoSource;
using Xunit;

namespace Task2.UnitTests.DemoImplementation
{
    public class ToDoUnitTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly ToDo _toDo = new ToDo();

        [Fact]
        public void Should_return_empty_list_if_groups_are_empty()
        {
            // Arrange
            var groups = Enumerable.Empty<Group>();
            var accounts = _fixture.CreateMany<Account>();
            var emails = _fixture.CreateMany<string>();

            // Act
            var results = _toDo.FindPersonAndGroupByEmail(groups, accounts, emails);

            // Assert
            results.ShouldBeEmpty();
        }

        [Fact]
        public void Should_return_empty_list_if_accounts_are_empty()
        {
            // Arrange
            var groups = _fixture.CreateMany<Group>();
            var accounts = Enumerable.Empty<Account>();
            var emails = _fixture.CreateMany<string>();

            // Act
            var results = _toDo.FindPersonAndGroupByEmail(groups, accounts, emails);

            // Assert
            results.ShouldBeEmpty();
        }

        [Fact]
        public void Should_return_empty_list_if_emails_are_empty()
        {
            // Arrange
            var groups = _fixture.CreateMany<Group>();
            var accounts = _fixture.CreateMany<Account>();
            var emails = Enumerable.Empty<string>();

            // Act
            var results = _toDo.FindPersonAndGroupByEmail(groups, accounts, emails);

            // Assert
            results.ShouldBeEmpty();
        }

        [Fact]
        public void Should_return_empty_list_if_there_is_no_matching_elements()
        {
            // Arrange
            var groups = _fixture.CreateMany<Group>();
            var accounts = _fixture.CreateMany<Account>();
            var emails = _fixture.CreateMany<string>();

            // Act
            var results = _toDo.FindPersonAndGroupByEmail(groups, accounts, emails);

            // Assert
            results.ShouldBeEmpty();
        }

        [Fact]
        public void Should_not_include_person_without_matching_account()
        {
            // Arrange
            var groups = _fixture.CreateMany<Group>();
            var accounts = _fixture.CreateMany<Account>();
            var emails = groups.SelectMany(group => group.People).SelectMany(person => person.Emails, (person, emailAddress) => emailAddress.Email);

            // Act
            var results = _toDo.FindPersonAndGroupByEmail(groups, accounts, emails);

            // Assert
            results.ShouldBeEmpty();
        }

        [Fact]
        public void Should_find_all_matching_accounts_and_persons()
        {
            // Arrange
            var groups = _fixture.CreateMany<Group>();

            var accounts = groups
                .SelectMany(group => group.People)
                .SelectMany(person => person.Emails, (person, emailAddress) => _fixture
                    .Build<Account>()
                    .With(a => a.EmailAddress, emailAddress)
                    .Create());

            var emails = groups
                .SelectMany(group => group.People)
                .SelectMany(person => person.Emails, (person, emailAddress) => emailAddress.Email);

            // Act
            var results = _toDo.FindPersonAndGroupByEmail(groups, accounts, emails).ToList();

            // Assert
            results.Count.ShouldBe(emails.Count());

            foreach (var email in emails)
            {
                results.ShouldContain(r => r.Item1.EmailAddress.Email == email
                                        && r.Item2.Emails.Any(emailAddress => emailAddress.Email == email));
            }
        }
    }
}
