using NUnit.Framework;

namespace BillCollector.Tests
{
    public class SampleTest
    {
        [Test]
        public void Names_Not_Equal()
        {
            //arrange
            var firstname = "Tolani";
            var lastname = "john";

            // Act
            var result =  firstname != lastname;

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
