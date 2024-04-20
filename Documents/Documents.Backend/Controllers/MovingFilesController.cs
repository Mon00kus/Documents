using Documents.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Documents.Backend.Controllers
{
    public class MovingFilesController : Controller
    {
        public async Task MoveFileToNetworkPathAsync(string sourcePath, string targetPath)
        {
            await ManageFiles.MoveFileToNetworkPathAsync(sourcePath, targetPath);
        }
    }
}
