using Microsoft.AspNetCore.Mvc;
using MoodTunes.Models;
using MoodTunes.Services;

namespace MoodTunes.Controllers;

public class MoodController : Controller
{
    private readonly MoodService _service;

    public MoodController(MoodService service)
    {
        _service = service;
    }

    public IActionResult Index(MoodCategory? filterMood = null, string? searchNotes = null)
    {
        var entries = _service.GetAllEntries();

        if (filterMood.HasValue)
            entries = _service.FilterByMood(filterMood.Value);

        if (!string.IsNullOrEmpty(searchNotes))
            entries = _service.SearchNotes(searchNotes);

        return View(entries);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(MoodEntry entry)
    {
        _service.AddEntry(entry);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Stats()
    {
        var stats = _service.GetStats();
        return View(stats);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        _service.DeleteEntry(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult FilterByDate(DateTime from, DateTime to)
    {
        var entries = _service.FilterByDateRange(from, to);
        return View("Index", entries);
    }

    public IActionResult ExportJson()
    {
        var entries = _service.GetAllEntries();
        return Json(entries);
    }
}
