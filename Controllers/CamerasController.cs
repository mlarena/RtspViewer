using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RtspViewer.Models;
using RtspViewer.Services;

namespace RtspViewer.Controllers
{
    public class CamerasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RtspStreamService _streamService;

        public CamerasController(ApplicationDbContext context, RtspStreamService streamService)
        {
            _context = context;
            _streamService = streamService;
        }

        // Список камер
        public async Task<IActionResult> Index()
        {
            var cameras = await _context.Cameras.Include(c => c.MonitoringPost).ToListAsync();
            return View(cameras);
        }

        // Просмотр конкретной камеры и управление
        public async Task<IActionResult> Details(int id)
        {
            var camera = await _context.Cameras
                .Include(c => c.MonitoringPost)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (camera == null) return NotFound();
            
            return View(camera);
        }

        // Создание/подключение новой камеры
        public async Task<IActionResult> Create()
        {
            ViewBag.MonitoringPostId = new SelectList(await _context.MonitoringPosts.ToListAsync(), "Id", "Name");
            return View(new Camera());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Camera camera)
        {
            if (ModelState.IsValid)
            {
                _context.Add(camera);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MonitoringPostId = new SelectList(await _context.MonitoringPosts.ToListAsync(), "Id", "Name", camera.MonitoringPostId);
            return View(camera);
        }

        // Редактирование настроек камеры
        public async Task<IActionResult> Edit(int id)
        {
            var camera = await _context.Cameras.FindAsync(id);
            if (camera == null) return NotFound();
            ViewBag.MonitoringPostId = new SelectList(await _context.MonitoringPosts.ToListAsync(), "Id", "Name", camera.MonitoringPostId);
            return View(camera);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Camera camera)
        {
            if (id != camera.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(camera);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CameraExists(camera.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MonitoringPostId = new SelectList(await _context.MonitoringPosts.ToListAsync(), "Id", "Name", camera.MonitoringPostId);
            return View(camera);
        }

        // Удаление камеры
        public async Task<IActionResult> Delete(int id)
        {
            var camera = await _context.Cameras.FindAsync(id);
            if (camera == null) return NotFound();
            return View(camera);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var camera = await _context.Cameras.FindAsync(id);
            if (camera != null)
            {
                _context.Cameras.Remove(camera);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CameraExists(int id)
        {
            return _context.Cameras.Any(e => e.Id == id);
        }

        // API для управления потоком
        [HttpPost]
        public IActionResult StartStream(int id)
        {
            var camera = _context.Cameras.Find(id);
            if (camera == null) return Json(new { success = false, error = "Камера не найдена" });

            _streamService.Dispose(); // Останавливаем предыдущий поток
            
            string fullUrl = camera.RtspUrl;
            if (!string.IsNullOrEmpty(camera.Username) && !camera.RtspUrl.Contains("@"))
            {
                fullUrl = camera.RtspUrl.Replace("://", $"://{camera.Username}:{camera.Password}@");
            }

            _streamService.Start(fullUrl);
            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult GetFrame()
        {
            var frame = _streamService.GetFrame();
            if (frame == null) return NotFound();
            return File(frame, "image/jpeg");
        }
    }
}
