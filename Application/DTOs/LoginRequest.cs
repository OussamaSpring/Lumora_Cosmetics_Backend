<<<<<<< HEAD
﻿using System.ComponentModel;

namespace Application.DTOs;

public record LoginRequest(
    // should add regex
    string UsernameOrEmail, 
    [property: PasswordPropertyText] string Password);
=======
﻿namespace Application.DTOs;

//public record LoginRequest(
//    // should add regex
//    string UsernameOrEmail,
//    [property: PasswordPropertyText] string Password);


//public record LoginRequest(
//    // should add regex
//    [property: Required(ErrorMessage = "Username or email is required")] string UsernameOrEmail,
//    [property: Required(ErrorMessage = "Password is required")] string Password);


public record LoginRequest(
    // should add regex
    string UsernameOrEmail,
    string Password);
>>>>>>> origin/main
