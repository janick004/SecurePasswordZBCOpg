using System;
using System.IO;
using Xunit;
using SecurePasswordZBCOpg;
using SecurePasswordZBCOpg.Models;
using SecurePasswordZBCOpg.Services;
using SecurePasswordZBCOpg.Repositorys;

namespace SecurePasswordZBCOpg.Tests
{
	public class PasswordHasherTests
	{
		private readonly PasswordHasher _passwordHasher;
		private readonly string _testFilePath = "test_users.txt";

		public PasswordHasherTests()
		{
			_passwordHasher = new PasswordHasher();

			// Clean up file before each test run
			if (File.Exists(_testFilePath))
			{
				File.Delete(_testFilePath);
			}
		}

		[Fact]
		public void GenerateSalt_ShouldReturn16ByteArray()
		{
			byte[] salt = _passwordHasher.GenerateSalt();
			Assert.Equal(16, salt.Length);
		}

		[Fact]
		public void HashPassword_ShouldReturnConsistentHash_WithSameSalt()
		{
			string password = "Test123!";
			byte[] salt = _passwordHasher.GenerateSalt();

			byte[] hash1 = _passwordHasher.HashPassword(password, salt);
			byte[] hash2 = _passwordHasher.HashPassword(password, salt);

			Assert.Equal(hash1, hash2);
		}

		[Fact]
		public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
		{
			string password = "CorrectHorseBatteryStaple";
			byte[] salt = _passwordHasher.GenerateSalt();
			byte[] hash = _passwordHasher.HashPassword(password, salt);

			bool result = _passwordHasher.VerifyPassword(password, salt, hash);
			Assert.True(result);
		}

		[Fact]
		public void VerifyPassword_ShouldReturnFalse_ForWrongPassword()
		{
			string originalPassword = "Original123";
			string wrongPassword = "Wrong456";

			byte[] salt = _passwordHasher.GenerateSalt();
			byte[] hash = _passwordHasher.HashPassword(originalPassword, salt);

			bool result = _passwordHasher.VerifyPassword(wrongPassword, salt, hash);
			Assert.False(result);
		}

		[Fact]
		public void AuthService_ShouldRegisterAndLoginSuccessfully()
		{
			UserRepository repo = new UserRepository(_testFilePath);
			AuthService authService = new AuthService(repo, _passwordHasher);

			string username = "testuser";
			string password = "TestPassword123!";

			authService.Register(username, password);
			bool loginResult = authService.Login(username, password);

			Assert.True(loginResult);
		}

		[Fact]
		public void AuthService_ShouldFailLogin_WithIncorrectPassword()
		{
			UserRepository repo = new UserRepository(_testFilePath);
			AuthService authService = new AuthService(repo, _passwordHasher);

			string username = "testuser2";
			string password = "CorrectPassword";
			string wrongPassword = "WrongPassword";

			authService.Register(username, password);
			bool loginResult = authService.Login(username, wrongPassword);

			Assert.False(loginResult);
		}

		[Fact]
		public void UserRepository_ShouldReturnNull_ForNonExistingUser()
		{
			UserRepository repo = new UserRepository(_testFilePath);
			User user = repo.GetUserByUsername("nonexistent");

			Assert.Null(user);
		}
	}
}
