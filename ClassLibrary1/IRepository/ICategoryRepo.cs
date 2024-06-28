﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DBData;

namespace Interfaces.IRepository
{
    public interface ICategoryRepo : IReposBase<Category>
    {
        public Task<Category> GetCategoryWithDetailsAsync(int id);
    }
}
