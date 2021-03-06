namespace KK.AspNetCore.Images.Sample.Web.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using KK.AspNetCore.Images.Sample.Web.Models.Services;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.FileProviders;

    public interface IImagesService
    {
        IDirectoryContents ImagesDirectoryContents { get; }
        List<Image> Images { get; set; }
    }

    public class ImagesService : IImagesService
    {

        public ImagesService(IHostingEnvironment env)
        {
            this.hostingEnvironment = env;
            this.fileProvider = env.ContentRootFileProvider;

            this.Images = this.ImagesDirectoryContents
                            .Where(
                                i => !i.IsDirectory && i.Name.EndsWith(".jpg")
                            )
                            .Select(
                                i => new Image()
                                {
                                    Name = Path.GetFileNameWithoutExtension(i.Name),
                                    PhysicalPath = i.PhysicalPath
                                }
                            )
                            .ToList();
        }

        private IHostingEnvironment hostingEnvironment;
        private readonly IFileProvider fileProvider;

        public IDirectoryContents ImagesDirectoryContents
        {
            get
            {
                return this.fileProvider.GetDirectoryContents("Images/");
            }
        }

        public List<Image> Images { get; set; }
    }
}