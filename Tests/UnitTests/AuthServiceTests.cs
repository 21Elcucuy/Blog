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
        Assert.False(result.IsAuthenticated);
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
        Assert.False(result.IsAuthenticated);
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
       
      
       Assert.True(result.IsAuthenticated);
    }

    [Fact]
    public void RegisterAsync_WhenGiveExsitingEmailAndConfirmd_ShouldReturnFalseObject()
    {
        var authServices = new AuthServices(_mockUserManager.Object,_mockRoleManager.Object,_tokenProviderMock.Object,_confrimemailMock.Object);

        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new ApplicationUser(){EmailConfirmed = true});
       
        var result =  authServices.RegisterAsync(new RegisterRequest()).Result;
        _mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        Assert.False(result.IsAuthenticated);
        Assert.Equal("Email Already Rgisterd", result.Message);
    }
    [Fact]
    public void RegisterAsync_WhenGiveExsitingEmailAndNotConfirmd_ShouldReturnFalseObject()
    {
        var authServices = new AuthServices(_mockUserManager.Object,_mockRoleManager.Object,_tokenProviderMock.Object,_confrimemailMock.Object);

        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new ApplicationUser(){EmailConfirmed = false});
        _mockUserManager.Setup(x => 
                x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync("Test1");
        _confrimemailMock.Setup(x =>
            x.SendConfromationEmailAsync(It.IsAny<string>(), It.IsAny<string>() ,It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        var result =  authServices.RegisterAsync(new RegisterRequest()).Result;
        _mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        _confrimemailMock.Verify(x => 
            x.SendConfromationEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        
        Assert.False(result.IsAuthenticated);
        Assert.Equal("Please confirm your account by Chicking on Gmail", result.Message);
    }

    [Fact]
    public void RegisterAsync_WhenGiveExsitingUserName_ShouldReturnFalseObject()
    {
        var authServices = new AuthServices(_mockUserManager.Object,_mockRoleManager.Object,_tokenProviderMock.Object,_confrimemailMock.Object);

        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser)null);
        _mockUserManager.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
        
        var result =  authServices.RegisterAsync(new RegisterRequest()).Result;
        _mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        _mockUserManager.Verify(x => x.FindByNameAsync(It.IsAny<string>()), Times.Once);
        Assert.False(result.IsAuthenticated);
        Assert.Equal("UserName Already Used", result.Message);
    }

    [Fact]
    public void RegisterAsync_WhenGiveValidObject_ShouldReturnTrueObject()
    {
        var authServices = new AuthServices(_mockUserManager.Object,_mockRoleManager.Object,_tokenProviderMock.Object,_confrimemailMock.Object);

        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser)null);
        _mockUserManager.Setup(x => 
                x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync("Test1");
        _confrimemailMock.Setup(x =>
                x.SendConfromationEmailAsync(It.IsAny<string>(), It.IsAny<string>() ,It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockUserManager.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>() 
            , It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(),
            It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        _tokenProviderMock.Setup(x => x.GetToken(It.IsAny<ApplicationUser>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new JwtSecurityToken());
        
        var result =  authServices.RegisterAsync(new RegisterRequest()).Result;
        _tokenProviderMock.Verify(x => x.GetToken(It.IsAny<ApplicationUser>(),
            It.IsAny<CancellationToken>()) ,Times.Once);
        Assert.True(result.IsAuthenticated);
        Assert.Equal("Please Check Email", result.Message);
    }
    
}