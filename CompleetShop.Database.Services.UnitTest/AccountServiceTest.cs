using System;
using System.Diagnostics;
using AutoMapper;
using CompleetKassa.Database.Context;
using CompleetKassa.Database.Core.Entities;
using CompleetKassa.Log.Core;
using CompleetKassa.Models;
using CompleetKassa.ObjectMap;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Xunit;

namespace CompleetKassa.Database.Services.UnitTest
{
	public class AccountServiceTest
	{
		[Fact]
		public void AddRoleAsync_AddRole_Success ()
		{
			// arrange
			// ILogger
			var loggerMock = new Mock<ILogger> ();
			loggerMock.Setup (_ => _.Info (It.IsAny<string> ()))
				.Callback ((string message) => Trace.WriteLine (message));

			// IMapper
			var config = new MapperConfiguration (cfg =>
			{
				cfg.AddProfile (new RoleProfile ());
			});
			var mapper = config.CreateMapper ();


			// IAppUser
			var appUser = new AppUser (1, "LoggedUser");

			//AppDbContext
			var options = new DbContextOptionsBuilder<AppDbContext> ()
				  .UseInMemoryDatabase (Guid.NewGuid ().ToString ())
				  .ConfigureWarnings (w => w.Ignore (InMemoryEventId.TransactionIgnoredWarning))
				  .Options;

			var appDbContext = new AppDbContext (options);

			IAccountService accountService = new AccountService(loggerMock.Object, mapper, appUser, appDbContext);

			var newRole = new RoleModel
			{
				Name = "Role 1",
				Description = "Role 1 Description"
			};

			// act
			var dbRole = accountService.AddRoleAsync (newRole);

			// assert
			Assert.Equal (newRole.Name, dbRole.Result.Model.Name);
		}
	}
}
