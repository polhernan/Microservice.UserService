using MediatR;
using Serilog.Context;
using UserService.Domain.Entities;
using UserService.Infrastructure.Data;

namespace UserService.Application.Contexts.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<User>>
    {


        private readonly UserServiceDbContext _context;


        public UpdateUserCommandHandler(UserServiceDbContext context)
        {
            _context = context;
        }


        public async Task<Result<User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (request.GetUserId() == null)
            {
                using (LogContext.PushProperty("Email", request.Email))
                {
                    Log.Error("User is not provided when updating user.");
                }
                return Result<User>.Failure("User id was not provided");
            }

            User? user = _context.Users.FirstOrDefault(x => x.Email.Value == request.Email);

            if (user == null)
                return Result<User>.Failure($"No user with email {request.Email} found");

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                user.Name = request.Name;
            }

            if (!string.IsNullOrWhiteSpace(request.Surnames))
            {
                user.Surnames = request.Surnames;
            }

            await _context.SaveChangesAsync();

            return Result<User>.Success(user);
        }
    }
}
