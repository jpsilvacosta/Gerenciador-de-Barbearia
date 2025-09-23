﻿using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.User;
using BarberBoss.Domain.Services.LoggedUser;
using FluentValidation.Results;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Users.Update
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserUpdateOnlyRepository _repository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserUseCase(
            ILoggedUser loggedUser,
            IUserUpdateOnlyRepository repository,
            IUserReadOnlyRepository userReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _repository = repository;
            _userReadOnlyRepository = userReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestUpdateUserJson request)
        {
            var loggedUser = await _loggedUser.Get();

            await Validate(request, loggedUser.Email);

            var user = await _repository.GetById(loggedUser.Id);

            user.Name = request.Name;
            user.Email = request.Email;

            _repository.Update(user);

            await _unitOfWork.Commit();

        }

        private async Task Validate(RequestUpdateUserJson request, string currentEmail)
        {
            var validator = new UpdateUserValidator();

            var result = validator.Validate(request);
            
            if(currentEmail.Equals(request.Email) == false)
            {
                var userExists = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
                if (userExists)
                    result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
                
        }
    }
}
