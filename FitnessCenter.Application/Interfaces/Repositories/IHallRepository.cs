using FitnessCenter.Application.Other;
using FitnessCenter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Interfaces.Repositories
{
    public interface IHallRepository
    {
        Task<List<Hall>> GetAllAsync();
        Task<Hall?> GetByIdAsync(int id);
    }
}
