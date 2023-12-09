﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Physiosoft.Data;
using Physiosoft.DTO.User;
using Physiosoft.Security;
using Physiosoft.Service;

namespace Physiosoft.DAO
{
    public class UserDaoImpl : IUserDAO
    {
        private readonly PhysiosoftDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserAuthenticationService _userAuthenticationService;

        public UserDaoImpl(PhysiosoftDbContext dbContext, IMapper mapper, UserAuthenticationService userAuthenticationService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userAuthenticationService = userAuthenticationService;
        }

        public bool Delete(int id)
        {
            var userToDelete = _dbContext.Users.Find(id);

            if (userToDelete != null)
            {
                _dbContext.Users.Remove(userToDelete);

                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public List<User> GetAll()
        {
            var users = _dbContext.Users.ToList();
            return _mapper.Map<List<User>>(users);
        }

        public User? GetById(int id)
        {
            var userToGet = _dbContext.Users.Find(id);
            return _mapper.Map<User>(userToGet);
        }

        public void Insert(User user)
        {
            var userToInsert = _mapper.Map<User>(user);

            if (userToInsert != null)
            {
                _dbContext.Users.Add(userToInsert);
                _dbContext.SaveChanges();
            }

            // TODO
            // Throw exception error if its null
        }

        public User? Update(int id, User user)
        {
            var userToUpdate = _dbContext.Users.Find(id);

            if (userToUpdate != null)
            {
                _mapper.Map(user, userToUpdate);

                _dbContext.SaveChanges();
            }

            return _mapper.Map<User>(userToUpdate);
        }

        public async Task<User?> GetUserAsync(string username, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user is null) return null;

            if (!EncryptionUtil.IsValidPassword(password, user.Password!)) return null;

            return user;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbContext.Users.Where(x  => x.Username == username).FirstOrDefaultAsync();
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _dbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task SignUpUserAsync(UserSignupDTO request)
        {
            if(!await _dbContext.Users.Sign)
        }

        public async Task<User?> LoginUserAsync(UserLoginDTO credentials)
        {
            throw new NotImplementedException();
        }
    }
}
