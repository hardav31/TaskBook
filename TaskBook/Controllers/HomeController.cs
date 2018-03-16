using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Configuration;
using TaskBook.Models;

namespace TaskBook.Controllers
{
    public class HomeController : Controller
    {

        public static List<MyModel> products = new List<MyModel>()
        {
            new MyModel()
            {
                FileName="Shildt.pdf",
                Hexinak="Herbert Shildt:The Complete Reference ",
                NkarName="1.jpg",
                Nkaragrutyun="В этой книге описывается версия 4.0 языка С#."

            },
            new MyModel()
            {
                FileName="ASPNETMVC4inAction.pdf",
                Hexinak="Narayan: ASP.NET1",
                NkarName="2.jpg",
                Nkaragrutyun="Learn all the new ASP.NET ",
            }

        };

        public ActionResult Index()
        {
            
            ViewBag.Message = "";
            return View("Index", products);
        }

        [HttpPost]
        public ActionResult About(HttpPostedFileBase file, HttpPostedFileBase nkar, string vernagir, string nkaragir)
        {
            ViewBag.Message = "";
            string filename = Guid.NewGuid().ToString();
            string extensionfile = Path.GetExtension(file.FileName);
            filename += extensionfile;

            string nkarname = Guid.NewGuid().ToString();
            string extensionnkar = Path.GetExtension(nkar.FileName);
            nkarname += extensionnkar;

            extensionfile = extensionfile.ToLower();
            if (extensionfile == ".pdf" && (extensionnkar == ".jpg" || extensionnkar==".png"))
            {
                file.SaveAs(Server.MapPath("/File/" + filename));
                nkar.SaveAs(Server.MapPath("/Nkar/" + nkarname));

                products.Add(new MyModel()
                {
                    FileName = filename,
                    NkarName = nkarname,
                    Hexinak = vernagir,
                    Nkaragrutyun = nkaragir,
                });
                
                ViewBag.Message1 = "";
                ViewBag.Message2 = "";

            }
            else
            {
                if (extensionfile != ".pdf")
                {
                    ViewBag.Message1 = "The formate File isn't  correct(.pdf)";
                }
                if (extensionnkar != ".jpg" || extensionnkar !=".png")
                {
                    ViewBag.Message1 = "The formate image isn't  correct(.jpg,.png,)";
                }
            }

            return View("Index", products);
        }


        public ActionResult Delete(string filename)
        {
            MyModel ob = null;
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].FileName == filename)
                {
                    ob = products[i];

                    var path1 = Path.Combine(Server.MapPath("~/File"), ob.FileName);
                    FileInfo fileInf1 = new FileInfo(path1);
                    if (fileInf1.Exists)
                    {
                        fileInf1.Delete();
                    }

                    
                    string path2 = Path.Combine(Server.MapPath("~/Nkar"), ob.NkarName);
                    FileInfo fileInf2 = new FileInfo(path2);
                    if (fileInf2.Exists)
                    {
                        fileInf2.Delete();
                    }
                    products.RemoveAt(i);
                    break;
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Change(string filename)
        {  
            TempData["model"] = filename;
            return View("Change");
        }

        [HttpPost]
        public ActionResult Change(HttpPostedFileBase file, HttpPostedFileBase nkar, string vernagir, string nkaragir)
        {
            string model = TempData["model"] as string;
            MyModel ob = null;
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].FileName == model)
                {
                    ob = products[i];
                    string filename = ob.FileName;
                    string extensionfile = Path.GetExtension(file.FileName);
                  

                    string nkarname = ob.NkarName;
                    string extensionnkar = Path.GetExtension(nkar.FileName);

                    var path1 = Path.Combine(Server.MapPath("~/File"), ob.FileName);
                    FileInfo fileInf1 = new FileInfo(path1);
                    if (fileInf1.Exists)
                    {
                        fileInf1.Delete();
                    }

                    string path2 = Path.Combine(Server.MapPath("~/Nkar"), ob.NkarName);
                    FileInfo fileInf2 = new FileInfo(path2);
                    if (fileInf2.Exists)
                    {
                        fileInf2.Delete();
                        
                    }

                    if (extensionfile == ".pdf" && (extensionnkar == ".jpg" || extensionnkar == ".png"))
                    {
                        file.SaveAs(Server.MapPath("/File/" + filename));
                        nkar.SaveAs(Server.MapPath("/Nkar/" + nkarname));

                        ob.FileName = filename;
                        ob.NkarName = nkarname;
                        ob.Hexinak = vernagir;
                        ob.Nkaragrutyun = nkaragir;


                        ViewBag.Message1 = "";
                        ViewBag.Message2 = "";
                    }
                    else
                    {
                        if (extensionfile != ".pdf")
                        {
                            ViewBag.Message1 = "The formate File no correspond(.pdf)";
                        }
                        if (extensionnkar != ".jpg" || extensionnkar != ".png")
                        {
                            ViewBag.Message1 = "The formate image no correspond(.jpg,.png,)";
                        }
                        TempData["model"] = model;
                        return View("Change");
                    }
                    products[i] = ob;
                    break;
                }

            }

            return View("Index",products);
        }

        [HttpPost]
        public ActionResult MyAction(string submitButton,string filename)
        {
            switch (submitButton)
            {
                case "Delete":
                    
                    return (Delete(filename));
                case "Change":
                    
                    return (Change(filename));
                default:
                    
                    return (View());
            }
        }

    }
}
