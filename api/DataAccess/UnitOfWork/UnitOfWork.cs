using DataAccess.Repo;
using Domain.Entities.User;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        public UnitOfWork(

            IUserRepository userRepo,
            ISetupRepository setupRepo)
        {

            Users = userRepo;
            Setup = setupRepo;

        }

        public IUserRepository Users { get; private set; }
        public ISetupRepository Setup { get; private set; }


    }
}
