namespace MyCloa.Common.DataSerializer;

/// <summary>
/// 数据序列化接口
/// </summary>
public interface IDataSerializer
{
    /// <summary>
    /// 反序列化字符串
    /// </summary>
    /// <typeparam name="T">反序列后的对象类型</typeparam>
    /// <param name="data">待反序列化的字符串</param>
    /// <returns>返回反序列化后的对象</returns>
    T Deserialize<T>(string data);
    /// <summary>
    /// 对象序列化为字符串
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="data">待序列化对象</param>
    /// <returns>返回序列化的后字符串</returns>
    string Serializer<T>(T data);
}