using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalView
{
    public class PageInfo
    {
        public int Page  { get; set; }
        public int ItemsPerPage { get; set; }
        public string SortBy { get; set; }
        public bool Reverse { get; set; }
        public string  Search  { get; set; }
        public int TotalItems { get; set; }
        public int Activo { get; set; }
        public int CentroCostoId { get; set; }
        public List<string> Estados { get; set; }
        public string App { get; set; }
    }
}