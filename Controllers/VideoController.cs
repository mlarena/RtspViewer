using Microsoft.AspNetCore.Mvc;
using RtspViewer.Services;

namespace RtspViewer.Controllers;

public class VideoController : Controller
{
    private static RtspStreamService? _streamService;

    public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Start(string rtspUrl, string username, string password)
    {
        try
        {
            _streamService?.Dispose();
            _streamService = new RtspStreamService();

            // Собираем URL с credentials
            string fullUrl;
            if (!string.IsNullOrEmpty(username))
            {
                // Экранируем спецсимволы в пароле!
                var safeUser = Uri.EscapeDataString(username);
                var safePass = Uri.EscapeDataString(password);
                fullUrl = rtspUrl.Replace("://", $"://{safeUser}:{safePass}@");
            }
            else
            {
                fullUrl = rtspUrl;
            }

            Console.WriteLine($"[Controller] URL: {rtspUrl}");
            _streamService.Start(fullUrl);

            // Ждём первый кадр (максимум 10 секунд)
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(500);
                if (_streamService.GetFrame() != null)
                    return Json(new { success = true });
                
                if (!string.IsNullOrEmpty(_streamService.LastError))
                    return Json(new { success = false, error = _streamService.LastError });
            }

            return Json(new { success = false, error = "Таймаут ожидания видео" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }

    [HttpPost]
    public IActionResult Stop()
    {
        _streamService?.Dispose();
        _streamService = null;
        return Json(new { success = true });
    }

    [HttpGet]
    public IActionResult Frame()
    {
        var frame = _streamService?.GetFrame();
        if (frame == null) return NotFound();
        
        Response.Headers.Append("Cache-Control", "no-cache");
        return File(frame, "image/jpeg");
    }
}