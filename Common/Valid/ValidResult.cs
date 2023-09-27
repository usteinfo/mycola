namespace MyCloa.Common.Valid;

/// <summary>
/// 校验结果对象
/// </summary>
/// <param name="Success">true成功，false失败</param>
/// <param name="Message">失败返回错误信息</param>
public record ValidResult(bool Success,string Message);