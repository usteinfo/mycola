using System.Text.Json;

namespace MyCloa.Common.DataSerializer;

[TestFixture] 
public class DefaultSerializerTests 
{ 
    private DefaultSerializer _serializer; 
 
    [SetUp] 
    public void SetUp() 
    { 
        _serializer = new DefaultSerializer(); 
    } 
 
    [Test] 
    public void Deserialize_ValidData_ReturnsDeserializedObject() 
    { 
        // Arrange 
        var data = "{\"name\":\"John\",\"age\":30,\"city\":\"New York\"}"; 
 
        // Act 
        var result = _serializer.Deserialize<Person>(data); 
 
        // Assert 
        Assert.IsNotNull(result); 
        Assert.AreEqual("John", result.Name); 
        Assert.AreEqual(30, result.Age); 
        Assert.AreEqual("New York", result.City); 
    } 
 
    [Test] 
    public void Deserialize_InvalidData_ThrowsInvalidOperationException() 
    { 
        // Arrange 
        var data = "invalid data"; 
 
        // Act & Assert 
        Assert.Throws<JsonException>(() => _serializer.Deserialize<Person>(data)); 
    }
 
    [Test] 
    public void Serializer_ValidData_ReturnsSerializedString() 
    { 
        // Arrange 
        var person = new Person { Name = "John", Age = 30, City = "New York" }; 
 
        // Act 
        var result = _serializer.Serializer(person); 
 
        // Assert 
        Assert.IsNotNull(result); 
        Assert.AreEqual("{\"Name\":\"John\",\"Age\":30,\"City\":\"New York\"}", result); 
    } 
 
    [Test] 
    public void Serializer_NullData_ThrowsDebugAssertException() 
    { 
        // Arrange & Act & Assert 
        Assert.Throws<NullReferenceException>(() => _serializer.Serializer<Person>(null)); 
    } 
}

public record Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string City { get; set; }
} 