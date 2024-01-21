using TVMaze.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TVMaze.Controllers
{
    /// <summary>
    /// A controller for querring show data.
    /// </summary>
    [ApiController]
    [Route("Api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly IShowsService showsService;

        public ShowsController(IShowsService showsService)
        {
            this.showsService = showsService;
        }

        /// <summary>
        /// Gets a list of a show's cast
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A list of cast members</returns>
        [HttpGet]
        [Route("Cast")]
        public async Task<IActionResult> GetCast([FromQuery] int id)
        {
            var resul = this.showsService.GetCast(id);

            return Ok(resul);
        }


        /// <summary>
        /// Gets a paginated list of all the shows in the database
        /// </summary>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <returns>A paginated list of all shows and the total number of records</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int size, int page = 1)
        {
            var reuslt = await this.showsService.GetShowsAsync(size, page);

            return Ok(reuslt);
        }

        /// <summary>
        /// Gets a list of all the shows that contain a given string in their title
        /// </summary>
        /// <param name="title"></param>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <returns>A list of all the shows that match the search criteria and the total number of records</returns>
        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> GetAll([FromQuery] string title, int size, int page = 1)
        {
            var reuslt = await this.showsService.GetShowsByTitleAsync(title, size, page);

            return Ok(reuslt);
        }
    }
}
