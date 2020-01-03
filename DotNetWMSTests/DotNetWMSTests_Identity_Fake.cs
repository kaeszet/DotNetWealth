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
    public class FakeUserManagerBuilder
    {
        private Mock<FakeUserManager> _mock = new Mock<FakeUserManager>();

        public FakeUserManagerBuilder With(Action<Mock<FakeUserManager>> mock)
        {
            mock(_mock);
            return this;
        }
        public Mock<FakeUserManager> Build()
        {
            return _mock;
        }
    }
    public class FakeSignInManagerBuilder
    {
        private Mock<FakeSignInManager> _mock = new Mock<FakeSignInManager>();

        public FakeSignInManagerBuilder With(Action<Mock<FakeSignInManager>> mock)
        {
            mock(_mock);
            return this;
        }
        public Mock<FakeSignInManager> Build()
        {
            return _mock;
        }
    }

}
