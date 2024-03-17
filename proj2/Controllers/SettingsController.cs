using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proj2.Models;

namespace proj2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly HRContext _context;

        public SettingsController(HRContext context)
        {
            _context = context;
        }

        // GET: api/Settings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Setting>>> GetSettings()
        {
            return await _context.Settings.ToListAsync();
        }

        // GET: api/Settings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Setting>> GetSetting(int id)
        {
            var setting = await _context.Settings.FindAsync(id);

            if (setting == null)
            {
                return NotFound();
            }

            return setting;
        }

        // PUT: api/Settings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSetting(int id, Setting setting)
        {
            var existingSetting = await _context.Settings.FindAsync(id);
            if(existingSetting == null)
            {
                return BadRequest();
            }
            existingSetting.Late = setting.Late;
            existingSetting.Plus = setting.Plus;
            existingSetting.HolidayDayTwo = setting.HolidayDayTwo;
            existingSetting.HolidayDayOne = setting.HolidayDayOne;

            _context.Entry(existingSetting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SettingExists(id))
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

        // POST: api/Settings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Setting>> PostSetting(Setting setting)
        {
            var existingSetting = await _context.Settings.FirstOrDefaultAsync();

            if (existingSetting != null)
            {              
                existingSetting.Late = setting.Late; 
                existingSetting.Plus = setting.Plus;    
                existingSetting.HolidayDayTwo= setting.HolidayDayTwo;
                existingSetting.HolidayDayOne= setting.HolidayDayOne;
                 _context.Update(existingSetting);
                await _context.SaveChangesAsync();

            }
            else
            {
                _context.Settings.Add(setting);
                await _context.SaveChangesAsync();

            }

            return CreatedAtAction("GetSetting", new { id = setting.Id }, setting);
        }

        // DELETE: api/Settings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSetting(int id)
        {
            var setting = await _context.Settings.FindAsync(id);
            if (setting == null)
            {
                return NotFound();
            }

            _context.Settings.Remove(setting);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SettingExists(int id)
        {
            return _context.Settings.Any(e => e.Id == id);
        }
    }
}
