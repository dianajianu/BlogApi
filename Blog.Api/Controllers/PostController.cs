using Blog.Api.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly IPostService _postService;
        private readonly IAuthorizationService _authorizationService;

        public PostController(ILogger<PostController> logger, IPostService postService, IAuthorizationService authorizationService)
        {
            _logger = logger;
            _postService = postService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult> GetPosts()
        {
            try
            {
                if (!await _authorizationService.IsAuthorized(Request.Headers, Utils.Enums.User.user))
                    return StatusCode(StatusCodes.Status401Unauthorized,
                        "Unauthorized request");

                return Ok(await _postService.SearchPostAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error retrieving data");  
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult> Search([FromQuery] string? title, [FromQuery] int? category, [FromQuery] IEnumerable<int>? tags)
        {
            try
            {
                if (!await _authorizationService.IsAuthorized(Request.Headers, Utils.Enums.User.user))
                    return StatusCode(StatusCodes.Status401Unauthorized,
                        "Unauthorized request");

                return Ok(await _postService.SearchPostAsync(title, category, tags));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetPost(int id)
        {
            try
            {
                if (!await _authorizationService.IsAuthorized(Request.Headers, Utils.Enums.User.user))
                    return StatusCode(StatusCodes.Status401Unauthorized,
                        "Unauthorized request");

                var result = await _postService.GetPostAsync(id);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost(Dto.Input.Post post)
        {
            try
            {
                if (!await _authorizationService.IsAuthorized(Request.Headers, Utils.Enums.User.user))
                {
                    return StatusCode(StatusCodes.Status401Unauthorized,
                        "Unauthorized request");
                }

                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)));
                }

                var result = await _postService.AddPostAsync(post);

                return CreatedAtAction(nameof(GetPost),
                    new { Id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating data");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdatePost(int id, Dto.Input.Post post)
        {
            try
            {
                if (!await _authorizationService.IsAuthorized(Request.Headers, Utils.Enums.User.user))
                    return StatusCode(StatusCodes.Status401Unauthorized,
                        "Unauthorized request");

                if (id != post.Id)
                {
                    return BadRequest("Post ID mismatch");
                }

                var result = await _postService.UpdatePostAsync(post);

                if (result == null)
                {
                    return BadRequest($"Post with ID: {id} not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        [HttpPatch]
        public async Task<ActionResult> PatchPost(int id, JsonPatchDocument<Post> jsonPatch)
        {
            try
            {
                if (!await _authorizationService.IsAuthorized(Request.Headers, Utils.Enums.User.user))
                    return StatusCode(StatusCodes.Status401Unauthorized,
                        "Unauthorized request");

                if (id < 1)
                {
                    return BadRequest("Invalid post id");
                }

                var result = await _postService.PatchPostAsync(id, jsonPatch);

                if (result == null)
                {
                    return BadRequest($"Post with id: {id} not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error patching data");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            try
            {
                if (!await _authorizationService.IsAuthorized(Request.Headers, Utils.Enums.User.admin))
                    return StatusCode(StatusCodes.Status401Unauthorized,
                        "Unauthorized request");

                await _postService.DeletePostAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

        [HttpGet("categories")]
        public async Task<ActionResult> GetCategories()
        {
            try
            {
                if (!await _authorizationService.IsAuthorized(Request.Headers, Utils.Enums.User.user))
                    return StatusCode(StatusCodes.Status401Unauthorized,
                        "Unauthorized request");

                var result = await _postService.GetCategoriesAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }

        [HttpGet("tags")]
        public async Task<ActionResult> GetTags()
        {
            try
            {
                if (!await _authorizationService.IsAuthorized(Request.Headers, Utils.Enums.User.user))
                    return StatusCode(StatusCodes.Status401Unauthorized,
                        "Unauthorized request");

                var result = await _postService.GetTagsAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }
    }
}