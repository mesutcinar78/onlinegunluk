using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OnlineGunlugum.Models;
using Microsoft.AspNet.Identity;
//ben ekledim model i

namespace OnlineGunlugum.Controllers
{
    [Authorize]//token siz işlem yaptirmaz

    //login olmaya zorluyor authorize,yada yetki sahibi olanlar görebilir,apinin tepesine yaziyorum burdada token siz işlem yaptirmaz
    public class MainController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        //servis metodu yaziyorum
        public IEnumerable<NotBaslikDTO> GetBasliklar()
        {
            //adam demekki online 
            string loggedInUserID = User.Identity.GetUserId();
            //GetUserId(); control nokta ekledim
            //notlarin içine kullanicinin id sini attim şimdi de set edicem

            return db.Notlar.Where(x => x.ApplicationUserID == loggedInUserID).Select(x => new NotBaslikDTO() { ID = x.ID, Baslik = x.Baslik }).ToList();
        }

        public IHttpActionResult GetNot(int id)//burada ne yaptik
                                               //http response message ile ayni
        {
            string loggedInUserID = User.Identity.GetUserId();
            Not not = db.Notlar.FirstOrDefault(x => x.ApplicationUserID == loggedInUserID && x.ID == id);

            if (not == null)
                return NotFound();

            return Ok(not);
            //notlar listelendi
        }

        [HttpPost]
        public HttpResponseMessage Save(NotKaydetDto not)
        {
            string loggedInUserID = User.Identity.GetUserId();
            NotIslemBilgiDto mesaj = new NotIslemBilgiDto();

            //gelen not isimli parameter üzerinde koşul yaziyorum

            if (not.ID == 0)//yeni kayıt
                            //gelen id daha oluşturulamadiysa yeni kayit aksi halde else te güncelleme yapicam
            {
                Not n = new Not();
                n.ApplicationUserID = loggedInUserID;
                n.EklenmeTarihi = DateTime.Now;
                n.Baslik = not.Baslik;
                n.Icerik = not.Icerik;

                db.Notlar.Add(n);
                db.SaveChanges();

                mesaj.ID = n.ID;
                mesaj.Mesaj = "Eklendi(" + n.EklenmeTarihi.Value.ToLongDateString() + ")";
            }
            else//güncelleme
            {
                Not n = db.Notlar.FirstOrDefault(x => x.ApplicationUserID == loggedInUserID && x.ID == not.ID);

                if (n == null)//bulamadi diyelim id yi o yüzden if içerisinde kontrol ediyorum
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Bir Hata Oluştu...");

                n.Baslik = not.Baslik;
                n.Icerik = not.Icerik;
                n.SonGüncellemeTarihi = DateTime.Now;

                db.SaveChanges();

                mesaj.ID = n.ID;
                mesaj.Mesaj = "Güncellendi (" + n.SonGüncellemeTarihi.Value.ToLongDateString() + ")";
            }

            return Request.CreateResponse(HttpStatusCode.OK, mesaj);

            //artık güncelleme mi kayıt mi yapti mesaj içerisinde duruyor

        }

        [HttpPost]
        public HttpResponseMessage Delete(int id)
        {
            string loggedInUserID = User.Identity.GetUserId();
            Not not = db.Notlar.FirstOrDefault(x => x.ApplicationUserID == loggedInUserID && x.ID == id);
            string baslik = not.Baslik;

            db.Notlar.Remove(not);
            db.SaveChanges();

            NotIslemBilgiDto mesaj = new NotIslemBilgiDto()
            {
                ID = id,
                Mesaj = baslik + "başıklı not silindi..."
            };

            return Request.CreateResponse(HttpStatusCode.OK, mesaj);
            //accountbindingsmodele gittik

        }
    }
}
