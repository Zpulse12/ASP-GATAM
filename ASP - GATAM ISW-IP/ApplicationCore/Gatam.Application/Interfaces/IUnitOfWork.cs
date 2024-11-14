﻿using Gatam.Domain;

namespace Gatam.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IGenericRepository<ApplicationModule> ModuleRepository { get; }
        public IGenericRepository<Question> QuestionRepository { get; }
        Task Commit();
    }
}
