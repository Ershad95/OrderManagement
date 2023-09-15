﻿using Application.Dto;
using MediatR;

namespace Application.Features.AddUser;

public class AddUserCommand : IRequest<TokenDto>
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
}