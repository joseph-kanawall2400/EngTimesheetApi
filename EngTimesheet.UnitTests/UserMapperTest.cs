using EngTimesheet.Shared;
using EngTimesheet.Shared.Mappers;
using EngTimesheet.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EngTimesheet.UnitTests
{
	[TestClass]
	public class UserMapperTest
	{
		private bool _manager = true;
		private int _id = 1;
		private string _email = "email";
		private string _firstName = "firstName";
		private string _lastName = "lastName";
		private DateTime _registered = new DateTime(2000, 1, 1);
		private DateTime _deactivated = new DateTime(2001, 2, 2);

		[TestMethod]
		public void TestMap_AccountRegisterDTO()
		{
			AccountRegisterDTO dto = new AccountRegisterDTO()
			{
				Email = _email,
				FirstName = _firstName,
				LastName = _lastName
			};

			User user = UserMapper.Map(dto);

			Assert.AreEqual(_email, user.Email);
			Assert.AreEqual(_firstName, user.FirstName);
			Assert.AreEqual(_lastName, user.LastName);
		}

		[TestMethod]
		public void TestMap_UserDTO()
		{
			UserDTO dto = new UserDTO()
			{
				Id = _id,
				Manager = _manager,
				Email = _email,
				FirstName = _firstName,
				LastName = _lastName,
				Registered = _registered,
				Deactivated = _deactivated
			};

			User user = UserMapper.Map(dto);

			Assert.AreEqual(_id, user.Id);
			Assert.AreEqual(_manager, user.Manager);
			Assert.AreEqual(_email, user.Email);
			Assert.AreEqual(_firstName, user.FirstName);
			Assert.AreEqual(_lastName, user.LastName);
			Assert.AreEqual(_registered.FirstOfMonth(), user.Registered);
			Assert.AreEqual(_deactivated.FirstOfMonth(), user.Deactivated);
		}

		[TestMethod]
		public void TestMapToUserDTO()
		{
			User user = new User()
			{
				Id = _id,
				Manager = _manager,
				Email = _email,
				FirstName = _firstName,
				LastName = _lastName,
				Registered = _registered,
				Deactivated = _deactivated
			};

			UserDTO dto = UserMapper.MapToUserDTO(user);

			Assert.AreEqual(_id, dto.Id);
			Assert.AreEqual(_manager, dto.Manager);
			Assert.AreEqual(_email, dto.Email);
			Assert.AreEqual(_firstName, dto.FirstName);
			Assert.AreEqual(_lastName, dto.LastName);
			Assert.AreEqual(_registered.FirstOfMonth(), dto.Registered);
			Assert.AreEqual(_deactivated.FirstOfMonth(), dto.Deactivated);
		}
	}
}
