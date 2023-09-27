using System.Net.Http.Headers;
using System.Text.Json;

namespace MyCloa.Common.Command;

/// <summary>
/// 远程命令抽象类
/// </summary>
public abstract class RemoteCommandBase:IRemoteCommand
{
    private HttpClient _httpClient;
    private string _remoteUrl;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="httpClient">httpclicen对象</param>
    /// <param name="remoteUrl">远程网关地址</param>
    protected RemoteCommandBase(HttpClient httpClient,string remoteUrl)
    {
        _httpClient = httpClient;
        _remoteUrl = remoteUrl;
    }


    /// <summary>
    /// 执行远程命令
    /// </summary>
    /// <param name="requestStringEntity">命令请求参数</param>
    /// <returns>返回执行结果</returns>
    public async Task<string> Execute(RequestStringEntity requestStringEntity)
    {
        HttpContent httpContent = new StringContent(JsonSerializer.Serialize(requestStringEntity));
        httpContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
        
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
        
        var result = await _httpClient.PostAsync(CreateRemoteUrl(requestStringEntity),httpContent);
        return await result.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 创建远程访问Url
    /// </summary>
    /// <param name="requestStringEntity">远程请求参数</param>
    /// <returns>返回远程地址</returns>
    private string CreateRemoteUrl(RequestStringEntity requestStringEntity)
    {
        return $"{(requestStringEntity.HttpsSchema ? "https":"http")}://{requestStringEntity.ServiceName}{_remoteUrl}";
    }

}