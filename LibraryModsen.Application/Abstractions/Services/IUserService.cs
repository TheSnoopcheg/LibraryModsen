﻿using LibraryModsen.Application.Contracts.User;

namespace LibraryModsen.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<UserResponse?> GetById(Guid Id, CancellationToken cancelToken = default);
        Task<IEnumerable<NotificationResponse>> GetNotifications(Guid userId, CancellationToken cancelToken = default);
    }
}