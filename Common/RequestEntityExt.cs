namespace MyCloa.Common;

/// <summary>
/// 请求命令扩展
/// </summary>
public static class RequestEntityExt
{
    public static RequestStringEntity Copy(this RequestStringEntity requestStringEntity)
    {
        RequestStringEntity remoteRequestStringEntity = new RequestStringEntity();
        remoteRequestStringEntity.Data = requestStringEntity.Data;
        remoteRequestStringEntity.Token = requestStringEntity.Token;
        remoteRequestStringEntity.CommandName = requestStringEntity.CommandName;
        remoteRequestStringEntity.ServiceName = requestStringEntity.ServiceName;
        remoteRequestStringEntity.RequestId = requestStringEntity.RequestId;
    
        remoteRequestStringEntity.HttpsSchema = requestStringEntity.HttpsSchema;
        return remoteRequestStringEntity;
    }
}