using SecurePasswordZBCOpg.Models;
using SecurePasswordZBCOpg.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurePasswordZBCOpg.Services
{
	public class AuthService
	{
		private readonly UserRepository _userRepository;
		private readonly PasswordHasher _passwordHasher;

		public AuthService(UserRepository userRepository, PasswordHasher passwordHasher)
		{
			_userRepository = userRepository;
			_passwordHasher = passwordHasher;
		}

		public void Register(string username, string password)
		{
			var salt = _passwordHasher.GenerateSalt();
			var hash = _passwordHasher.HashPassword(password, salt);

			var user = new User(username, salt, hash);
			_userRepository.SaveUser(user);
		}

		public bool Login(string username, string password)
		{
			var user = _userRepository.GetUserByUsername(username);
			if (user == null) return false;

			return _passwordHasher.VerifyPassword(password, user.Salt, user.PasswordHash);
		}
	}
}
