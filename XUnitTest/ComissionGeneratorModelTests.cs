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
            Assert.False(PhoneNumberModel.Validate(number));
        }

        [Theory]
        [InlineData("111 111 111")]
        [InlineData("111-111-111")]
        [InlineData("+00 665 971 122")]
        [InlineData("123 123 123")]
        [InlineData("+48 123 123 123")]
        void TestPhoneNumberValidRegex(string number)
        {
            Assert.True(PhoneNumberModel.Validate(number));
        }

        //PostalCodeModel

        [Theory]
        [InlineData("9-311")]
        [InlineData("199-310")]
        [InlineData("99--111")]
        [InlineData("99-3111")]
        [InlineData("10-0 01")]
        [InlineData("x99-311x")]
        void TestPostalCodeInvalidRegex(string postalCode)
        {
            Assert.False(PostalCodeModel.Validate(postalCode));
        }      

        [Theory]
        [InlineData("10-100")]
        [InlineData("10100")]
        [InlineData("10 100")]
        void TestPostalCodeValidRegex(string postalCode)
        {
            Assert.True(PostalCodeModel.Validate(postalCode));
        }

        [Theory]
        [InlineData("123 123 123 123")]
        [InlineData("x7751251674x")]
        [InlineData("775y927y78y21")]
        [InlineData("444 444 55 22 2")]
        void TestNIPInvalidRegex(string NIPnumber)
        {
            Assert.False(NIPModel.Validate(NIPnumber));
        }

        [Theory]
        [InlineData("775 115 1661")]
        [InlineData("7751251677")]
        [InlineData("775-125-16-77")]
        [InlineData("775 105 16 77")]
        [InlineData("775105 16 77")]
        void TestNIPValidRegex(string NIPnumber)
        {
            Assert.True(NIPModel.Validate(NIPnumber));
        }


        [Theory]
        [InlineData("11111111")]
        [InlineData("1111111111")]
        [InlineData("11 11 11 11 11")]
        [InlineData("x111111111x")]
        [InlineData("111x111x111")]
        [InlineData("!@111!111!111!")]
        void TestREGONInValidRegex(string regonNumber)
        {
            Assert.False(RegonModel.Validate(regonNumber));
        }


        [Theory]
        [InlineData("111111111")]
        [InlineData("111 111 111")]
        void TestREGONValidRegex(string regonNumber)
        {
            Assert.True(RegonModel.Validate(regonNumber));
        }

        [Theory]
        [InlineData("david.jones@proseware.com")]
        [InlineData("d.j@server1.proseware.com")]
        [InlineData("jones@ms1.proseware.com")]
        [InlineData("js@proseware.com9")]
        [InlineData("js#internal@proseware.com")]
        [InlineData("\"j\\\"s\\\"\"@proseware.com")]
        [InlineData("jj.s@server1.wp.pl.com")]
        [InlineData("qwerty@gmail.com")]
        void TestEmailAddressValidRegex(string emailAddress)
        {
            Assert.True(EmailAddressModel.Validate(emailAddress));
        }

        [Theory]
        [InlineData("js@contoso.中国")]
        [InlineData("b@w.p")]
        [InlineData("bqw@gog")]
        [InlineData("@.pl")]
        [InlineData("askoas@.l")]
        [InlineData("d@.p")]
        void TestEmailAddressInvalidRegex(string emailAddress)
        {
            Assert.False(EmailAddressModel.Validate(emailAddress));
        }


    }
}
