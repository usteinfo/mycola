using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace MyCloa.Common.DataSerializer;

/// <summary>
/// 默认对象序列化处理器
/// </summary>
public class DefaultSerializer : IDataSerializer
{
    public T Deserialize<T>(string data)
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // JavaScriptEncoder.Create(UnicodeRanges.All)
            PropertyNameCaseInsensitive = true
        };
        return JsonSerializer.Deserialize<T>(data, options) ?? throw new InvalidOperationException();
    }

    public string Serializer<T>(T data)
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // JavaScriptEncoder.Create(UnicodeRanges.All)
            PropertyNameCaseInsensitive = true
        };
        //Debug.Assert(data != null, nameof(data) + " != null");
        return JsonSerializer.Serialize(data, data.GetType(), options);
    }
}