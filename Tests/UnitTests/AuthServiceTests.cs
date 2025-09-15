using System.IdentityModel.Tokens.Jwt;
using Application.Authentication.DTO;
using Application.Authentication.Interface;
using Application.Authentication.Services;
using Application.EmailServices.Interface;
using Castle.Core.Logging;
using Domain.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Project.JWT;
using Tests.Helpers;

namespace Tests.UnitTests;

public class AuthServiceTests
{
    
    private readonly Mock<UserManager<ApplicationUser>>  _mockUserManager =MockHelper.GetUserManagerMock<ApplicationUser>();
    private readonly Mock<RoleManager<IdentityRole>>  _mockRoleManager =MockHelper.GetRoleManagerMock<IdentityRole>();
    private readonly Mock<ITokenProvider> _tokenProviderMock = new();
    private readonly Mock<IConfrimEmailServices> _confrimemailMock = new();
 
    [Fact]
    public void LoginAsync_WhenloginRequestEmailIsEmpty_ShouldReturnFalseObject()
    {
        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);
        var authServices = new AuthServices(_mockUserManager.Object,_mockRoleManager.Object,_tokenProviderMock.Object,_confrimemailMock.Object);
        var result = authServices.LoginAsync(new LoginRequest() {Email = "" , Password = ""}).Result;
        _mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        Assert.Equal(false, result.IsAuthenticated);
        Assert.Equal("Password or Email in Incorrect", result.Message);

        
    }

    [Fact]
    public void LoginAsync_WhenGiveWrongPassword_ShouldReturnFalseObject()
    {
        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
        _mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(false);
        var authServices = new AuthServices(_mockUserManager.Object,_mockRoleManager.Object,_tokenProviderMock.Object,_confrimemailMock.Object);
        var result = authServices.LoginAsync(new LoginRequest() {Email = "Test1@gmail.com" , Password = ""}).Result;
        _mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        _mockUserManager.Verify(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        Assert.Equal(false, result.IsAuthenticated);
        Assert.Equal("Password or Email in Incorrect", result.Message);
    }

    [Fact]
    public void LoginAsync_WhenLoginRequestIsValid_ShouldReturnTrueObject()
    {
       var authServices = new AuthServices(_mockUserManager.Object,_mockRoleManager.Object,_tokenProviderMock.Object,_confrimemailMock.Object);
      
        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new ApplicationUser() { EmailConfirmed = true });

        _mockUserManager.Setup(x => 
            x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);
        _mockUserManager.Setup(x => x.GetLockoutEnabledAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(false);
       _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>());
       _tokenProviderMock.Setup(x => x.GetToken(It.IsAny<ApplicationUser>() , It.IsAny<CancellationToken>())).ReturnsAsync(new JwtSecurityToken());
       
       var result = authServices.LoginAsync(new LoginRequest(){Email ="Test1@gmail.com" ,Password = "Test2"}).Result;
       
       Assert.Equal(true ,result.IsAuthenticated);
       
    }
}