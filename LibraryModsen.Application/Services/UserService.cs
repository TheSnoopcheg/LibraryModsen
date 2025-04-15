using AutoMapper;
using LibraryModsen.Application.Abstractions.Services;
using LibraryModsen.Application.Contracts.User;
using LibraryModsen.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryModsen.Application.Services;

public class UserService(
        UserManager<User> manager,
        IMapper mapper) : IUserService
{
    private readonly UserManager<User> _manager = manager;
    private readonly IMapper _mapper = mapper;

    public async Task<UserResponse?> GetById(Guid Id)
    {
        var user = await _manager
            .Users
            .AsNoTracking()
            .Include(u => u.TakenBooks)
                .ThenInclude(b => b.Book)
            .FirstOrDefaultAsync(u => u.Id == Id);
        var userRes = _mapper.Map<UserResponse>(user);
        return userRes;
    }
    public async Task<IEnumerable<NotificationResponse>> GetNotifications(Guid userId)
    {
        var user = await _manager
            .Users
            .AsNoTracking()
            .Include(u => u.TakenBooks)
                .ThenInclude(b => b.Book)
            .FirstOrDefaultAsync(u => u.Id == userId);
        List<NotificationResponse> result = new List<NotificationResponse>();
        for(int i = 0; i < user?.TakenBooks?.Count; i++)
        {
            TimeSpan leftTime = user.TakenBooks[i].ExpirationTime - DateTime.UtcNow;
            if (leftTime > TimeSpan.FromDays(1))
            {
                continue;
            }
            result.Add(new NotificationResponse($"The return period for the book {user.TakenBooks[i].Book?.Title} is coming to an end. Time left: {leftTime.ToString(@"hh\:mm")}"));
        }
        return result;
    }
}
