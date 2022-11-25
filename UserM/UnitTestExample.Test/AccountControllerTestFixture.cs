using NUnit.Framework;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestExample.Controllers;

namespace UnitTestExample.Test
{
    public class AccountControllerTestFixture
    {

        [
            Test,
            TestCase("Password1",false),
            TestCase("mate1@gmail", false),
            TestCase("mate1gmail.com", false),
            TestCase("mate1.gmail.com",false),
            TestCase("mate1@gmail.com", true)
        ]
        public void TestValidateEmail(string email, bool expectedResult)
        {
            //Arrange
            var accountController = new AccountController();

            //Act
            var actualResult = accountController.ValidateEmail(email);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);

        }


        [   Test,
            TestCase("Password", false),
            TestCase("PASSWORD123", false),
            TestCase("password123",false),
            TestCase("Pw12", false),
            TestCase("Password123", true)
        ]

        public void TestPasswordValidation (string password, bool expectedResult)
        {
            //Arrange
            var accountController = new AccountController();

            //Act
            var actualResult = accountController.ValidatePassword(password);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [   Test,
            TestCase("mate1@gmail.com", "Aa123456"),
            TestCase("mate2@freemail.com", "Jelszo123")
        ]

        public void TestRegisterHappyPath(string email, string password)
        {
            //Arrange 
            var accountController = new AccountController();

            //Act 
            var actualResult = accountController.Register(email,password);

            //Assert
            Assert.AreEqual(email, actualResult.Email);
            Assert.AreEqual(password, actualResult.Password);
            Assert.AreNotEqual(Guid.Empty, actualResult.ID);
        }

        [   Test,
            TestCase("irf@uni-corvinus", "Abcd1234"),
    TestCase("irf.uni-corvinus.hu", "Abcd1234"),
    TestCase("irf@uni-corvinus.hu", "abcd1234"),
    TestCase("irf@uni-corvinus.hu", "ABCD1234"),
    TestCase("irf@uni-corvinus.hu", "abcdABCD"),
    TestCase("irf@uni-corvinus.hu", "Ab1234")

        ]
        public void TestRegisterValidateException(string email, string password)
        {

            //Arrange
            var accountController = new AccountController();

            //Act
            try
            {
                var actualResult = accountController.Register(email, password);
                Assert.Fail();
            }
            catch (Exception ex)
            {

                Assert.IsInstanceOf<ValidationException>(ex);
            }
        }
    }
}
