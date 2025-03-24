using SecurePasswordZBCOpg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace SecurePasswordZBCOpg.Repositorys
{
	public class UserRepository
	{
		private readonly string _filePath;

		public UserRepository(string filePath)
		{
			_filePath = filePath;
		}

		public void SaveUser(User user)
		{
			string line = $"{user.Username}|{Convert.ToBase64String(user.Salt)}|{Convert.ToBase64String(user.PasswordHash)}";
			File.AppendAllLines(_filePath, new[] { line });
		}

		public User GetUserByUsername(string username)
		{
			if (!File.Exists(_filePath)) return null;

			foreach (var line in File.ReadAllLines(_filePath))
			{
				var parts = line.Split('|');
				if (parts.Length != 3) continue;

				if (parts[0] == username)
				{
					return new User(
						parts[0],
						Convert.FromBase64String(parts[1]),
						Convert.FromBase64String(parts[2])
					);
				}
			}

			return null;
		}
	}
}
