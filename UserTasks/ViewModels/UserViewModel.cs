﻿using System.ComponentModel.DataAnnotations;

namespace UserTasks.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required"), StringLength(200, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 200 characters")]
        public string UserName { get; set; }

        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required"), StringLength(200, ErrorMessage = "Email must be at most 200 characters"), EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsLockedOut { get; set; }

        public string[] Roles { get; set; }
    }
}
