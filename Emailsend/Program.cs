using System;
// Expensionje
using System.Net.Mail;
using System.IO;
using System.Linq;

namespace EmailsendLogger
{

    class Program
    {
        static void Main(string[] args)
        {

            SendTest("Teszt");
            /*
                Napi teams log küldés az elmentet hibákról.
                Email küldés - Kész
                Logger - Kész
                
                Ütemezett küldés - Nincs kész 
                Ötlet: Elmenteni az utolsó olvasott ID-t végig olvasni a txt-t ID-k alapján elküldeni 
                és elküldeni az adott sort/sorokat
                Utolsót megint mentjük.
               
            */

        }

        public static void SendTest(string content)
    {
    
        //Ha nem 0 vagy üres a content akkor lefut
        if (!String.IsNullOrEmpty(content))
        {
                //console teszt kiírása
            Console.WriteLine("Teszt email küldése folyamatban...");

            try
            {
                   
                    MailMessage mail = new MailMessage("Honnan", "Hova", "TÁRGY", content);
                    //Email html element felépítés
                    mail.IsBodyHtml = true;
                    //Smtp connect
                    SmtpClient smtpClient = new SmtpClient("smtp", 587);
                    smtpClient.Credentials = new System.Net.NetworkCredential("felhnev", "jelszo");
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mail);
                    Console.WriteLine("Email elküldve!");

            }
            catch (Exception ex)
            {
                 Logger(ex.Message);
                Console.WriteLine("Nem sikerült elküldeni a Teszt emailt");
            }

        }
    }
     
        
        public static void FajlKrealas()
        {
            string path = @"C:\Log\";
            string filepath = path+"log.txt";
            try
            {
                if (!Directory.Exists(path))
                {
                    DirectoryInfo mappa = Directory.CreateDirectory(path);
                    
                }
            }
            catch (IOException iohiba)
            {
                Console.WriteLine(iohiba.Message);
                throw;
            }

            try
            {
                if (!File.Exists(filepath))
                {
                   var myFile = File.Create(filepath);
                    //Itt lezárom a file-t ,hogy amikor újra hívja magát a Logger akkor ne fusson bele abba,hogy nyitva van.
                    myFile.Close();
                }


            }
            catch (IOException iohiba )
            {
                Console.WriteLine(iohiba.Message);
                throw;
           
            }
        }
        public static void Logger(string logcontent)
        {
            string path = @"C:\Log\log.txt";

                if(File.Exists(path))
                    {       
                            //Belül hívom meg ezt a kettőt így nem foglalja le előre magának a memóriát,ha hibára fut.
                            string id = GetLastID();
                            string time = DateTime.Now.ToString();
                        using (StreamWriter sw = File.AppendText(path))
                            {
                                sw.WriteLine(id +" - "+time + ": " + logcontent);
                                Console.WriteLine("sikeres írás");
                                sw.Close();
                            }
                     }
                else
                    {
                        //Meghívom újra a függvényeket.
                        FajlKrealas();
                        Logger(logcontent);
                    }
        }

        public static string GetLastID()
        {
            string path = @"C:\Log\log.txt";
            var info = new FileInfo(path);
            int LastID = 0;
            if (info.Length > 0)
            {
                string lastline = File.ReadLines(path).Last();
                string[] substing = lastline.Split(" - ");
                LastID = int.Parse(substing[0]);
                LastID++;
                return LastID.ToString();
            }
            else
            {
                return LastID.ToString();
            }

        }
    }
}

