﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeauteAPI.Models;

namespace BeauteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GendersController : ControllerBase
    {
        private readonly BeautysaloonContext _context;

        public GendersController(BeautysaloonContext context)
        {
            _context = context;
        }

        // GET: api/Genders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gender>>> GetGender()
        {
            return await _context.Gender.Include(x=>x.Client).ToListAsync();
        }

        // GET: api/Genders/5
        [HttpGet("{code}")]
        public async Task<ActionResult<Gender>> GetGender(string code)
        {
            var gender = await _context.Gender.FindAsync(code);

            if (gender == null)
            {
                return NotFound();
            }

            return gender;
        }

        // PUT: api/Genders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{code}")]
        public async Task<IActionResult> PutGender(string code, Gender gender)
        {
            if (code != gender.Code)
            {
                return BadRequest();
            }

            _context.Entry(gender).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenderExists(code))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Genders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Gender>> PostGender(Gender gender)
        {
            _context.Gender.Add(gender);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GenderExists(gender.Code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetGender", new { code = gender.Code }, gender);
        }

        // DELETE: api/Genders/5
        [HttpDelete("{code}")]
        public async Task<ActionResult<Gender>> DeleteGender(string code)
        {
            var gender = await _context.Gender.FindAsync(code);
            if (gender == null)
            {
                return NotFound();
            }

            _context.Gender.Remove(gender);
            await _context.SaveChangesAsync();

            return gender;
        }

        private bool GenderExists(string code)
        {
            return _context.Gender.Any(e => e.Code == code);
        }
    }
}
