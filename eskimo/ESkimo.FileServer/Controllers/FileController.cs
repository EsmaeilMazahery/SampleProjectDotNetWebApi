using ESkimo.FileServer.ViewModels;
using ESkimo.Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ESkimo.FileServer.Controllers
{
    [Route("api/File")]
    public class FileController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public FileController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("Upload/{PreAddress?}")]
        public IActionResult Post_File_Upload(string PreAddress = null)
        {
            try
            {
                HttpRequest httpRequest = HttpContext.Request;

                // check validations
                if (httpRequest.Form.Files == null)
                    return Ok(new Post_File_Upload_Response { result = false, message = "هیچ فایلی انتخاب نشده است." });
                if (httpRequest.Form.Files.Count != 1)
                    return Ok(new Post_File_Upload_Response { result = false, message = "تعداد فایل انتخاب شده غیرمجاز است." });

                IFormFile file = httpRequest.Form.Files[0];

                string[] extensions = new string[] { ".rar", ".zip", ".xls", ".xlsx", ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".bmp", ".png", ".gif", ".txt", ".html", ".htm", ".css", ".7zip", ".mp3", ".ogg", ".wav", ".wma", ".7z", ".pps", ".ppt", ".pptx", ".xlr", ".ods", ".odp", ".3gp", ".avi", ".flv", ".h264", ".mkv", ".mov", ".mp4", ".mpg", ".mpeg", ".swf", ".wmv", ".odt", ".wks", ".wps" };
                string[] imageExtensions = new string[] { ".jpg", ".jpeg", ".bmp", ".png", ".gif" };

                if (!extensions.Any(a => a == Path.GetExtension(file.FileName).ToLower()))
                    return Ok(new Post_File_Upload_Response { result = false, message = "پسوند فایل انتخاب شده مجاز نیست." });

                //string[] types = new string[] { "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "application/msexcel", "application/x-msexcel", "application/x-ms-excel", "application/x-excel", "application/x-dos_ms_excel", "application/xls", "application/x-xls" };
                //if (!types.Contains(file.ContentType.ToLower()))
                //    return Request.CreateResponse(HttpStatusCode.OK, new Post_File_Upload_Response { result = false, message = "لطفا فایل معتبری انتخاب کنید.", filename = null });

                //if(file.Length > ‭20971520‬)
                //    return Ok(new Post_File_Upload_Response { result = false, message = "حجم فایل شما بیشتر از حد مجاز است. حداکثر حجم مجاز 20 مگابایت است." });

                // generate filename
                string root = _hostingEnvironment.WebRootPath + "/Files/";
                string path = root;
                string pathThumb = "";
                string pathMedium = "";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (!string.IsNullOrWhiteSpace(PreAddress))
                {
                    if (imageExtensions.Any(a => a == Path.GetExtension(file.FileName).ToLower()))
                    {
                        pathThumb = Path.Combine(path, "Thumb", PreAddress);

                        if (!Directory.Exists(pathThumb))
                            Directory.CreateDirectory(pathThumb);

                        pathMedium = Path.Combine(path, "Medium", PreAddress);

                        if (!Directory.Exists(pathMedium))
                            Directory.CreateDirectory(pathMedium);
                    }

                    path = Path.Combine(path, PreAddress);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                // upload
                path = Path.Combine(path, filename);
                if (!string.IsNullOrEmpty(pathThumb))
                {
                    pathThumb = Path.Combine(pathThumb, filename);
                    pathMedium = Path.Combine(pathMedium, filename);
                }

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Close();
                    if (!string.IsNullOrEmpty(pathThumb))
                    {
                        Image newImageThumb = GetReducedImage(250, 250, file.OpenReadStream());
                        newImageThumb.Save(pathThumb);

                        Image newImageMedium = GetReducedImage(800, 800, file.OpenReadStream());
                        newImageMedium.Save(pathMedium);
                    }
                }
                FileInfo f = new FileInfo(path);
                if (f.Length == 0)
                    return BadRequest(new Post_File_Upload_Response { result = false, message = "خطایی در آپلود فایل به وجود آمد" });

                // done
                return Ok(new Post_File_Upload_Response
                {
                    result = true,
                    message = "فایل شما باموفقیت آپلود شد.",
                    filename = path.TrimStart(root).Replace("\\", "/")
                });
            }
            catch (Exception ex)
            {
                // Log.LogToFile(ex, MethodBase.GetCurrentMethod());
                return Ok(new Post_File_Upload_Response { result = false, message = ex.Message + "خطای نامشخصی رخ داده است." });
            }
        }

        public Image GetReducedImage(int width, int height, Stream resourceImage)
        {
            try
            {
                Image image = Image.FromStream(resourceImage);
                Image thumb = image.GetThumbnailImage(image.Width > width ? width : image.Width, image.Height > height ? height : image.Height, () => false, IntPtr.Zero);

                return thumb;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        [HttpPost]
        [Route("Exists")]
        public IActionResult Get_File_Exists(Post_File_Exists_Request model)
        {
            try
            {

                if (model == null || !ModelState.IsValid)
                    return Ok(new Post_File_Exists_Response { result = false, message = "درخواست ارسالی نامعتبر است.", exists = false });

                string root = _hostingEnvironment.WebRootPath + "/Files/";
                string path = Path.Combine(root, model.Filename);
                bool result = System.IO.File.Exists(path);

                if (result)
                    return Ok(new Post_File_Exists_Response { result = true, message = "فایل مورد نظر یافت شد.", exists = result });
                else
                    return Ok(new Post_File_Exists_Response { result = true, message = "فایل مورد نظر وجود ندارد.", exists = result });
            }
            catch
            {
                // Log.LogToFile(ex, MethodBase.GetCurrentMethod());
                return Ok(new Post_File_Exists_Response { result = false, message = "خطای نامشخصی رخ داده است.", exists = false });
            }
        }

    }
}
