﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CandyFramework.Core.Interface.Entity
{
    public interface IUpdateEntity
    {
        string UpdateUser { get; set; }
        DateTime UpdateDate { get; set; }
    }
}
