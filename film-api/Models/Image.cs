using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmApi.Models
{
    public class Image
    {
        public long ImageID { get; set; }
        public long FilmID { get; set; }
        public string Data { get; set; }
    }
}
