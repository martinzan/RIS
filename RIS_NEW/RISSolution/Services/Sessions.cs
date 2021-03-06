﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web.Services.Protocols;
using BiznisObjects;
using DatabaseEntities;
using IServices;

namespace Services
{
    /// <summary>
    ///  Zoznám prihlasení
    /// </summary>
    public class Sessions : IServiceSession
    {
        
        private readonly int ADMIN = 1;
        private readonly int STOL = 2;

        private risTabulky risContext;

        Dictionary<String,BRisUser> prihlasenia =new Dictionary<String,BRisUser>() ;

        /// <summary>
        /// Vytvorí nový zoznam prihlasených uživateľov
        /// </summary>
        /// <param name="risContext">kontext databázy</param>
        public Sessions(risTabulky risContext)
        {
            this.risContext = risContext;
        }

        /// <summary>
        ///  Má dané prihlasenie admin prava
        /// </summary>
        /// <param name="session">string prihlasenia</param>
        /// <returns><c>TRUE</c> ,áno ak dané prihlasenie má admin práva
        /// <c>FALSE</c> , nie ak dané prihlasenie nemá admin práva
        /// </returns>
        public Boolean JeAdmin(String session)
        {
            if (prihlasenia[session] != null)
            {

                /*
                    return (prihlasenia[session].typ_uctu.id == ADMIN);
                    
                */ // ercisk 
                return true;                  
            }
            throw new SoapException("You are not logged in", SoapException.ClientFaultCode);          
        }

        /// <summary>
        ///  Má dané prihlasenie práva stola
        /// </summary>
        /// <param name="session">string prihlasenia</param>
        /// <returns><c>TRUE</c> ,áno ak dané prihlasenie má práva stola
        /// <c>FALSE</c> , nie ak dané prihlasenie nemá práva stola
        /// </returns>
        public Boolean jeStol(String session)
        {
            if (prihlasenia[session] != null)
            {
                /*
                return (prihlasenia[session].typ_uctu.id == STOL);
                */ // ercisk
                return true;
            }
            throw new SoapException("You are not logged in", SoapException.ClientFaultCode);
        }

        /// <summary>
        ///  Prihlas učet
        /// </summary>
        /// <param name="email">prihlasovací login</param>
        /// <param name="heslo">prihlasovacie heslo</param>
        /// <returns>
        ///    session prihlasenia
        /// </returns>
        public String logIn(String email, String heslo)
        {
            String hash = GetCrypt(heslo);
            BRisUser ucet=Zoznamy.dajUcet(email,risContext);
            if (ucet != null)
            {
                if (ucet.Password == hash)
                {
                    string NewID = GenerateSession();
                    prihlasenia.Add(NewID, ucet);
                    return NewID;
                }
                else
                {
                    throw new SoapException("Wrong password or login", SoapException.ClientFaultCode);
                }
            }
            else
            {
                throw new SoapException("Wrong password or login", SoapException.ClientFaultCode);
            }
        }


        private static string GenerateSession()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            GuidString = GuidString + DateTime.Now.Millisecond;
            return GuidString;
        }

        private static string GetCrypt(string text)
        {
            string hash = "";
            SHA512 alg = SHA512.Create();
            byte[] result = alg.ComputeHash(Encoding.UTF8.GetBytes(text));
            hash = Encoding.UTF8.GetString(result);
            return hash;
        }

        public bool logOut(string email)
        {
            BRisUser ucet = Zoznamy.dajUcet(email, risContext);
            string kluc = "";

            if (ucet != null)
            {
                foreach (var p in prihlasenia)
                {
                    if (p.Value.Email == email)
                    {
                        kluc = p.Key;
                    }
                }
            }
            return prihlasenia.Remove(kluc);
        }

        public BRisUser registruj(string meno, string priezvisko, string email, string heslo)
        {
            BRisUser ret = new BRisUser();
            ret.Name = meno;
            ret.RisUserId = int.Parse(DateTime.Now.Minute + "" + DateTime.Now.Millisecond);
            ret.Email = email;
            ret.Password = GetCrypt(heslo);
            ret.Surname = priezvisko;
            ret.FillEntity();

            risContext.ris_user.Add(ret.entityRisUser);
            risContext.SaveChanges();
            return ret;
        }
    }
}
