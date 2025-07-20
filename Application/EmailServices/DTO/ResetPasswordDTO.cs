namespace Application.EmailServices.DTO;

public class ResetPasswordDTO
{
    public string user_id { get; set; }
    public string token { get; set; }
    public string newPassword { get; set; }
    
}