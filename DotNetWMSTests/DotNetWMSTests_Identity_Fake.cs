using DotNetWMS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetWMSTests
{
    public class FakeUserManager : UserManager<WMSIdentityUser>
    {
        public FakeUserManager() : 
            base(new Mock<IUserStore<WMSIdentityUser>>().Object, 
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<WMSIdentityUser>>().Object,
            new IUserValidator<WMSIdentityUser>[0],
            new IPasswordValidator<WMSIdentityUser>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object, 
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<WMSIdentityUser>>>().Object)
        {

        }
    }
    public class FakeSignInManager : SignInManager<WMSIdentityUser>
    {
        public FakeSignInManager(): 
            base(new Mock<FakeUserManager>().Object,
            new HttpContextAccessor(),
            new Mock<IUserClaimsPrincipalFactory<WMSIdentityUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<WMSIdentityUser>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<WMSIdentityUser>>().Object)
        { 
        
        }
    }
    public class FakeRoleManager : RoleManager<IdentityRole>
    {
        public FakeRoleManager() :
            base(
            new Mock<IRoleStore<IdentityRole>>().Object,
            new IRoleValidator<IdentityRole>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<ILogger<FakeRoleManager>>().Object
            
            )
        { 

        }
    }
    public class FakeLogger
    {
        public FakeLogger()
        {
            new Mock<ILogger<UserManager<WMSIdentityUser>>>();
            new Mock<ILogger<SignInManager<WMSIdentityUser>>>();
            new Mock<ILogger<RoleManager<WMSIdentityUser>>>();
            
        }
    }

    public class FakeUserManagerBuilder
    {
        private Mock<FakeUserManager> _mock = new Mock<FakeUserManager>();

        public FakeUserManagerBuilder With(Action<Mock<FakeUserManager>> mock)
        {
            mock(_mock);
            return this;
        }
        public Mock<FakeUserManager> Build() => _mock;
    }
    public class FakeSignInManagerBuilder
    {
        private Mock<FakeSignInManager> _mock = new Mock<FakeSignInManager>();

        public FakeSignInManagerBuilder With(Action<Mock<FakeSignInManager>> mock)
        {
            mock(_mock);
            return this;
        }
        public Mock<FakeSignInManager> Build() => _mock;
    }
    public class FakeRoleManagerBuilder
    {
        private Mock<FakeRoleManager> _mock = new Mock<FakeRoleManager>();

        public FakeRoleManagerBuilder With(Action<Mock<FakeRoleManager>> mock)
        {
            mock(_mock);
            return this;
        }
        public Mock<FakeRoleManager> Build() => _mock;
    }
    public class FakeLoggerBuilder
    {
        private Mock<FakeLogger> _mock = new Mock<FakeLogger>();

        public FakeLoggerBuilder With(Action<Mock<FakeLogger>> mock)
        {
            mock(_mock);
            return this;
        }
        public Mock<FakeLogger> Build() => _mock;
    }
    

}
