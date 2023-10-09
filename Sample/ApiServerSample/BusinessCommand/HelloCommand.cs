using MyCloa.Common.Command;

namespace ApiServerSample.BusinessCommand
{
    [Command(Name = "Hello", RequestAuthentication = false, RequestAuthorization = false)]
    public class HelloCommand : CommandBase<string, string>
    {
        protected override string ExecuteCore(string request)
        {
            return "Hello " + request;
        }
    }
}
