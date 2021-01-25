using Api_UploadFileLog.Controllers;
using Api_UploadFileLog.Entidades;
using Api_UploadFileLog.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Api_UploadFileLog.Tests.Controllers
{
    [TestClass]
    public class AnexosControllerTests
    {
        private readonly Mock<ILogRepository> _logRepositoryMock = new Mock<ILogRepository>();

        public AnexosControllerTests()
        {

        }

        private AnexosController CreateTestSubject()
        {
            return new AnexosController(_logRepositoryMock.Object);
        }

        [TestMethod]
        public void PostFile_DeveRetornarLista_QuandoNaoHaErrosNoArquivo()
        {
            AnexosController anexoController = this.CreateTestSubject();
            _logRepositoryMock.Setup(m => m.AddList(It.IsAny<List<Log>>())).Returns(1);

            Log modelEnvio = new Log(
                                        0,
                                        "221.123.22.151",
                                        "user-identifier",
                                        "frank",
                                        Convert.ToDateTime("25/Jun/2019 19:32:10"),
                                        "-0800",
                                        "GET http://shame.example.com/bear HTTP/1.0",
                                        200,
                                        0,
                                        "http://mom.com/caveigem",
                                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36"
                );

            List<Log> lstRetornoEsperado = new List<Log>();
            lstRetornoEsperado.Add(modelEnvio);

            FormFile file;
            string path = @"../../../File/batchCorreto.log";

            using (var stream = File.OpenRead(path))
            {
                file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary()
                };
                IActionResult resultado = anexoController.PostFile(file);

                Assert.AreEqual(typeof(ObjectResult), resultado.GetType());

                string jsonRetorno = JsonSerializer.Serialize((resultado as ObjectResult).Value);
                string jsonEsperado = JsonSerializer.Serialize(new ObjectResult(lstRetornoEsperado).Value);
                Assert.AreEqual(jsonEsperado, jsonRetorno);
            }

            _logRepositoryMock.Verify(m =>
                m.AddList(It.IsAny<List<Log>>()),
                Times.Once());
        }

        [TestMethod]
        public void PostFile_DeveRetornarErroDeExtensao_QuandoNaoForExtensaoLog()
        {
            AnexosController anexoController = this.CreateTestSubject();

            string mensagemRetorno = "Arquivo com extensão inválida.";

            FormFile file;
            string path = @"../../../File/BatExtensaoInvalida.pdf";

            using (var stream = File.OpenRead(path))
            {
                file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary()
                };
                IActionResult resultado = anexoController.PostFile(file);

                Assert.AreEqual(typeof(ObjectResult), resultado.GetType());

                string jsonRetorno = JsonSerializer.Serialize((resultado as ObjectResult).Value);
                string jsonEsperado = JsonSerializer.Serialize(new ObjectResult(mensagemRetorno).Value);
                Assert.AreEqual(jsonEsperado, jsonRetorno);
            }
        }

        [TestMethod]
        public void PostFile_DeveRetornarErroDadosInvalidos_QuandoDadosForInvalidos()
        {
            AnexosController anexoController = this.CreateTestSubject();

            StringBuilder sb = new StringBuilder().AppendLine("Erro Linha 1: Ip Inválido,Data Inválida (dd/MMM/yyyy HH:mm:ss)");

            FormFile file;
            string path = @"../../../File/batchDataInvalida.log";

            using (var stream = File.OpenRead(path))
            {
                file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary()
                };
                IActionResult resultado = anexoController.PostFile(file);

                Assert.AreEqual(typeof(ObjectResult), resultado.GetType());

                string jsonRetorno = JsonSerializer.Serialize((resultado as ObjectResult).Value);
                string jsonEsperado = JsonSerializer.Serialize(new ObjectResult(sb.ToString()).Value);
                Assert.AreEqual(jsonEsperado, jsonRetorno);
            }
        }

    }
}
