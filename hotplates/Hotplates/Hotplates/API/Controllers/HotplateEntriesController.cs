using Hotplates.Core.DTOs;
using Hotplates.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotplates.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotplateEntriesController : ControllerBase
{
    private readonly IHotplateEntryService _service;

    public HotplateEntriesController(IHotplateEntryService service)
    {
        _service = service;
    }

    // GET: api/hotplateentries
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<HotplateEntryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var entries = await _service.GetAllAsync();
        return Ok(entries);
    }

    // GET: api/hotplateentries/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(HotplateEntryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var entry = await _service.GetByIdAsync(id);
        if (entry == null)
            return NotFound();

        return Ok(entry);
    }

    // POST: api/hotplateentries
    [HttpPost]
    [ProducesResponseType(typeof(HotplateEntryDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] HotplateCreateRequest request)
    {
        var created = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/hotplateentries/{id}
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(HotplateEntryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] HotplateUpdateRequest request)
    {
        var updated = await _service.UpdateAsync(id, request);
        if (updated == null)
            return NotFound();

        return Ok(updated);
    }

    // TODO: Instead of deleting the plate, we should probably just mark it as "resolved" or "inactive" and keep it in the database for historical purposes.
    //          THis will need a dataabse migration update as well. For now, we'll just implement the delete functionality as is, but we should revisit this
    //          in the future.
    // DELETE: api/hotplateentries/{id}
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
