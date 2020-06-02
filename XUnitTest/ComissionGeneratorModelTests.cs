using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ClassLibrary;
using Xunit;

namespace XUnitTest
{
    public class ComissionGeneratorModelTests
    {
        [Fact]
        void AdditionTest()
        {
            int n = 2;
            int m = 4;
            int sum = n + m;
            Assert.Equal(6, sum);
        }

       
        /// PhoneNumberModel
       
      

        [Theory]
        [InlineData("111 11 1111")]
        [InlineData("544-555-.3~!")]
        [InlineData("xab,212:982")]
        [InlineData("+48+ 665 971 122")]
        [InlineData("kpoqw665-981 522")]
        [InlineData("123-435 678")]
        [InlineData("123-435 67812")]
        [InlineData("123-43534 678")]
        [InlineData("xx123-435 678xx")]
        [InlineData("xx 123-435 678 xx")]
        void TestPhoneNumberInvalidRegex(string number)
        {
            Assert.False(PhoneNumberModel.ValidateNumber(number));
        }

        [Theory]
        [InlineData("111 111 111")]
        [InlineData("111-111-111")]
        [InlineData("+00 665 971 122")]
        void TestPhoneNumberValidRegex(string number)
        {
            Assert.True(PhoneNumberModel.ValidateNumber(number));
        }

        //PostalCodeModel

        [Theory]
        [InlineData("9-311")]
        [InlineData("199-310")]
        [InlineData("99311")]
        [InlineData("11 311")]
        [InlineData("99--111")]
        [InlineData("99-3111")]
        void TestPostalCodeInvalidRegex(string postalCode)
        {
            Assert.False(PostalCodeModel.Validate(postalCode));
        }
        

        [Theory]
        [InlineData("10-100")]
        void TestPostalCodeValidRegex(string postalCode)
        {
            Assert.True(PostalCodeModel.Validate(postalCode));
        }

    }
}
