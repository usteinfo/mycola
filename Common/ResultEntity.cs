namespace MyCloa.Common;

/// <summary>
/// 命令执行结果对象
/// </summary>
public class ResultEntity
{
    /// <summary>
    /// 执行结果，true 成功 false 失败
    /// </summary>
    public bool Result { get; set; }

    /// <summary>
    /// 错误消息
    /// </summary>
    public string ErrorMessage { get; set; } = "";

    /// <summary>
    /// 错误代码
    /// </summary>
    public string ErrorCode { get; set; } = "";

    /// <summary>
    /// 请求id
    /// </summary>
    public string RequestId { get; set; } = "";

    /// <summary>
    /// 创建成功结果对象
    /// </summary>
    /// <param name="requestId">请求Id</param>
    /// <returns>返回创建的对象</returns>
    public static ResultEntity Success(string requestId)
    {
        var resultEntity = new ResultEntity
        {
            Result = true,
            RequestId = requestId
        };
        return resultEntity;
    }

    /// <summary>
    /// 创建失败对象
    /// </summary>
    /// <param name="requestId">请求Id</param>
    /// <param name="errormessage">错误消息</param>
    /// <param name="errorCode">错误代码</param>
    /// <returns>返回创建的对象</returns>
    public static ResultEntity Error(string requestId, string errormessage, string errorCode = "")
    {
        var resultEntity = new ResultEntity
        {
            Result = false,
            RequestId = requestId,
            ErrorMessage = errormessage,
            ErrorCode = errorCode
        };
        return resultEntity;
    }

    /// <summary>
    /// 创建成功对象
    /// </summary>
    /// <param name="requestId">请求Id</param>
    /// <param name="data">数据</param>
    /// <typeparam name="T">数据类型</typeparam>
    /// <returns>返回创建的对象</returns>
    public static ResultEntity Success<T>(string requestId, T data)
    {
        var resultEntity = new ResultEntity<T>
        {
            Result = true,
            RequestId = requestId,
            Data = data
        };
        return resultEntity;
    }
}

/// <summary>
/// 泛型结果对象
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class ResultEntity<T> : ResultEntity
{
    /// <summary>
    /// 结果数据
    /// </summary>
    public T? Data { get; set; }
}