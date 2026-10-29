using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;
using System.IO;

namespace ServisNET.Database
{
    public static class Helper
    {
        private static readonly string dbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServisNET.db");
        private static readonly string connectionString = $"Data Source={dbFile};Version=3;Foreign Keys=True;";

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

        public static void initializeDatabase()
        {
            if (!File.Exists(dbFile))
            {
                SQLiteConnection.CreateFile(dbFile);
            }

            using (var connection = GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                        CREATE TABLE IF NOT EXISTS Kullanicilar (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            KullaniciAdi TEXT NOT NULL UNIQUE,
                            Sifre TEXT NOT NULL,
                            AdSoyad TEXT NOT NULL,
                            Yetki INTEGER NOT NULL DEFAULT 0
                        );

                        CREATE TABLE IF NOT EXISTS Araclar (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            SasiNo TEXT NOT NULL UNIQUE,
                            Plaka TEXT NOT NULL,
                            Marka TEXT NOT NULL,
                            Model TEXT NOT NULL,
                            Yil INTEGER NOT NULL,
                            MusteriAdSoyad TEXT NOT NULL,
                            MusteriTelefon TEXT NOT NULL,
                            SonKilometre INTEGER NOT NULL DEFAULT 0
                        );

                        CREATE TABLE IF NOT EXISTS Kayitlar (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            AracId INTEGER NOT NULL,
                            GirisTarihi TEXT NOT NULL,
                            CikisTarihi TEXT,
                            GuncelKilometre INTEGER NOT NULL,
                            ArizaKodu TEXT,
                            MusteriSikayeti TEXT,
                            TeknisyenNotu TEXT,
                            ToplamTutar REAL NOT NULL DEFAULT 0,
                            Durum TEXT NOT NULL DEFAULT 'Teslim Alındı',
                            FOREIGN KEY (AracId) REFERENCES Araclar(Id) ON DELETE RESTRICT
                        );

                        CREATE TABLE IF NOT EXISTS Hasarlar (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            KayitId INTEGER NOT NULL,
                            X INTEGER NOT NULL,
                            Y INTEGER NOT NULL,
                            HasarTipi TEXT NOT NULL,
                            Aciklama TEXT,
                            FOREIGN KEY (KayitId) REFERENCES Kayitlar(Id) ON DELETE CASCADE
                        );

                        CREATE TABLE IF NOT EXISTS Katalog (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            ParcaKodu TEXT NOT NULL UNIQUE,
                            Ad TEXT NOT NULL,
                            IslemTipi TEXT NOT NULL,
                            StokAdedi REAL NOT NULL DEFAULT 0,
                            RezerveAdet REAL NOT NULL DEFAULT 0,
                            BirimFiyat REAL NOT NULL DEFAULT 0
                        );

                        CREATE TABLE IF NOT EXISTS Islemler (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            KayitId INTEGER NOT NULL,
                            KatalogId INTEGER,
                            IslemTipi TEXT NOT NULL,
                            Aciklama TEXT NOT NULL,
                            Miktar REAL NOT NULL DEFAULT 1,
                            BirimFiyat REAL NOT NULL DEFAULT 0,
                            FOREIGN KEY (KayitId) REFERENCES Kayitlar(Id) ON DELETE CASCADE,
                            FOREIGN KEY (KatalogId) REFERENCES Katalog(Id) ON DELETE SET NULL
                        );
                ";
                command.ExecuteNonQuery();
            }
        }
    }
}
