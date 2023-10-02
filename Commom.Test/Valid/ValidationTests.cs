using System.ComponentModel.DataAnnotations;

namespace MyCloa.Common.Valid;

[TestFixture]
public class ValidationTests
{
    [Test]
    public void Execute_ValidRequest_ReturnsValidResult()
    {
        // Arrange
        var requestEntity = new ValidRequestEntity(){Age = 20};

        // Act
        var result = new ValidationContextRequest().Valid(requestEntity);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.AreEqual("", result.Message);
    }

    [Test]
    public void Execute_InvalidRequest_ReturnsInvalidResultWithErrorMessage()
    {
        // Arrange
        var requestEntity = new InvalidRequestEntity();

        // Act
        var result = new ValidationContextRequest().Valid(requestEntity);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("请求参数验证出错：Error message", result.Message);
    }

    // Positive test case
    public class ValidRequestEntity
    {
        [System.ComponentModel.DataAnnotations.Range(10,99,ErrorMessage = "Error message")]
        public int Age { get; set; }
    }

    // Negative test case
    public class InvalidRequestEntity
    {
        [Required(ErrorMessage = "Error message")]
        public string Property { get; set; }
    }
}