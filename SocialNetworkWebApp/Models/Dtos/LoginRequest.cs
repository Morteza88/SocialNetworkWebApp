﻿using System.ComponentModel.DataAnnotations;

namespace SocialNetworkWebApp.Models.Dtos
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
