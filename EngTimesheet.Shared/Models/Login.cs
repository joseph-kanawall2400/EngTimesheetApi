using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EngTimesheetApi.Shared.Models
{
	public class Login
	{
		public int Id { get; set; }
		public byte[] PasswordHash { get; private set; }
		public byte[] PasswordSalt { get; private set; }
		public User User { get; set; }

		public void SetPassword(string password)
		{
			PasswordSalt = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
			PasswordHash = GenerateSaltedHash(Encoding.UTF8.GetBytes(password), PasswordSalt);
		}

		public bool CheckPassword(string password)
		{
			return PasswordHash.SequenceEqual(GenerateSaltedHash(Encoding.UTF8.GetBytes(password), PasswordSalt));
		}

		private byte[] GenerateSaltedHash(byte[] password, byte[] salt)
		{
			return new SHA256Managed().ComputeHash(password.Concat(salt).ToArray());
		}
	}
}
