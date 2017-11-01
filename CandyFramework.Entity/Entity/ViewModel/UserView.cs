﻿using CandyFramework.Core.Enum;
using CandyFramework.Entity.Entity.Entity;
using CandyFramework.Entity.Interface.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyFramework.Entity.Entity.ViewModel
{
    public sealed class UserView : Base.User, IUserView
    {
        public string ProfilPhotoBase64 { get; set; }
        public string FullName { get; set; }

        public UserView()
        {

        }
        public UserEntity Map()
        {
            UserEntity tempObject = (UserEntity)(Base.User)this;
            tempObject.ProfilPhoto = Convert.FromBase64String(this.ProfilPhotoBase64);

            return tempObject;
        }
    }
}