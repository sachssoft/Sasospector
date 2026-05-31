using Sachssoft.Sasospector.Schemas;

namespace Sachssoft.Sasospector.Test.Person
{
    internal class PersonSchemaSource : SchemaSourceBase<PersonModel>
    {
        protected override IInspectorSchema? CreateSchema(PersonModel model)
        {
            var builder = new InspectorSchemaBuilder<PersonModel>();

            builder.AddProperty(
                typeof(string),
                nameof(PersonModel.FirstName),
                (s, t) => s.FirstName,
                (s, t, v) => s.FirstName = (string?)v
            );

            builder.AddProperty(
                typeof(string),
                nameof(PersonModel.LastName),
                (s, t) => s.LastName,
                (s, t, v) => s.LastName = (string?)v
            );

            builder.AddProperty(
                typeof(DateTime),
                nameof(PersonModel.DateOfBirth),
                (s, t) => s.DateOfBirth,
                (s, t, v) => s.DateOfBirth = (DateTime)(v ?? default(DateTime))
            );

            builder.AddProperty(
                typeof(int),
                nameof(PersonModel.Age),
                (s, t) => s.Age,
                new InspectorPropertyInfoMetadata()
            );

            return builder.Build();
        }
    }
}
