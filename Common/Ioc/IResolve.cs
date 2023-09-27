namespace MyCloa.Common.Ioc;

/// <summary>
/// IOC接口
/// </summary>
public interface IResolve
{
    /// <summary>
    /// 创建类型为T的对象
    /// </summary>
    /// <typeparam name="T">待创建对象类型</typeparam>
    /// <returns>返回创建成功的对象</returns>
    T Resolve<T>();
}
