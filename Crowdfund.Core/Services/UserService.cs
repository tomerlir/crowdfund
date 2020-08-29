using Crowdfund.Core.Data;
using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Interfaces;
using Crowdfund.Core.Services.Options.UserOptions;
using System;
using System.Linq;

namespace Crowdfund.Core.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public Result<int> LoginUser(CreateUserOptions createUserOptions)
        {
            if (createUserOptions == null || string.IsNullOrWhiteSpace(createUserOptions.Email))
            {
                return Result<int>.Failed(StatusCode.BadRequest, "Please enter a valid E-mail");
            }

            try
            {
                var existingUser = SearchUser(new SearchUserOptions
                {
                    Email = createUserOptions.Email
                }).SingleOrDefault();

                if (existingUser != null) return Result<int>.Succeed(existingUser.UserId);

                var user = new User()
                {
                    FirstName = createUserOptions.FirstName,
                    LastName = createUserOptions.LastName,
                    Address = createUserOptions.Address
                };

                var validEmail = user.IsValidEmail(createUserOptions.Email);

                if (validEmail)
                {
                    user.Email = createUserOptions.Email;
                }
                else
                {
                    return Result<int>.Failed(StatusCode.BadRequest, "Invalid Email");
                }

                _context.Add(user);

                var rows = 0;

                try
                {
                    rows = _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Result<int>.Failed(StatusCode.InternalServerError, ex.Message);
                }

                return rows <= 0
                    ? Result<int>.Failed(StatusCode.InternalServerError, "User Could Not Be Created")
                    : Result<int>.Succeed(user.UserId);
            }
            catch (Exception ex)
            {
                return Result<int>.Failed(StatusCode.InternalServerError, ex.Message);
            }
        }


        public User GetUserById(int? id)
        {
            return id != null ? _context.Set<User>().SingleOrDefault(u => u.UserId == id) : null;
        }

        public IQueryable<User> SearchUser(SearchUserOptions options)
        {
            if (options == null)
            {
                return null;
            }

            var query = _context.Set<User>().AsQueryable();

            if (options.UserId != null)
            {
                query = query.Where(u => u.UserId == options.UserId);
            }

            if (!string.IsNullOrWhiteSpace(options.FirstName))
            {
                query = query.Where(c => c.FirstName == options.FirstName);
            }

            if (!string.IsNullOrWhiteSpace(options.LastName))
            {
                query = query.Where(c => c.LastName == options.LastName);
            }

            if (!string.IsNullOrWhiteSpace(options.Email))
            {
                query = query.Where(c => c.Email == options.Email);
            }

            if (!string.IsNullOrWhiteSpace(options.Address))
            {
                query = query.Where(c => c.Address == options.Address);
            }

            if (options.CreatedAt != null)
            {
                query = query.Where(c => c.CreatedAt == options.CreatedAt);
            }

            if (options.CreatedFrom != null)
            {
                query = query.Where(c => c.CreatedAt >= options.CreatedFrom);
            }

            if (options.CreatedTo != null)
            {
                query = query.Where(c => c.CreatedAt <= options.CreatedTo);
            }

            return query;
        }

        public Result<bool> UpdateUser(int? userId, UpdateUserOptions options)
        {
            options.Email = options.Email?.Trim();
            options.FirstName = options.FirstName?.Trim();
            options.LastName = options.LastName?.Trim();
            options.Address = options.Address?.Trim();

            try
            {
                var user = _context.Set<User>().SingleOrDefault(u => u.UserId == userId);

                if (user == null)
                {
                    return Result<bool>.Failed(StatusCode.NotFound,
                        "User not found");
                }

                if (!string.IsNullOrWhiteSpace(options.FirstName))
                {
                    user.FirstName = options.FirstName;
                }

                if (!string.IsNullOrWhiteSpace(options.Email))
                {
                    var validEmail = user.IsValidEmail(options.Email);

                    if (validEmail)
                    {
                        user.Email = options.Email;
                    }
                    else
                    {
                        return Result<bool>.Failed(StatusCode.BadRequest, "Invalid Email");
                    }

                    user.Email = options.Email;
                }
                else
                {
                    return Result<bool>.Failed(StatusCode.BadRequest, "Email cannot be empty");
                }

                if (!string.IsNullOrWhiteSpace(options.LastName))
                {
                    user.LastName = options.LastName;
                }

                if (!string.IsNullOrWhiteSpace(options.Address))
                {
                    user.Address = options.Address;
                }

                var rows = 0;

                try
                {
                    rows = _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Result<bool>.Failed(StatusCode.InternalServerError, ex.Message);
                }

                return rows <= 0
                    ? Result<bool>.Failed(StatusCode.BadRequest, "No Changes Applied")
                    : Result<bool>.Succeed(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(StatusCode.InternalServerError, ex.Message);
            }
        }
    }
}