﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Cashed.Contract
{
    public interface ICommonModelQueries<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> GetByName(string name);
    }
}
