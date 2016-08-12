using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using SimPeg.Model;
using SimPeg.Repository.Api;
using SimPeg.Repository.Service;

namespace SimPeg.Repository.Service.UnitTest
{
    [TestFixture]
    public class PegawaiRepositoryTest
    {
        private IDapperContext _context;
        private IPegawaiRepository _pegawaiRepo;

        [SetUp]
        public void Init()
        {
            ResetDatabase();

            _context = new DapperContext();
            _pegawaiRepo = new PegawaiRepository(_context);
        }

        private void ResetDatabase()
        {
            // contoh string koneksi menggunakan database SQL Server
            var connectionString = @"Data Source=(local)\SQLSERVER2014;Initial Catalog=SimPeg;Integrated Security=SSPI;";
            var mySqlDatabase = new NDbUnit.Core.SqlClient.SqlDbUnitTest(connectionString);

            mySqlDatabase.ReadXmlSchema(@"..\..\SimPeg.xsd");
            mySqlDatabase.ReadXml(@"..\..\Pegawai.xml");

            mySqlDatabase.PerformDbOperation(NDbUnit.Core.DbOperationFlag.CleanInsert);
        }

        [Test]
        public void GetByIDTest()
        {
            var nik = "12345";
            var pegawai = _pegawaiRepo.GetByID(nik);

            Assert.IsNotNull(pegawai);
            Assert.AreEqual("12345", pegawai.Nik);
            Assert.AreEqual("Janoe Hendarto, M.Kom", pegawai.Nama);
            Assert.AreEqual("Seturan Yogyakarta", pegawai.Alamat);
            Assert.AreEqual("Yogyakarta", pegawai.Kota);
        }

        [Test]
        public void GetAllTest()
        {
            var listOfPegawai = _pegawaiRepo.GetAll();
            Assert.AreEqual(3, listOfPegawai.Count);

            var index = 2;
            var pegawai = listOfPegawai[index];

            Assert.IsNotNull(pegawai);
            Assert.AreEqual("12346", pegawai.Nik);
            Assert.AreEqual("Vina Zahrotun Kamila, M.Kom", pegawai.Nama);
            Assert.AreEqual("Jl. Sunan Kudus", pegawai.Alamat);
            Assert.AreEqual("Semarang", pegawai.Kota);
        }

        [TearDown]
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
