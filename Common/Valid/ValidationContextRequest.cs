using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyCloa.Common.Valid;

/// <summary>
/// 数据校验方案
/// </summary>
public class ValidationContextRequest : ValidRequestBase
{
    protected override ValidResult Execute<T>(T requestEntity)
    {
        var context = new ValidationContext(requestEntity, serviceProvider: null, items: null);
        var errorResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(requestEntity, context, errorResults, true))
        {
            StringBuilder sb = new StringBuilder();
            foreach (var validationResult in errorResults)
            {
                sb.AppendFormat("{0},", validationResult.ErrorMessage);
            }
            return  new ValidResult(false,"请求参数验证出错：" + sb.ToString().TrimEnd(','));
        }

        return new ValidResult(true, "");
    }
    
}