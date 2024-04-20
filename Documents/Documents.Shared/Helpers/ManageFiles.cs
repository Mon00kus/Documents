using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents.Shared.Helpers
{
    public static class ManageFiles
    {
        public static async Task MoveFileToNetworkPathAsync(string sourcePath, string targetPath)
        {
            if (File.Exists(sourcePath))
            {
                await Task.Run(() =>
                {
                    try
                    {
                        File.Move(sourcePath, targetPath);
                    }
                    catch (IOException ex)
                    {                        
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                });
            }
        }
    }
}