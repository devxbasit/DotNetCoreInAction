using System;

namespace Core.Models
{
    public class Customer
    {
        public Customer(string firstName, string lastName, string zipCode)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            ZipCode = zipCode ?? throw new ArgumentNullException(nameof(zipCode));
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string ZipCode { get; set; }
    }
}