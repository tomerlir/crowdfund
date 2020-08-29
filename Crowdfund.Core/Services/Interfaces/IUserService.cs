using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Options.UserOptions;
using System.Linq;

namespace Crowdfund.Core.Services.Interfaces
{
    public interface IUserService
    {
        Result<int> LoginUser(CreateUserOptions createUserOptions);
        User GetUserById(int? id);
        IQueryable<User> SearchUser(SearchUserOptions options);
        Result<bool> UpdateUser(int? userId, UpdateUserOptions options);
    }
}