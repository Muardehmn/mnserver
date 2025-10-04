using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mnserver.Data;
using mnserver.Models;

namespace mnserver.Controllers
{
    [Route("[controller]")]
    public class ClientsController : Controller
    {
        private readonly AppDbContext _context;

        public ClientsController(AppDbContext context)
        {
            _context = context;
        }
        // GET: Client
        [HttpGet("api/select")]
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Client.ToListAsync());
            return Ok(await _context.Client.ToListAsync());
        }

        // GET: Client/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Client/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Client/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Kod,Name,Borcu")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Client/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Kod,Name,Borcu")] Client client)
        {
            if (id != client.Id)
            {
                Console.WriteLine($"ID Mismatch: {id} != {client.Id}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Client/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Client.FindAsync(id);
            if (client != null)
            {
                _context.Client.Remove(client);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Client.Any(e => e.Id == id);
        }
        //[HttpGet("add")]
        [HttpPost("add-link")] // Yeni bir route adı təyin edək və POST olaraq qeyd edək
        [ValidateAntiForgeryToken]
        public IActionResult AddWithLink(int kod, string name, float borcu)
        {
            var musteri = new Client
            {
                Kod = kod,
                Name = name,
                Borcu = borcu
            };

            _context.Client.Add(musteri);
            _context.SaveChanges();
            return Ok(musteri);
        }
        // POST: Clients/api
        [HttpPost("api/insert")] // Və ya istədiyiniz başqa bir route: məsələn, "json-create"
        public async Task<IActionResult> CreateApi([FromBody] Client client)
        {
            // Model yoxlanılır (Client modeli və onun xüsusiyyətlərindəki Data Annotations əsasında)
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Client.Add(client);
                    await _context.SaveChangesAsync();

                    // 201 Created Status Code və yaradılmış obyekti geri qaytarır
                    // Bu, API üçün ən yaxşı standartdır.
                    return CreatedAtAction(nameof(Details), new { id = client.Id }, client);
                }
                catch (Exception ex)
                {
                    // Verilənlər bazası xətası olarsa
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }

            // Əgər ModelState.IsValid deyilsə (məlumatlar Modelə uyğun gəlmirsə)
            return BadRequest(ModelState);
        }
    }
}
