using AutoFixture;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using Task1.DemoImplementation;
using Task1.DemoSource;
using Xunit;

namespace Task1.UnitTests.DemoImplementation
{
    public class MapperUnitTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Theory]
        [InlineData("", "", "")]
        [InlineData("a", "", "a")]
        [InlineData("1", "", "1")]
        [InlineData("ą", "", "")]
        [InlineData("%", "", "")]
        [InlineData("", "b", "b")]
        [InlineData("", "2", "2")]
        [InlineData("", "ć", "")]
        [InlineData("", "*", "")]
        [InlineData("a1", "b2", "a1 b2")]
        public void SanitizedNameWithId_should_contain_all_english_letters_and_numbers(string name, string id, string sanitizedNameWithId)
        {
            // Arrange
            var person = new Person
            {
                Name = name,
                Id = id,
                Emails = new List<EmailAddress> { _fixture.Create<EmailAddress>() }
            };

            // Act
            var results = Mapper.Flatten(new List<Person> { person });

            // Assert
            results
                .ShouldHaveSingleItem()
                .SanitizedNameWithId
                .ShouldBe(sanitizedNameWithId);
        }

        [Fact]
        public void FormattedEmail_should_be_merged_Email_and_EmailType()
        {
            // Arrange
            var person = _fixture
                .Build<Person>()
                .With(p => p.Emails, new List<EmailAddress> { _fixture.Create<EmailAddress>() })
                .Create();

            // Act
            var results = Mapper.Flatten(new List<Person> { person });

            // Assert
            results
                .ShouldHaveSingleItem()
                .FormattedEmail
                .ShouldBe($"{person.Emails.First().Email} {person.Emails.First().EmailType}");
        }

        [Fact]
        public void Should_omit_Emails_equal_to_null()
        {
            // Arrange
            var person = _fixture
                .Build<Person>()
                .Without(p => p.Emails)
                .Create();

            // Act
            var results = Mapper.Flatten(new List<Person> { person });

            // Assert
            results.ShouldBeEmpty();
        }

        [Fact]
        public void Should_merge_Emails_with_Person()
        {
            // Act
            const int peopleCount = 5;
            const int emailsCount = 6;

            var people = _fixture
                .Build<Person>()
                .With(p => p.Emails, _fixture.CreateMany<EmailAddress>(emailsCount))
                .CreateMany(peopleCount);

            // Assert
            var results = Mapper.Flatten(people);

            // Arrange
            results.Count().ShouldBe(peopleCount * emailsCount);
        }
    }
}
