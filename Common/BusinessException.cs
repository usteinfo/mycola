using System.Runtime.Serialization;

namespace MyCloa.Common;

/// <summary>
/// 业务异常类
/// </summary>
[Serializable]
public class BusinessException:Exception
{
    /// <summary>
    /// 默认构造函数
    /// </summary>
    public BusinessException()
    {
    }

    /// <summary>
    /// 异常扩展信息：json格式
    /// </summary>
    public string BusinessData { get; private set; } = "";

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message">异常信息</param>
    public BusinessException(string? message)
        : base(message)
    {
        
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message">异常信息</param>
    /// <param name="innerException">内联异常</param>
    public BusinessException(string? message, Exception? innerException)
        : base(message,innerException)
    {
        
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message">异常信息</param>
    /// <param name="array">格式化异常信息的参数</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static BusinessException Create(string? message, params object[] array)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }
        
        var businessException = new BusinessException(string.Format(message,array));
        return businessException;
    }
    
    /// <summary>
    /// 创建异常
    /// </summary>
    /// <param name="message">异常信息</param>
    /// <param name="data">异常扩展信息：json格式</param>
    /// <returns>返回创建成功的异常对象</returns>
    public static BusinessException Create(string? message, string data = "")
    {
        var businessException = new BusinessException(message)
        {
            BusinessData = data
        };
        return businessException;
    }
    /// <summary>
    /// 创建异常
    /// </summary>
    /// <param name="message">异常信息</param>
    /// <param name="innerException">内联异常</param>
    /// <param name="data">异常扩展信息：json格式</param>
    /// <returns>返回创建成功的异常对象</returns>
    public static BusinessException Create(string? message, Exception? innerException, string data = "")
    {
        var businessException = new BusinessException(message,innerException)
        {
            BusinessData = data
        };
        return businessException;
    }

    /// <summary>When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is a null reference (<see langword="Nothing" /> in Visual Basic).</exception>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (!string.IsNullOrEmpty(BusinessData))
        {
            info.AddValue("BusinessData", BusinessData);
        }
        base.GetObjectData(info, context);
    }
}