﻿using NUnit.Framework;
using ForumDEG.ViewModels;
using ForumDEG.Utils;
using Moq;
using ForumDEG.Interfaces;

namespace Tests {
    public class UserRegistrationViewModelTests {

        private UserRegistrationViewModel viewModel;
        private Mock<IPageService> _pageService;

        [SetUp]
        public void Setup() {
            _pageService = new Mock<IPageService>();
            viewModel = new UserRegistrationViewModel(_pageService.Object);

            viewModel.NameIn = "Name";
            viewModel.EmailIn = "email@email.com";
            viewModel.RegistrationIn= "123456789";
            viewModel.PasswordIn = "Pb123456785";
            viewModel.CourseIn = "course";
            viewModel.UserTypeIn = 1;
        }

        [Test()]
        public void UserRegistrationViewModelTests_HasEmptySpace_NameInBlank() {
            viewModel.NameIn = "";
           Assert.True(viewModel.HasEmptySpace());
        }

        [Test()]
        public void UserRegistrationViewModelTests_HasEmptySpace_EmailInBlank() {
            viewModel.EmailIn = "";
            Assert.True(viewModel.HasEmptySpace());
        }

        [Test()]
        public void UserRegistrationViewModelTests_HasEmptySpace_RegistrationInBlank() {
            viewModel.RegistrationIn = "";
            Assert.True(viewModel.HasEmptySpace());
        }

        [Test()]
        public void UserRegistrationViewModelTests_HasEmptySpace_PasswordInBlank() {
            viewModel.PasswordIn = "";
            Assert.True(viewModel.HasEmptySpace());
        }

        [Test()]
        public void UserRegistrationViewModelTests_HasEmptySpace_BlankCourse() {
            viewModel.UserTypeIn = 0;
            viewModel.CourseIn = "";
            Assert.True(viewModel.HasEmptySpace());
        }

        [Test()]
        public void UserRegistrationViewModelTests_HasEmptySpace_NoBlankFields() {
            Assert.False(viewModel.HasEmptySpace());
        }

        [Test()]
        public void UserRegistrationViewModelTests_ValidateRegisterNumber_ValidRegisterNumber() {
            Assert.True(viewModel.ValidateRegisterNumber());
            Assert.True(viewModel.ActivityIndicator);
        }

        [Test()]
        public void UserRegistrationViewModelTests_ValidateRegisterNumber_InvalidRegisterNumber1() {

            //less numbers than 9
            viewModel.RegistrationIn = "123";
            Assert.False(viewModel.ValidateRegisterNumber());
            // too many numbers
            viewModel.RegistrationIn = "1234678910111213";
            Assert.False(viewModel.ValidateRegisterNumber());
            Assert.False(viewModel.ActivityIndicator);
        }

        [Test()]
        public void UserRegistrationViewModelTests_ValidateEmail_ValidEmail() {
            Assert.True(viewModel.ValidateEmail());
        }

        [Test()]
        public void UserRegistrationViewModelTests_ValidateEmail_InvalidEmail() {
            viewModel.EmailIn = "email";
            Assert.False(viewModel.ValidateEmail());

            viewModel.EmailIn = "email@email";
            Assert.False(viewModel.ValidateEmail());

            viewModel.EmailIn = "email@";
            Assert.False(viewModel.ValidateEmail());
        }


        [Test()]
        public void UserRegistrationViewModelTests_ValidatePassword_ValidPassWord() {
            Assert.True(viewModel.ValidatePassword());
        }

        [Test()]
        public void UserRegistrationViewModelTests_ValidatePassword_invalidPassWord() {
            //less chars than 8
            viewModel.PasswordIn = "Pb1";
            Assert.False(viewModel.ValidatePassword());

            //too many chars
            viewModel.PasswordIn = "Pb123456789012345";
            Assert.False(viewModel.ValidatePassword());

            //no uppercase chars
            viewModel.PasswordIn = "pb1234567";
            Assert.False(viewModel.ValidatePassword());

            //no lowercase chars
            viewModel.PasswordIn = "PB1234567";
            Assert.False(viewModel.ValidatePassword());

            //no numbers
            viewModel.PasswordIn = "Abcdefghij";
            Assert.False(viewModel.ValidatePassword());
        }

        [Test()]
        public void UserRegistrationViewModelTests_CleanFields() {
            viewModel.CleanFields();
            Assert.Null(viewModel.NameIn);
            Assert.Null(viewModel.PasswordIn);
            Assert.Null(viewModel.RegistrationIn);
            Assert.Null(viewModel.CourseIn);
            Assert.Null(viewModel.EmailIn);
            Assert.AreEqual(0, viewModel.UserTypeIn);
        }

        [Test()]
        public void UserRegistrationViewModelTests_UserTypeIandIsCoordConection() {
            Assert.AreEqual(viewModel.UserTypeIn == 0, viewModel.IsCoord);
            viewModel.UserTypeIn++;
            Assert.AreEqual(viewModel.UserTypeIn == 0, viewModel.IsCoord);
        }

    }
}
