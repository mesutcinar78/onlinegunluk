using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineGunlugum.Models/*.DTOs*/
{
    public class NotIslemBilgiDto
    {
        public int ID { get; set; }
        //notlar tablosundan sadece ic kullanacagim ve mesaj iletecegim ekstradan
        public string Mesaj { get; set; }
    }
}