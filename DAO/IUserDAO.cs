﻿using Physiosoft.Data;
using Physiosoft.DTO.User;

namespace Physiosoft.DAO
{
    public interface IUserDAO
    {
        void Insert(User user);
        User? Update(int id, User user);
        bool Delete(int id);
        User? GetById(int id);
        List<User> GetAll();
        Task SignUpUserAsync(UserSignupDTO request);
        Task<User?> LoginUserAsync(UserLoginDTO credentials);
        Task<User?> GetUserAsync(string username, string password);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmail(string email);
    }
}
