﻿using CandyFramework.Core.Concrete.Common;
using CandyFramework.DataAccessLayer.Concrete.EntityFramework.Base;
using CandyFramework.DataAccessLayer.Concrete.EntityFramework.Context;
using CandyFramework.DataAccessLayer.Interface;
using CandyFramework.Entity.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyFramework.DataAccessLayer.Concrete.EntityFramework
{
    public class UserEfRepository : EntityFrameworkRepository<UserEntity, CandyContext>, IUserRepository
    {
        public UserEfRepository()
        {

        }
        public override IQueryable<UserEntity> AsQueryable()
        {
            return base._context.Users.Include("UserGroup");
        }
        /// <summary>
        /// Kullanıcı adı ve şifre ile kullanıcı bulma
        /// </summary>
        /// <param name="userName">Kullanıcı adı veya email</param>
        /// <param name="password">Şifrelenmiş şifre</param>
        /// <returns></returns>
        public UserEntity GetUserByPassword(string userName, string password)
        {
            var user = base.First(x => (x.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase) || x.Email.Equals(userName, StringComparison.InvariantCultureIgnoreCase)));
            if (user.Password.Decrypt() == password)
            {
                return user;
            }
            else
            {
                return default(UserEntity);
            }
        }

        public List<UserEntity> SearhUsers(string searchText)
        {
            return base.Gets(x => x.FirstName.Contains(searchText) || x.LastName.Contains(searchText) ||
                        x.UserName.Contains(searchText) || x.Email.Contains(searchText))
                .ToList();
        }

        public bool UserNamePasswordControl(string userName, string password)
        {
            return GetUserByPassword(userName, password) != null;
        }
    }
}
