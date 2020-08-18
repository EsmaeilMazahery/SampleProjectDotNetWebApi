using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SP.FileServer.ViewModels
{
    public class Post_File_Upload_Response
    {
        public bool result { get; set; }
        public string message { get; set; }
        public string filename { get; set; } = null;
    }

    public class Post_File_Exists_Request
    {
        [Required]
        public string Filename { get; set; }
    }

    public class Post_File_Exists_Response
    {
        public bool result { get; set; }
        public string message { get; set; }
        public bool exists { get; set; }
    }
}