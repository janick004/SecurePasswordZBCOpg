using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurePasswordZBCOpg.Models
{
	public class User
	{
		public string Username { get; set; }
		public byte[] Salt { get; set; }
		public byte[] PasswordHash { get; set; }

		public User(string username, byte[] salt, byte[] passwordHash)
		{
			Username = username;
			Salt = salt;
			PasswordHash = passwordHash;
		}
	}
}
