﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TheLastBuildWeek.Models;

namespace TheLastBuildWeek.Controllers
{
    public class HomeController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        private class Animale
        {
            public int Id { get; set; }
            public DateTime DataRegistrazione { get; set; }
            public string Nome { get; set; }
            public string Tipologia { get; set; }
            public string CodiceMicrochip { get; set; }
            public string Foto { get; set; }
            public string NomeProprietario { get; set; }
            public string CognomeProprietario { get; set; }
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Login([Bind(Include = "Username, Password, Role")] T_User users)
        {

            var user = db.T_User.FirstOrDefault(u => u.Username == users.Username && u.Password == users.Password);

            if (user != null)
            {

                FormsAuthentication.SetAuthCookie(users.Username, false);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //[Authorize(Roles="Veterinario")]
        public ActionResult LoggedIn()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {

            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Home");
        }

        public ActionResult InsertChipCode()
        {
            return View();
        }

        public ActionResult GetByChipCode(string code)
        {
            List<Animale> animaleList = new List<Animale>();

            foreach (T_Animali animale in db.T_Animali.ToList())
                animaleList.Add(new Animale
                {
                    Id = animale.IDAnimale,
                    DataRegistrazione = animale.DataRegistrazione,
                    Nome = animale.NomeAnimale,
                    Tipologia = animale.Tipologia,
                    CodiceMicrochip = animale.CodiceMicrochip,
                    Foto = animale.FotoAnimale,
                    NomeProprietario = animale.NomeProprietario,
                    CognomeProprietario = animale.CognomeProprietario
                }) ;
            return Json(animaleList.Where(a => a.CodiceMicrochip == code), JsonRequestBehavior.AllowGet);
        }
        /////////////////////////////////////////////// ACTION PER VIEW RICOVERI ///////////////////////////
        [HttpGet]
        public ActionResult Ricoveri (T_Ricovero ricovero)
        {
            var animaleNome = db.T_Animali.FirstOrDefault();
            var foreignKeyAnimale = db.T_Ricovero.FirstOrDefault();

            if(foreignKeyAnimale == null)
            {
                return View(animaleNome);
            }


            return View(db.T_Ricovero.ToList());
        }

        [HttpGet]
        public ActionResult Visite()
        {
            return View();
        }


        [HttpGet]
        public ActionResult DetailVisita (T_Visita visita)
        {
            var animaleRicoverato = db.T_Animali.FirstOrDefault();
            var idAnimaleRicoverato = db.T_Ricovero.FirstOrDefault();

            if (animaleRicoverato.IDAnimale == idAnimaleRicoverato.FKIDAnimale)
            {
                return View(db.T_Visita.ToList());
            }

            return View();
        }
    }
}
