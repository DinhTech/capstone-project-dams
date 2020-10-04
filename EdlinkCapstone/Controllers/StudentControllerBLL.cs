﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdlinkCapstone.Models;
using EdlinkCapstone.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EdlinkCapstone.Controllers
{
    public class StudentControllerBLL : Controller
    {
        public int CreateStudent(string firstName, string lastName, string address, string email, string phoneNumber, DateTime dateOfBirth, int schoolID)
        {
            int createdID;
            StudentValidationException exception = new StudentValidationException();
            if (string.IsNullOrWhiteSpace(firstName))
            {
                exception.SubExceptions.Add(new ArgumentNullException(nameof(firstName), "First name was not provided."));
            }
            else
            {
                if (firstName.Any(x => char.IsDigit(x)))
                {
                    exception.SubExceptions.Add(new ArgumentException(nameof(firstName), "First name cannot contain numbers."));
                }
                if (firstName.Length > 50)
                {
                    exception.SubExceptions.Add(new ArgumentOutOfRangeException(nameof(firstName), "First name cannot be more than 50 characters long."));
                }
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                exception.SubExceptions.Add(new ArgumentNullException(nameof(lastName), "Last name was not provided."));
            }
            else
            {
                if (lastName.Any(x => char.IsDigit(x)))
                {
                    exception.SubExceptions.Add(new ArgumentException(nameof(lastName), "Last name cannot contain numbers."));
                }
                if (lastName.Length > 50)
                {
                    exception.SubExceptions.Add(new ArgumentOutOfRangeException(nameof(lastName), "Last name cannot be more than 50 characters long."));
                }
            }
            if (string.IsNullOrWhiteSpace(address))
            {
                exception.SubExceptions.Add(new ArgumentNullException(nameof(address), "Address was not provided."));
            }
            else
            {
                address = address.Trim();
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                exception.SubExceptions.Add(new ArgumentNullException(nameof(email), "Email was not provided."));
            }
            else
            {
                email = email.Trim();
            }
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                exception.SubExceptions.Add(new ArgumentNullException(nameof(phoneNumber), "Phone Number was not provided."));
            }
            if (dateOfBirth == null)
            {
                exception.SubExceptions.Add(new ArgumentNullException(nameof(dateOfBirth), "Date of Birth was not provided."));
            }
            else
            {
                // Check for phone number formatting (feel free to use RegEx or any other method).
                // Has to be in the else branch to avoid null reference exceptions.
                int temp;
                string[] phoneParts = phoneNumber.Split('-');
                if (!(
                    phoneParts[0].Length == 3 &&
                    int.TryParse(phoneParts[0], out temp) &&
                    phoneParts[1].Length == 3 &&
                    int.TryParse(phoneParts[1], out temp) &&
                    phoneParts[2].Length == 4 &&
                    int.TryParse(phoneParts[2], out temp)
                    ))
                {
                    exception.SubExceptions.Add(new ArgumentException(nameof(phoneNumber), "Phone Number number was not in a valid format."));
                }
            }
            if (exception.SubExceptions.Count > 0)
            {
                throw exception;
            }
            Student myStudent = new Student()
            {
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                Email = email,
                PhoneNumber = phoneNumber,
                DateOfBirth = dateOfBirth,
                SchoolID = schoolID
            };

            using (SchoolContext context = new SchoolContext())
            {
                context.Students.Add(myStudent);
                context.SaveChanges();
                createdID = myStudent.ID;
            }
           return createdID;
        }

        public List<Student> GetStudents()
        {
            List<Student> students;
            using (SchoolContext context = new SchoolContext())
            {
                students = context.Students.ToList();
            }
            return students;
        }
    }
}