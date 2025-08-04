using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Context;
using UserService.Application.Common.Models;
using UserService.Domain.Common;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Data;

namespace UserService.Application.Contexts.CtxUsers.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<User>>
    {


        private readonly IPasswordHasher _passwordHasher;

        private readonly UserServiceDbContext _context;

        public CreateUserCommandHandler(IPasswordHasher passwordHasher, UserServiceDbContext context)
        {
            _passwordHasher = passwordHasher;
            _context = context;
        }


        public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            Result<User> userResult = User.Create(request.Name, request.Surname, request.Email, request.Password, _passwordHasher);

            if(!userResult.Succeded)
                return userResult;

            try
            {
                _context.Add(userResult.Value);

                await _context.SaveChangesAsync();

            }catch(DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
            {
                using(LogContext.PushProperty("Email",userResult.Value.Email))
                using (LogContext.PushProperty("Id", userResult.Value.Id))
                {
                    Log.Error("Violating Unique Key Constraint");
                }

                Result<User> respondResult = Result<User>.Failure("Violating Unique Key Constraint");

                return respondResult;
            }

            return userResult;
        }
    }
}