using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using Microsoft.Extensions.Logging;
namespace Tests.Helpers;

public class MockHelper
{
    
    public static Mock<UserManager<TUser>> GetUserManagerMock<TUser>() where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();
        var options = new Mock<IOptions<IdentityOptions>>();
        var passwordHasher = new Mock<IPasswordHasher<TUser>>();
        var userValidators = new List<IUserValidator<TUser>> { new Mock<IUserValidator<TUser>>().Object };
        var passwordValidators = new List<IPasswordValidator<TUser>> { new Mock<IPasswordValidator<TUser>>().Object };
        var lookupNormalizer = new Mock<ILookupNormalizer>();
        var identityErrorDescriber = new Mock<IdentityErrorDescriber>();
        var services = new Mock<IServiceProvider>();
        var logger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<TUser>>>();

        return new Mock<UserManager<TUser>>(
            store.Object,
            options.Object,
            passwordHasher.Object,
            userValidators,
            passwordValidators,
            lookupNormalizer.Object,
            identityErrorDescriber.Object,
            services.Object,
            logger.Object
        );
    }
    public static Mock<RoleManager<TRole>> GetRoleManagerMock<TRole>() where TRole : class
    {
        var store = new Mock<IRoleStore<TRole>>();
        var roleValidators = new List<IRoleValidator<TRole>> { new Mock<IRoleValidator<TRole>>().Object };
        var keyNormalizer = new Mock<ILookupNormalizer>();
        var errors = new IdentityErrorDescriber();
        var logger = new Mock<ILogger<RoleManager<TRole>>>();

        return new Mock<RoleManager<TRole>>(
            store.Object,
            roleValidators,
            keyNormalizer.Object,
            errors,
            logger.Object
        );
    }
    
}