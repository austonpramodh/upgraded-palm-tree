using System;
using Microsoft.AspNetCore.Mvc;
using MongoExample.Services;
using MongoExample.Models;
using System.ComponentModel;

namespace MongoExample.Controllers;

[Controller]
[Route("api/[controller]")]
public class PlaylistController : Controller
{

  private readonly MongoDBService _mongoDBService;

  public PlaylistController(MongoDBService mongoDBService)
  {
    _mongoDBService = mongoDBService;
  }

  [HttpGet]
  public async Task<List<Playlist>> Get()
  {
    return await _mongoDBService.GetAsync();
  }

  [HttpPost]
  public async Task<IActionResult> Post([FromBody] Playlist playlist)
  {
    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(playlist))
    {
      string name = descriptor.Name;
      object value = descriptor.GetValue(playlist);
      Console.WriteLine("{0}={1}", name, value);
    }

    await _mongoDBService.CreateAsync(playlist);
    return CreatedAtAction(nameof(Get), new { id = playlist.Id }, playlist);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> AddToPlaylist(string id, [FromBody] string movieId)
  {
    await _mongoDBService.AddToPlaylistAsync(id, movieId);
    return NoContent();
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(string id)
  {
    await _mongoDBService.DeleteAsync(id);
    return NoContent();
  }

}