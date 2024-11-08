using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects.ResponseDtos;

namespace LoggingWebApi.Formatters;

public class CsvOutputFormatter : TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }
    
    protected override bool CanWriteType(Type? type)
    {
        // Determines whether an instance of a specified type c can be assigned to a variable of the current type.
        // public virtual bool IsAssignableFrom (Type? c);
        
        // The CanWriteType method is overridden, and it indicates whether
        // or not the CompanyDto type can be written by this serializer.
        
        if (typeof(CompanyResponseDto).IsAssignableFrom(type) ||
            typeof(IEnumerable<CompanyResponseDto>).IsAssignableFrom(type))
        {
            return base.CanWriteType(type);
        }
        
        return false;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();

        if (context.Object is IEnumerable<CompanyResponseDto>)
        {
            foreach (var company in (IEnumerable<CompanyResponseDto>)context.Object)
            {
                FormatCsv(buffer, (CompanyResponseDto)company);
            }
        }
        else
        {
            FormatCsv(buffer, (CompanyResponseDto)context.Object);
        }

        await response.WriteAsync(buffer.ToString());
    }

    private static void FormatCsv(StringBuilder buffer, CompanyResponseDto companyResponseDto)
    {
        buffer.AppendLine($"{companyResponseDto.Id},\"{companyResponseDto.Name}\",\"{companyResponseDto.FullAddress}\"");
    }
}