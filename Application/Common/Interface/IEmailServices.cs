using Application.Common.enums;

namespace Application.PostSev;

public interface IEmailServices
{
    public Task SendEmailAsync(string to, string subject, string body, string textPart = "html" ,CancellationToken cancellationToken = default);
   
}