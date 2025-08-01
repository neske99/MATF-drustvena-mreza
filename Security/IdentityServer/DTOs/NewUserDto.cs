﻿using System.ComponentModel.DataAnnotations;

namespace IdentityService.DTOs
{
    public class NewUserDto
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
