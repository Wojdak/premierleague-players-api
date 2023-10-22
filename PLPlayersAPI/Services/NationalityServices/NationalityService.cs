﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PLPlayersAPI.Data;
using PLPlayersAPI.Models;
using PLPlayersAPI.Models.DTOs;

namespace PLPlayersAPI.Services.NationalityServices
{
    public class NationalityService : INationalityService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public NationalityService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NationalityDTO>> GetAllNationalitiesAsync()
        {
            var nationalities = await _context.Nationalities.ToListAsync();

            var nationalitiesDTO = nationalities.Select(_mapper.Map<Nationality, NationalityDTO>).ToList();

            return nationalitiesDTO;
        }

        public async Task<NationalityDTO?> GetNationalityByIdAsync(int nationalityId)
        {
            var nationality = await _context.Nationalities.FirstOrDefaultAsync(n => n.NationalityId == nationalityId);

            if (nationality is null) return null;

            var nationalityDTO = _mapper.Map<Nationality, NationalityDTO>(nationality);

            return nationalityDTO;
        }
    }
}
