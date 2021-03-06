﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdlinkCapstone.Models;
using EdlinkCapstone.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EdlinkCapstone.Controllers
{
    public class UserControllerBLL : Controller
    {
        //TODO: checking for  Duplicates using email
        //Currently creating or registering a student
        
        //This method will let users to login
        public User LogIn(string email, string passWord)
        {
            using (UserContext context = new UserContext())
            {
                UserValidationException exception = new UserValidationException();
                if (string.IsNullOrWhiteSpace(email))
                {
                    exception.SubExceptions.Add(new ArgumentNullException("Email was not provided."));
                }
                else if(context.Users.Where(x => x.Email == email).Count() != 1)
                {
                    exception.SubExceptions.Add(new InvalidOperationException("Email was not found"));
                }
                else
                {
                    email = email.Trim();
                }
                if (string.IsNullOrWhiteSpace(passWord))
                {
                    exception.SubExceptions.Add(new ArgumentNullException("PassWord was not provided."));
                }
                else if (context.Users.Where(x => x.Email == email && x.PassWord == passWord).Count() != 1)
                {
                    exception.SubExceptions.Add(new InvalidOperationException("Email or Password is mismatched"));
                }
                else
                {
                    passWord = passWord.Trim();
                }
                if (exception.SubExceptions.Count > 0)
                {
                    throw exception;
                }
                    var authUser = (context.Users.Where(x => x.Email == email && x.PassWord == passWord)).Single();
                    return authUser;
                }
        }
        //This method will let users to register for their kid
        public int RegisterUser(string firstName, string lastName, string email, string passWord)
        {
            int createdID;
            UserValidationException exception = new UserValidationException();
            if (string.IsNullOrWhiteSpace(firstName))
            {
                exception.SubExceptions.Add(new ArgumentNullException("First name was not provided."));
            }
            else
            {
                if (firstName.Any(x => char.IsDigit(x)))
                {
                    exception.SubExceptions.Add(new ArgumentException("First name cannot contain numbers."));
                }
                if (firstName.Length > 50)
                {
                    exception.SubExceptions.Add(new ArgumentOutOfRangeException("First name cannot be more than 50 characters long."));
                }
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                exception.SubExceptions.Add(new ArgumentNullException("Last name was not provided."));
            }
            else
            {
                if (lastName.Any(x => char.IsDigit(x)))
                {
                    exception.SubExceptions.Add(new ArgumentException("Last name cannot contain numbers."));
                }
                if (lastName.Length > 50)
                {
                    exception.SubExceptions.Add(new ArgumentOutOfRangeException("Last name cannot be more than 50 characters long."));
                }
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                exception.SubExceptions.Add(new ArgumentNullException("Email was not provided."));
            }
            else
            {
                email = email.Trim();
            }
            if (string.IsNullOrWhiteSpace(passWord))
            {
                exception.SubExceptions.Add(new ArgumentNullException("PassWord was not provided."));
            }
            else
            {
                passWord = passWord.Trim();
            }
            if (exception.SubExceptions.Count > 0)
            {
                throw exception;
            }
            User newUser = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PassWord = passWord
            };

            using (UserContext context = new UserContext())
            {
                context.Users.Add(newUser);
                context.SaveChanges();
                createdID = newUser.ID;
            }
           return createdID;
        }     
    }
}
