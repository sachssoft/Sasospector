namespace Sachssoft.Sasospector.Test.Person
{
    public class PersonTests
    {
        [Fact]
        public void Schema_Should_Read_All_Person_Properties_Correctly()
        {
            var person1 = CreatePerson1();
            var person2 = CreatePerson2();
            var person3 = CreatePerson3();

            var schemaSource = new PersonSchemaSource();
            var schema = schemaSource.Resolve(person1);

            Assert.NotNull(schema);

            foreach (var property in schema.Properties.Values)
            {
                var value1 = property.GetValue(person1);
                var value2 = property.GetValue(person2);
                var value3 = property.GetValue(person3);

                Assert.NotNull(value1);
                Assert.NotNull(value2);
                Assert.NotNull(value3);
            }
        }

        [Fact]
        public void FirstName_Should_Be_Correct()
        {
            var person = CreatePerson1();

            var schema = new PersonSchemaSource().Resolve(person);

            var prop = schema.Properties["FirstName"];

            Assert.Equal("John", prop.GetValue(person));
        }

        private static PersonModel CreatePerson1()
        {
            return new PersonModel
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
        }

        private static PersonModel CreatePerson2()
        {
            return new PersonModel
            {
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 5, 15)
            };
        }

        private static PersonModel CreatePerson3()
        {
            return new PersonModel
            {
                FirstName = "Alice",
                LastName = "Johnson",
                DateOfBirth = new DateTime(2000, 12, 31)
            };
        }
    }
}