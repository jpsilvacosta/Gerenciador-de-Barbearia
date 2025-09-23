﻿using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Domain.Services;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BarberBoss.Infrastructure.Services.LoggedUser
{
    public class LoggedUser : ILoggedUser
    {
        private readonly BarberBossDbContext _dbContext;
        private readonly ITokenProvider _tokenProvider;

        public LoggedUser(BarberBossDbContext dbContext, ITokenProvider tokenProvider)
        {
            _dbContext = dbContext;
            _tokenProvider = tokenProvider;
        }

        public async Task<User> Get()
        {
            string token = _tokenProvider.TokenOnRequest();

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

            return await _dbContext
                .Users
                .AsNoTracking()
                .FirstAsync(user => user.UserIdentifier == Guid.Parse(identifier));
        }
    }
}
