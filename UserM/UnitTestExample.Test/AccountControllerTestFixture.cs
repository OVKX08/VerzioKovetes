using NUnit.Framework;
using System;
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


        
    }
}
