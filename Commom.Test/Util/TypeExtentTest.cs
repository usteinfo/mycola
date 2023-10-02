using System.Globalization;

namespace MyCloa.Common.Util
{
    [TestFixture]
    public class TypeExtentTests
    {
        [Test]
        public void To_ConvertValueToDestinationType_ReturnsConvertedValue()
        {
            // Arrange
            object value = "10";
            Type destinationType = typeof(int);

            // Act
            var result = value.To(destinationType);

            // Assert
            Assert.AreEqual(10, result);
        }

        [Test]
        public void To_ConvertValueToDestinationTypeWithCulture_ReturnsConvertedValue()
        {
            // Arrange
            object value = "10.5";
            Type destinationType = typeof(decimal);
            var culture = new CultureInfo("en-US");

            // Act
            var result = value.To(destinationType, culture);

            // Assert
            Assert.AreEqual(10.5m, result);
        }

        [Test]
        public void To_ConvertEnumValueToInt_ReturnsConvertedValue()
        {
            // Arrange
            object value = MyEnum.Value1;
            Type destinationType = typeof(int);

            // Act
            var result = value.To(destinationType);

            // Assert
            Assert.AreEqual(1, result);
        }
        
        [Test]
        public void To_ConvertIntValueToEnum_ReturnsConvertedValue()
        {
            // Arrange
            object value = 2;
            Type destinationType = typeof(MyEnum);

            // Act
            var result = value.To(destinationType);

            // Assert
            Assert.AreEqual(MyEnum.Value2, result);
        }

        [Test]
        public void To_ConvertIncompatibleValueToDestinationType_ReturnsConvertedValue()
        {
            // Arrange
            object value = "2010-01-01";
            Type destinationType = typeof(DateTime);

            // Act
            var result = value.To(destinationType);

            // Assert
            Assert.IsInstanceOf<DateTime>(result);
            Assert.AreEqual(new DateTime(2010, 1, 1), result);
        }
    }

    public enum MyEnum
    {
        Value1 =1,
        Value2 =2
    }
}