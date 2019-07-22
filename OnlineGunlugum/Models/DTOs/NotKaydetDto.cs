using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineGunlugum.Models/*.DTOs*/
//OnlineGunlugum.Models.DTOs kisminda .dtos kismini sildim,dto klasörü ekleyince uzamıitı mıodel adim
{
    public class NotKaydetDto
    {
        //PROPERTİLERİ BİREBİR AYNİ YAPMAK ZORUNDA DEĞİLİZ

        public int ID { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
    }
}