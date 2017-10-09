﻿using System.Threading.Tasks;

namespace Logic.Cashed.Contract
{
    public interface IGenericModelCommands<T> where T : class
    {
        Task Update(T model);
        void Delete(int id);
    }
}