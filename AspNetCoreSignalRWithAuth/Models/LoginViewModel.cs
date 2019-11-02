﻿using System.ComponentModel.DataAnnotations;

namespace AspNetCoreSignalRWithAuth.Models
{
    public class LoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, DataType(DataType.Password), MinLength(4)]
        public string Password { get; set; }

    }
}
