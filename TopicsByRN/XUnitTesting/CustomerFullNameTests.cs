using AutoFixture;
using AutoFixture.Xunit2;
using Core.Models;
using Xunit;

namespace XUnitTesting
{
    public class CustomerFullNameTests
    {
        [Fact]
        public void FullNameReturnsExpected()
        {
            var sut = new Customer("Jon", "Skeet", "190000");

            var actual = sut.FullName;
            Assert.Equal("Jon Skeet", sut.FullName);
        }

        [Theory]
        [InlineData("Jon", "Skeet", "Jon Skeet")]
        public void FullNameReturnsExpected2(string firstName, string lastName, string expected)
        {
            var fixture = new Fixture();
            var sut = fixture.Build<Customer>()
                .With(x => x.FirstName, firstName)
                .With(x => x.LastName, lastName)
                .Create();

            var actual = sut.FullName;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineAutoData("Jon", "Skeet", "Jon Skeet")]
        public void FullNameReturnsExpected3(string firstName, string lastName, string expected, Customer sut)
        {
            sut.FirstName = firstName;
            sut.LastName = lastName;
            var actual = sut.FullName;

            Assert.Equal(expected, actual);
        }
    }
}