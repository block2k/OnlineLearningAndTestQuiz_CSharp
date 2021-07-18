using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Product
    {
        private int id;
        private string MaHang;
        private string TenHang;
        private string DVT;
        private int Gia;
        private string Image;

        public Product()
        {
        }

        public Product(int id, string maHang, string tenHang, string dVT, int gia, string image)
        {
            this.Id = id;
            MaHang1 = maHang;
            TenHang1 = tenHang;
            DVT1 = dVT;
            Gia1 = gia;
            Image1 = image;
        }

        public int Id { get => id; set => id = value; }
        public string MaHang1 { get => MaHang; set => MaHang = value; }
        public string TenHang1 { get => TenHang; set => TenHang = value; }
        public string DVT1 { get => DVT; set => DVT = value; }
        public int Gia1 { get => Gia; set => Gia = value; }
        public string Image1 { get => Image; set => Image = value; }
    }
}