using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Product
    {
        private string MaHang;
        private string TenHang;
        private string DVT;
        private int Gia;
        private string Image;

        public Product()
        {
        }

        public Product(string maHang, string tenHang, string dVT, int gia, string image)
        {
            MaHang1 = maHang;
            TenHang1 = tenHang;
            DVT1 = dVT;
            Gia1 = gia;
            Image1 = image;
        }

        public string MaHang1 { get => MaHang; set => MaHang = value; }
        public string TenHang1 { get => TenHang; set => TenHang = value; }
        public string DVT1 { get => DVT; set => DVT = value; }
        public int Gia1 { get => Gia; set => Gia = value; }
        public string Image1 { get => Image; set => Image = value; }
    }
}