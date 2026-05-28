using System;
using System.Collections.Generic;
using System.Text;

namespace Sachssoft.Sasospector.Test.Person
{
    internal class PersonModel
    {

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Age
        {
            get
            {
                var today = DateTime.Today;

                int age = today.Year - DateOfBirth.Year;

                if (DateOfBirth.Date > today.AddYears(-age))
                    age--;

                return age;
            }
        }
    }
}
