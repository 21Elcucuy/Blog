using System.Net;
using Application.Common.enums;
using Application.PostSev;
using Application.PostSev.Interface;
using Domain.Entity.Identity;
using Helper.EmailSettings;
using Infrastructure.Persistence.DbContexts;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using ILogger = Serilog.ILogger;

namespace Infrastructure.Services;

public class EmailServices : IEmailServices
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<EmailServices> _logger;
    private readonly EmailSettings _settings;

    public EmailServices(IOptions<EmailSettings> Settings, UserManager<ApplicationUser> userManager ,ILogger<EmailServices> logger)
    {
        _userManager = userManager;
        _logger = logger;

        _settings = Settings.Value;
    }

  
    public async  Task SendEmailAsync(string to, string subject, string body, string textPart = "html" ,   CancellationToken cancellationToken = default)
    {
        var email = BuildMessage(to , subject, body, textPart);
        
        using var smtp = new SmtpClient();
        try
        {
          
                await smtp.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, SecureSocketOptions.StartTls ,cancellationToken);
                await smtp.AuthenticateAsync(_settings.SenderEmail, _settings.SenderPassword ,cancellationToken);
            

           var response = await smtp.SendAsync(email , cancellationToken);
           _logger.LogInformation($"Email sent to {to}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending email to {to}");
            throw;
        }
        finally
        {
            await smtp.DisconnectAsync(true ,cancellationToken);
        }
    
        
    }


    
    private MimeMessage BuildMessage(string to, string subject, string body, string textPart )
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(textPart) {Text = body};
        return email;
    }
}