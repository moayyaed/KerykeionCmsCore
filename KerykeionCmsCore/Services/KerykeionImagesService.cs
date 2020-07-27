using ImageManagement.Services;
using KerykeionCmsCore.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsCore.Services
{
    public class KerykeionImagesService : KerykeionCmsService<Image>
    {
        private readonly ImagesService _imagesService;

        public KerykeionImagesService(EntitiesService service,
            ImagesService imagesService) : base(service)
        {
            _imagesService = imagesService;
        }

        public async Task<KerykeionDbResult> CreateAsync(Image image, IFormFile file, IEnumerable<KeyValuePair<string, StringValues>> formForeignKeys)
        {
            if (image == null || file == null)
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = "The supplied arguments are not valid." });
            }

            if (string.IsNullOrEmpty(image.Name))
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = "Please provide a name for the Image." });
            }

            if (formForeignKeys != null)
            {
                var result = await EntitiesService.AssignFormForeignKeysAsync(image, formForeignKeys);
                if (!result.Successfull)
                {
                    return result;
                }
            }

            var uploadResult = await _imagesService.SaveImage(file);
            if (uploadResult.Success)
            {
                image.Url = $"/images/{uploadResult.ImgUrl}";
                return await CreateAsync(image);
            }

            return KerykeionDbResult.Fail(new KerykeionDbError { Message = "The upload of the file failed." });
        }

        public async Task<KerykeionDbResult> UpdateAsync(Image image, IFormFile file, IEnumerable<KeyValuePair<string, StringValues>> formForeignKeys)
        {
            if (image == null || file == null)
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = "The supplied arguments are not valid." });
            }

            if (formForeignKeys != null)
            {
                var result = await EntitiesService.AssignFormForeignKeysAsync(image, formForeignKeys);
                if (!result.Successfull)
                {
                    return result;
                }
            }

            var replaceImgResult = await _imagesService.ReplaceImage(image?.Url.Split("/").Last(), file);
            if (replaceImgResult.Success)
            {
                image.Url = $"/images/{replaceImgResult.ImgUrl}";
                return await UpdateAsync(image);
            }

            return KerykeionDbResult.Fail(new KerykeionDbError { Message = "Could not upload the new image." });
        }

        public async Task<KerykeionDbResult> UpdateAsync(Image image, IEnumerable<KeyValuePair<string, StringValues>> formForeignKeys)
        {
            if (formForeignKeys != null)
            {
                var result = await EntitiesService.AssignFormForeignKeysAsync(image, formForeignKeys);
                if (!result.Successfull)
                {
                    return result;
                }
            }

            return await UpdateAsync(image);
        }



        public async Task<KerykeionDbResult> RemoveImageAndDeleteAsync(Image image)
        {
            if (image == null)
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = "The supplied arguments are not valid." });
            }

            var removeImgResult = await _imagesService.DeleteImage(image.Url.Split("/").Last());
            if (removeImgResult.Success)
            {
                return await DeleteAsync(image);
            }

            return KerykeionDbResult.Fail(new KerykeionDbError { Message = "Could not delete the image." });
        }
    }
}
