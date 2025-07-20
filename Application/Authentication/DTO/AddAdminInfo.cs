namespace Application.Authentication.DTO;

public class AddAdminInfo
{
    public string Message { get; set; }

    public bool IsAuthenticated { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }
}