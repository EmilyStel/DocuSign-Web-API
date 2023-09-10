using System;
using BL.Repositories;
using Castle.Core.Logging;
using DAL;
using DAL.Intefaces;
using DocuSign.Controllers;
using DocuSign.Interfaces;
using DocuSign.Models;
using FakeItEasy;
using FluentAssertions;

namespace DocuSign.UnitTests
{
	public class UserRepositoryTests
	{
        private readonly IStorage _storage;
        private readonly IUserStorageMapper _storageMapper;

        public UserRepositoryTests()
        {
            _storageMapper = A.Fake<IUserStorageMapper>();
            _storage = A.Fake<IStorage>();
        }

        [Fact]
        public void CreateUser()
        {
            var user = A.Fake<User>();
            var repo = new UserRepository(_storage, _storageMapper);

            repo.DeleteUser(user.Name);


            Console.WriteLine( user.Name);

            var result = repo.CreateUser(user.Name, user.LastName, "ff@gmail.com");

            //result.Name.Should().BeSameAs(user.Name);
            //result.LastName.Should().BeSameAs(user.LastName);
            //result.Email.Should().BeSameAs(user.Email);

            repo.DeleteUser(user.Name);

            Assert.IsType<User>(result);
        }




        [Fact]
        public void GetUser()
        {
            var user = A.Fake<User>();
            var repo = new UserStorageMapper();

            var result = repo.GetUser(user.Name);

            Assert.IsType<User>(result);

            //repo.DeleteUser(user.Name);
        }


        //[Fact]
        //public void GetUsers()
        //{
        //    var user = A.Fake<User>();
        //    var repo = new UserRepository(_storage, _storageMapper);

        //    repo.DeleteUser(user.Name);


        //    Console.WriteLine(user.Name);

        //    var result = repo.CreateUser(user.Name, user.LastName, "ff@gmail.com");

        //    result.Name.Should().BeSameAs(user.Name);
        //    result.LastName.Should().BeSameAs(user.LastName);
        //    result.Email.Should().BeSameAs(user.Email);

        //    repo.DeleteUser(user.Name);
        //}
    }
}

