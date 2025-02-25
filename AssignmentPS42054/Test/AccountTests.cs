using NUnit.Framework;
using AssignmentPS42054.DAL;
using AssignmentPS42054.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssignmentPS42054.Test
{
    [TestFixture]
    public class AccountTests
    {
        private AccountDAL _accountDAL;
        private IConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            var config = new Dictionary<string, string>
            {
                {"ConnectionStrings:DefaultConnection", "Data Source=LAPTOP-79G7JAGG;;Initial Catalog=asm_net103;Trusted_Connection=True;TrustServerCertificate=True;"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(config)
                .Build();

            _configuration = configuration;
            _accountDAL = new AccountDAL(_configuration);
        }

        [Test]
        public void Register_User_ShouldReturnTrue_WhenUserIsNew()
        {
            var result = _accountDAL.Register("Khoa", "123", "Khoa@gmail.com", 0337429968);
            Assert.IsTrue(result, "User should't be registered successfully.");
        }

        [Test]
        public void Register_User_ShouldReturnFalse_WhenUserExists()
        {
            _accountDAL.Register("Customer102", "123", "Khoa@gmail.com", 0337429958);
            var result = _accountDAL.Register("Customer102", "123", "Khoa@gmail.com", 0337429958);
            Assert.IsFalse(result, "User should be registered succesfully.");
        }
        [Test]
        public void Register_InvalidEmail()
        {
            _accountDAL.Register("Customer103", "123", "abcccccc", 0337439948);
            var user = _accountDAL.Login("Customer103", "123");
            Assert.IsNull(user, "User should be able to log in.");
        }
        [Test]
        public void Register_LostInfomation()
        {
            var user =_accountDAL.Register("", "123", "Khoa@gmail.com", 0337439948);
            Assert.IsFalse(user, "User should be able to log in.");
        }


        [Test]
        public void Login_ValidCredentials_ShouldReturnUser()
        {
            _accountDAL.Register("Customer103", "123", "Khoa@gmail.com", 0337439948);
            var user = _accountDAL.Login("Customer103", "123");
            Assert.IsNotNull(user, "User should't be able to log in.");
        }
       
        [Test]
        public void Login_InvalidCredentials_ShouldReturnNull()
        {
            var user = _accountDAL.Login("Customer1", "abc");
            Assert.IsNull(user, "Login should't fail with invalid credentials.");
        }
        [Test]
        public void GetUserById_ValidId_ShouldReturnUser()
        {
            _accountDAL.Register("Customer105", "123", "Khoa@gmail.com", 123456789);
            int id = _accountDAL.GetIDbyUser("Customer105");
            var user = _accountDAL.GetUserById(id);
            Assert.IsNotNull(user, "User should't be found by ID.");
        }
        [Test]
        public void Login_UserAdmin()
        {
            var user = _accountDAL.Login("Admin", "123");
            Assert.IsNotNull(user, "Wrong pass or name");
        }
        [Test]
        public void Login_UserCustomer()
        {
            var user = _accountDAL.Login("Customer1", "123");
            Assert.IsNotNull(user, "Wrong pass or name");
        }
        [Test]
        public void Login_WrongPass()
        {
            var user = _accountDAL.Login("Customer1", "11111");
            Assert.IsNull(user, "Not Wrong Name or Password");
        }
        [Test]
        public void Login_WrongNameWrongPass()
        {
            var user = _accountDAL.Login("Customer1000", "abcyzx");
            Assert.IsNull(user, "Not Wrong name or password");
        }
        //[Test]
        //public void UpdateUser_ShouldModifyUserDetails()
        //{
        //    _accountDAL.Register("updateUser", "pass", "update@example.com", 123456789);
        //    int id = _accountDAL.GetIDbyUser("updateUser");
        //    var user = _accountDAL.GetUserById(id);
        //    user.FName = "Updated Name";
        //    _accountDAL.UpdateUser(user);
        //    var updatedUser = _accountDAL.GetUserById(id);
        //    Assert.AreEqual("Updated Name", updatedUser.FName, "User's name should be updated.");
        //}
    }
}
